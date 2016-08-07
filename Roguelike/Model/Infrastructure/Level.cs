using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework.Audio;
using Roguelike.Model.GameObjects;
using Roguelike.Model.Lighting;
using Roguelike.util;
using Roguelike.Model.GameObjects.Monsters;
using Roguelike.Model.Lighting.Shape;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Util;
using Roguelike.Sound;
using Roguelike.Model.GameObjects.Interactables;
using Roguelike.Model.GameObjects.Loot;
using Roguelike.View;
using Roguelike.Model.GameObjects.Monsters.Randomized;
using Roguelike.Model.GameObjects.Pickups;
using Roguelike.Model.LevelGeneration;

namespace Roguelike.Model.Infrastructure
{
    public class Level
    {
        public Model gameModel;

        // levels are 0-indexed
        public int LevelNumber;
        public bool levelComplete = false;

        private static int[] levelDimensionsX = new int[] { 25, 28, 35, 39, 45 };
        private static int[] levelDimensionsY = new int[] { 25, 28, 35, 39, 45 };
        private static int[] levelRecursionDepths = new int[] { 3, 3, 4, 4, 5 };

        /* This assumes that the above three level sizes are the same length */
        public static int maxLevels = levelDimensionsX.Length - 1;

        /* Keeps track of ingame position */
        public int gamePosX = 100 * 8;
        public int gamePosY = 100 * 8;

        public bool Ending  {get; set; }
        public double teleportTimer = 0;

        /* Reference to the representation of the world */
        public World theWorld;

        /* Main character gameObject (separate so we can make sure it is
         * drawn before all other non-background gameObjects) */
        public MainCharacter mainChar;
        public AdvanceLevelInteractable portal;
        //public LightingMain lighting;

        /* GameObject HashSets for layered drawing */
        private Dictionary<LayerType, HashSet<GameObject>> layers;
        private HashSet<GameObject> worldLayer;
        private HashSet<GameObject> worldTextureLayer;
        private HashSet<GameObject> stuffLayer;
        private HashSet<GameObject> playerLayer;
        private HashSet<GameObject> abovePlayerLayer;
        private HashSet<GameObject> shadowrealm;
        private HashSet<GameObject> lightingLayer;
        private HashSet<GameObject> HUDLayer;
        private HashSet<GameObject> toAddWorld;
        private HashSet<GameObject> toAddWorldTexture;
        private HashSet<GameObject> toAddStuff;
        private HashSet<GameObject> toAddPlayer;
        private HashSet<GameObject> toAddAbovePlayer;
        private HashSet<GameObject> toAddHUD;
        private HashSet<GameObject> toRemove;

        public HashSet<GameObject> castsShadowsStatic;
        public HashSet<GameObject> castsShadowsDynamic;
        public HashSet<GameObject> castsLights;

        public LineIntersection intersection;

        public StatsInstance playerStatsInstance;

        public DebugHUD hud;

        public SoundEffect currentSong;
        public SoundEffectInstance songInstance;

        public bool walkTutorial = false;
        public bool shootTutorial = false;
        public bool flareTutorial = false;

        SoundEffectsManager soundsManager;
        RandomMonsterGenerator RMG;

        private bool escapeKeyDown = false;
        private bool gameWasPaused = false;

        public PickupGenerator pickupGen;

        public double playthroughStartTimeSecs = -1;

        /// <summary>
        /// First level constructor
        /// </summary>
        /// <param name="modelIn">The model that this object will reference</param>
        /// <param name="levelNumberIn">The number of this level (0-indexed)</param>
        private Level(Model modelIn)
        {
            LevelNumber = 0;
            gameModel = modelIn;
            walkTutorial = true;
            shootTutorial = true;
            flareTutorial = true;

            FirstLevelInitialize();

            songInstance.Play();
        }

