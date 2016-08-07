#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;
using Roguelike.View;
//using Roguelike.Util.ConsoleCommands;
using Roguelike.Menus;
//using Ziggyware;
//using Shadows2D;
#endregion

namespace Roguelike
{
    public enum GameState
    {
        Demo,
        SplashScreen,
        Menu,
        SkillScreen,
        Game,
        EndGame,
        PauseScreen,
        None
    }

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Roguelike : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        public SpriteBatchWrapper spriteBatch;

        View.View gameView;
        Model.Model gameModel;

        public ContentManager content;

        Random random = new Random();
        private PenumbraComponent penumbra;

        public Roguelike()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.GraphicsProfile = GraphicsProfile.HiDef;

            /* Set a default resolution */
            graphics.PreferredBackBufferHeight = 900;
            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8;
            graphics.SynchronizeWithVerticalRetrace = true;

            //graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            this.content = Content;

            // Create our lighting component and register it as a service so that subsystems can access it.
            penumbra = new PenumbraComponent(this)
            {
                AmbientColor = new Color(new Vector3(0.1f))
            };
            Services.AddService(penumbra);

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            /* Create the View and the Model */
            gameView = new View.View(this, graphics, Services);
            gameModel = new Model.Model(this, ref spriteBatch, Services);
            gameModel.Initialize();

            gameView.setModel(gameModel);
            gameModel.setView(gameView);

            //spriteBatch = new SpriteBatchWrapper(GraphicsDevice, gameModel);
            Services.AddService(typeof(SpriteBatchWrapper), spriteBatch);
            penumbra.Initialize();
            gameView.Initialize();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            //TODO
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            gameModel.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.Black);

            gameView.Draw(gameTime, gameModel.gameState);

            base.Draw(gameTime);
        }

        /// <summary>
        /// Used to pass graphics out to lighting.
        /// </summary>
        /// <returns></returns>
        public GraphicsDeviceManager getGraphics()
        {
            return graphics;
        }

        protected override void OnExiting(Object sender, EventArgs args)
        {
            gameModel.onExit();
        }
    }
}
