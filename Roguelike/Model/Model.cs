#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.VisualBasic;
using Roguelike.Model.GameObjects;
//using Roguelike.Model.Lighting;
using Roguelike.util;
using Roguelike.Model.GameObjects.Monsters;
//using Roguelike.Model.Lighting.Shape;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Util;
using Roguelike.Sound;
using Roguelike.Model.GameObjects.Interactables;
using Roguelike.Model.GameObjects.Loot;
using Roguelike.Model.Infrastructure;
using Roguelike.View;
using Roguelike.Menus;
using Roguelike.Model.GameObjects.Projectiles;
#endregion

namespace Roguelike.Model
{
    public enum LayerType
    {
        World,
        WorldTexture,
        Stuff,
        Player,
        AbovePlayer,
        HUD,
        None
    }

    public class Model
    {
        //private XNAGameConsole.GameConsole console;

        /* A reference to the Game class*/
        public Roguelike Game;
        public View.View gameView;

        public bool paused = false;
        public bool pauseScreenActive = false;
        public bool lightingEnabled = true;
        
        public BasicEffect be;
        public GameState gameState;

        /* Set if the movemeng keys are mapped to dvorak or qwerty*/
        public bool dvorak = true;
        
        private string saveFilename = "db3.sav";
        public SkillTreeStats skillTree;
        public Level currentLevel;

        public Menus.IntroSequence splashScreen;
        public Menus.EndScreen endScreen;
        public Menus.MenuScreen menuScreen;
        public Menus.SkillScreen skillScreen;
        public Menus.PauseScreen pauseScreen;

        public LootGenerator g;

        private GameServiceContainer service;

        public Model(Roguelike game, ref SpriteBatchWrapper s, GameServiceContainer serviceProvider)
        {
            Game = game;
            gameState = GameState.SplashScreen;

            s = new SpriteBatchWrapper(game.GraphicsDevice, this);

            service = serviceProvider;

            //initializeConsole(s.s);

            skillTree = SkillTreeStats.LoadInstanceFromFile(saveFilename);

            if (skillTree == null)
            {
                ConsoleWriteLine("Init: Save file load failed. Using default skill tree.");
                skillTree = SkillTreeStats.MakeDefaultSkillTree();
            }

            else
            {
                ConsoleWriteLine("Init: Save file loaded from disk");
            }

            String layout = InputLanguage.CurrentInputLanguage.LayoutName.ToLower();
            ConsoleWriteLine("Init: Keyboard layout detected as \"" + layout + "\"");

            if (layout.Contains("qwerty"))
            {
                ConsoleWriteLine("Init: QWERTY layout detected");
                dvorak = false;
            }

            else if (layout.Contains("dvorak"))
            {
                ConsoleWriteLine("Init: Dvorak layout detected");
                dvorak = true;
            }

            else
            {
                ConsoleWriteLine("Init: Layout not detected (could be QWERTY). Defaulting to QWERTY");
                dvorak = false;
            }
        }

        public void Initialize()
        {
            splashScreen = new Menus.IntroSequence(this, Game.getGraphics());
            pauseScreen = new PauseScreen(this);
            splashScreen.LoadContent(Game);
        }

        public void setView(View.View view)
        {
            gameView = view;
            //be = new StandardBasicEffect(gameView.graphics.GraphicsDevice);
            //be = new BasicEffect(gameView.graphics.GraphicsDevice);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            switch (gameState)
            {
                case GameState.Demo:
                    if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Space))
                    {
                        gameState = GameState.SplashScreen;
                    }
                    break;
                case GameState.SplashScreen:
                    splashScreen.Update(gameTime);
                    break;
                case GameState.Menu:
                    menuScreen.Update(gameTime);
                    break;
                case GameState.SkillScreen:
                    skillScreen.Update(gameTime);
                    break;
                case GameState.Game:
                    /* Update the main game */
                    UpdateGame(gameTime);
                    break;
                case GameState.PauseScreen:
                    currentLevel.selectiveUpdate(gameTime);
                    break;
                case GameState.EndGame:
                    endScreen.Update(gameTime);
                    break;
                default:
                    break;
            }
        }

        private void UpdateGame(GameTime gameTime) {
            if (pauseScreenActive)
            {
                paused = true;
            }

            else
            {
                paused = false;
            }

            if (!paused)
            {
                if (currentLevel.levelComplete) {
                    if (currentLevel.LevelNumber < Level.maxLevels)
                    {
                        currentLevel = currentLevel.AdvanceLevel();
                        gameView.AdvanceLevel();
                        
                    }

                    else
                    {
                        endGame(true, "You made it to the end!");
                    }
                    
                }
                currentLevel.Update(gameTime);

                /* Let View update frames */
                gameView.Update(gameTime);
            }

            else
            {
                currentLevel.selectiveUpdate(gameTime);
            }
        }

        public void startMenu()
        {
            switch (gameState)
            {
                case GameState.SplashScreen:
                    splashScreen.ExitSplashScreen();
                    menuScreen = new MenuScreen(this, Game.getGraphics());
                    break;
                case GameState.SkillScreen:
                    menuScreen = new MenuScreen(this, Game.getGraphics());
                    skillScreen.ExitSkillScreen();
                    break;
                case GameState.EndGame:
                    menuScreen = new MenuScreen(this, Game.getGraphics());
                    endScreen.ExitEndScreen();
                    break;
                default:
                    break;
            }
            gameState = GameState.Menu;
        }

        public void startSkillScreen()
        {
            skillScreen = new SkillScreen(this, Game.getGraphics(), skillTree);
            menuScreen.ExitMenuScreen();
            gameState = GameState.SkillScreen;
        }

        public void startGame()
        {
            gameState = GameState.Game;

            //init level
            g = new LootGenerator();
            currentLevel = Level.createFirstLevel(this, service);

            BulletPool.Init(currentLevel);

            gameView.StartGameInitialize();
        }

        public void endGame(bool success, string killText)
        {
            currentLevel.GameOver();
            endScreen = new EndScreen(this, Game.getGraphics(), success, killText);
            gameState = GameState.EndGame;

        }

        public void ConsoleWriteLine(string s)
        {
            //console.WriteLine(s);
        }

        public View.View getView()
        {
            return gameView;
        }

        //internal void initializeConsole(SpriteBatch spriteBatch)
        //{
        //    // instantiation of commands
        //    var commands = new IConsoleCommand[] { 
        //        new SetCommand(this),
        //        new SpawnItemCommand(this),
        //        new SpawnMonsterCommand(this),
        //        new ModLightCommand(this),
        //        new MakePolygonCommand(this),
        //        new ModHUDCommand(this),
        //        new PlayTestSoundCommand(this),
        //        new KeymapCommand(this),
        //        new NoClipCommand(this), 
        //        new SpawnItemByIdCommand(this), 
        //        new ToastCommand(this), 
        //        new RevealMapCommand(this),
        //        new AddSkillPointsCommand(this), 
        //        new SpawnRandomMonsterCommand(this), 
        //        new SpawnSpiderPodCommand(this),
        //        new SpawnPickupByIdCommand(this), 
        //        new DebugModeCommand(this), 
        //        new SkipToLevelCommand(this)
        //    };

        //    console = new GameConsole(Game, spriteBatch, commands, new GameConsoleOptions
        //    { // Console options
        //        Height = 800, 
        //        OpenOnWrite = false
        //    });
        //}

        public void refreshPauseScreen()
        {
            pauseScreen.CurrentLevel = currentLevel;
        }

        internal void onExit()
        {
            Console.WriteLine("save here");
            skillTree.SaveStatsToFile(saveFilename);
        }
    }
}