        private Level(Level oldLevel)
        {
            LevelNumber = oldLevel.LevelNumber + 1;
            gameModel = oldLevel.gameModel;

            playthroughStartTimeSecs = oldLevel.playthroughStartTimeSecs;

            AllInitialize();

            playerStatsInstance = oldLevel.playerStatsInstance;
            mainChar = oldLevel.mainChar;
            mainChar.setNewLevel(this);
            this.addGameObject(mainChar);

            theWorld.SubsequentLevelInitialize();
            mainChar.worldCenter.X = gamePosX;
            mainChar.worldCenter.Y = gamePosY;
            mainChar.boundingBox.X = gamePosX;
            mainChar.boundingBox.Y = gamePosY;

            // place squids
            foreach (Tuple<MonsterEntry, int, int> s in theWorld.squidSquad)
            {
                foreach (AMonster m in s.Item1.GenerateMonster(this, s.Item3 * 100 + 50, s.Item2 * 100 + 50))
                {
                    this.addGameObject(m);
                }
            }

            hud = oldLevel.hud;
            hud.Reinitialize(this);

            songInstance.Play();
        }

        public static Level createFirstLevel(Model modelIn)
        {
            return new Level(modelIn);
        }

        private void AllInitialize()
        {
            Ending = false;

            /* Initialize the layers */
            layers = new Dictionary<LayerType, HashSet<GameObject>>();
            worldLayer = new HashSet<GameObject>();
            worldTextureLayer = new HashSet<GameObject>();
            stuffLayer = new HashSet<GameObject>();
            playerLayer = new HashSet<GameObject>();
            abovePlayerLayer = new HashSet<GameObject>();
            shadowrealm = new HashSet<GameObject>();
            lightingLayer = new HashSet<GameObject>();
            HUDLayer = new HashSet<GameObject>();
            layers.Add(LayerType.World, worldLayer);
            layers.Add(LayerType.WorldTexture, worldTextureLayer);
            layers.Add(LayerType.Stuff, stuffLayer);
            layers.Add(LayerType.Player, playerLayer);
            layers.Add(LayerType.AbovePlayer, abovePlayerLayer);
            layers.Add(LayerType.HUD, HUDLayer);

            toAddWorld = new HashSet<GameObject>();
            toAddWorldTexture = new HashSet<GameObject>();
            toAddStuff = new HashSet<GameObject>();
            toAddPlayer = new HashSet<GameObject>();
            toAddAbovePlayer = new HashSet<GameObject>();
            toAddHUD = new HashSet<GameObject>();
            toRemove = new HashSet<GameObject>();

            castsShadowsStatic = new HashSet<GameObject>();
            castsShadowsDynamic = new HashSet<GameObject>();
            castsLights = new HashSet<GameObject>();

            /* Initialize util stuff */
            intersection = new LineIntersection();

            /* Initialize the world and main character */
            theWorld = new World(levelDimensionsX[LevelNumber], levelDimensionsY[LevelNumber], levelRecursionDepths[LevelNumber], this);

            currentSong = gameModel.Game.Content.Load<SoundEffect>("Sound/Amiss");
            songInstance = currentSong.CreateInstance();
            songInstance.Volume = 0.2f;
            songInstance.IsLooped = true;

            soundsManager = new SoundEffectsManager(gameModel);
            RMG = new RandomMonsterGenerator(gameModel);

            pickupGen = new PickupGenerator();
        }

        private void FirstLevelInitialize()
        {
            AllInitialize();

            playerStatsInstance = gameModel.skillTree.makeInstance(this);
            mainChar = new MainCharacter(this, 0, 0, playerStatsInstance);
            this.addGameObject(mainChar);
            theWorld.FirstLevelInitialize();
            mainChar.worldCenter.X = gamePosX;
            mainChar.worldCenter.Y = gamePosY;
            mainChar.boundingBox.X = gamePosX;
            mainChar.boundingBox.Y = gamePosY;

            mainChar.LoadContent();

            // place squids
            foreach (Tuple<MonsterEntry, int, int> s in theWorld.squidSquad)
            {
                foreach (AMonster m in s.Item1.GenerateMonster(this, s.Item3 * 100 + 50, s.Item2 * 100 + 50))
                {
                    this.addGameObject(m);
                }
            }

            hud = new DebugHUD(this);
        }

        public void Update(GameTime gameTime)
        {
            if (playthroughStartTimeSecs == -1)
            {
                playthroughStartTimeSecs = gameTime.TotalGameTime.TotalSeconds;
            }

            if (!Ending)
            {
                #region hashSets
                /* Update the GameObject Hashsets */
                worldLayer.UnionWith(toAddWorld);
                worldTextureLayer.UnionWith(toAddWorldTexture);
                stuffLayer.UnionWith(toAddStuff);
                playerLayer.UnionWith(toAddPlayer);
                abovePlayerLayer.UnionWith(toAddAbovePlayer);
                //shadowrealm.UnionWith(toAddShadow);
                //lightingLayer.UnionWith(toAddLighting);
                HUDLayer.UnionWith(toAddHUD);
                foreach (GameObject objectToRemove in toRemove)
                {
                    objectToRemove.RemoveFromATiles();
                    foreach (LayerType layerNumber in Enum.GetValues(typeof(LayerType)))
                    {
                        if (layerNumber != LayerType.None)
                        {
                            layers[layerNumber].Remove(objectToRemove);
                        }
                    }
                    castsShadowsStatic.Remove(objectToRemove);
                    castsShadowsDynamic.Remove(objectToRemove);
                    castsLights.Remove(objectToRemove);
                }

                /* Clear the toAdd and toRemove HashSets */
                toAddWorld.Clear();
                toAddWorldTexture.Clear();
                toAddStuff.Clear();
                toAddPlayer.Clear();
                toAddAbovePlayer.Clear();
                //toAddShadow.Clear();
                //toAddLighting.Clear();
                toAddHUD.Clear();
                toRemove.Clear();
                #endregion hashSets

                ComputeDirections(gameTime);
                ApplyForces(gameTime);
                TakeMovements(gameTime);

                // update player
                foreach (GameObject g in playerLayer)
                {
                    g.Update(gameTime);
                }


                foreach (LayerType layerNumber in Enum.GetValues(typeof(LayerType)))
                {
                    if (layerNumber != LayerType.None && layerNumber != LayerType.Player)
                    {
                        HashSet<GameObject> singleLayer = layers[layerNumber];
                        foreach (GameObject g in singleLayer)
                        {
                            g.Update(gameTime);
                        }
                    }
                }

                if (walkTutorial)
                {
                    if (/*playerStatsInstance.distanceTraveled > 150 ||*/ gameTime.TotalGameTime.TotalSeconds - playthroughStartTimeSecs > 3)
                    {
                        walkTutorial = false;
                    }
                }

                if (shootTutorial)
                {
                    if (gameTime.TotalGameTime.TotalSeconds - playthroughStartTimeSecs > 6)
                    {
                        shootTutorial = false;
                    }
                }

                if (flareTutorial)
                {
                    if (gameTime.TotalGameTime.TotalSeconds - playthroughStartTimeSecs > 9)
                    {
                        flareTutorial = false;
                    }
                }
                

                soundsManager.Update((int)gameTime.TotalGameTime.TotalMilliseconds);
                selectiveUpdate(gameTime);
            }

            else
            {
                if (teleportTimer == 0)
                {
                    teleportTimer = gameTime.TotalGameTime.TotalMilliseconds;
                }

                if (gameTime.TotalGameTime.TotalMilliseconds - teleportTimer > 3000)
                {
                    levelComplete = true;
                }

                mainChar.LevelEndUpdate(gameTime);
            }
        }

        private void ComputeDirections(GameTime gameTime)
        {
            // update player
            foreach (GameObject g in playerLayer)
            {
                g.ComputeVelocity(gameTime);
            }


            foreach (LayerType layerNumber in Enum.GetValues(typeof(LayerType)))
            {
                if (layerNumber != LayerType.None && layerNumber != LayerType.Player)
                {
                    HashSet<GameObject> singleLayer = layers[layerNumber];
                    foreach (GameObject g in singleLayer)
                    {
                        g.ComputeVelocity(gameTime);
                    }
                }
            }
        }

        private void ApplyForces(GameTime gameTime)
        {
            // update player
            foreach (GameObject g in playerLayer)
            {
                g.ApplyForceToOtherObjects(gameTime);
            }


            foreach (LayerType layerNumber in Enum.GetValues(typeof(LayerType)))
            {
                if (layerNumber != LayerType.None && layerNumber != LayerType.Player)
                {
                    HashSet<GameObject> singleLayer = layers[layerNumber];
                    foreach (GameObject g in singleLayer)
                    {
                        g.ApplyForceToOtherObjects(gameTime);
                    }
                }
            }
        }

        private void TakeMovements(GameTime gameTime)
        {
            // update player
            foreach (GameObject g in playerLayer)
            {
                g.Move(gameTime);
            }


            foreach (LayerType layerNumber in Enum.GetValues(typeof(LayerType)))
            {
                if (layerNumber != LayerType.None && layerNumber != LayerType.Player)
                {
                    HashSet<GameObject> singleLayer = layers[layerNumber];
                    foreach (GameObject g in singleLayer)
                    {
                        g.Move(gameTime);
                    }
                }
            }
        }

        public void selectiveUpdate(GameTime gameTime)
        {
            #region keyEvents
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                escapeKeyDown = true;
            }

            if (escapeKeyDown)
            {
                if (Keyboard.GetState().IsKeyUp(Keys.Escape))
                {
                    escapeKeyDown = false;

                    if (gameWasPaused)
                    {
                        //unpause game
                        gameModel.gameState = GameState.Game;
                        gameWasPaused = false;
                    }

                    else
                    {
                        gameModel.gameState = GameState.PauseScreen;
                        gameModel.refreshPauseScreen();
                        gameWasPaused = true;
                    }
                }
            }
            #endregion keyEvents
        }

        public void addGameObject(GameObject g)
        {
            LayerType layerType = g.layerType;

            if (layerType == LayerType.None)
            {
                throw new Exception("Need to specify an object LayerType");
            }

            switch (layerType)
            {
                case LayerType.World:
                    toAddWorld.Add(g);
                    break;
                case LayerType.WorldTexture:
                    toAddWorldTexture.Add(g);
                    break;
                case LayerType.Stuff:
                    toAddStuff.Add(g);
                    break;
                case LayerType.Player:
                    toAddPlayer.Add(g);
                    break;
                case LayerType.AbovePlayer:
                    toAddAbovePlayer.Add(g);
                    break;
                //case LayerType.Shadow:
                //    toAddShadow.Add(g);
                //    break;
                //case LayerType.Lighting:
                //   toAddLighting.Add(g);
                //    break;
                case LayerType.HUD:
                    toAddHUD.Add(g);
                    break;
                default:
                    toAddStuff.Add(g);
                    break;
            }

            if (g.CastsLight())
            {
                castsLights.Add(g);
            }

            if (g.CastsShadow())
            {
                // Added AbovePlayer to count in doors.
                if (g.layerType == LayerType.World || g.layerType == LayerType.AbovePlayer)
                {
                    castsShadowsStatic.Add(g);
                }
                else
                {
                    castsShadowsDynamic.Add(g);
                }
            }
        }

        public void removeGameObject(GameObject g)
        {
            toRemove.Add(g);
        }

        /// <summary>
        /// Gets the ATile reference that Vector2 would be ontop of
        /// </summary>
        /// <param name="point">the Vector2 location that you want the ATile of</param>
        public ATile GetATileAt(Vector2 point)
        {
            int x = (int)Math.Floor(point.X / 100);
            int y = (int)Math.Floor(point.Y / 100);

            return theWorld.getATile(x, y);
        }
        public ATile[,] GetTileMatrix()
        {
            return theWorld.getATileMatrix();
        }

        public Dictionary<LayerType, HashSet<GameObject>> getGameObjects()
        {
            return layers;
        }

        public Level AdvanceLevel()
        {
            songInstance.Stop();

            Level l = new Level(this);

            return l;
        }


        internal void spawnRandomMonster(int xPos, int yPos)
        {
            addGameObject(RMG.generateMonster(this, xPos, yPos));
        }

        internal void GameOver()
        {
            songInstance.Stop();
        }

        /// <summary>
        /// Given a world location and convert to a set of coordinates for the ATile matrix
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static System.Drawing.Point GetMatrixCoordAt(Vector2 point)
        {
            // integer magic
            int x = (int)Math.Floor(point.X / 100);
            int y = (int)Math.Floor(point.Y / 100);

            return new System.Drawing.Point(x, y);
        }
    }
}
