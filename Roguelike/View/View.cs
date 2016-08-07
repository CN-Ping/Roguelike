#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Model.GameObjects;
using Roguelike.Model;
using Shadows2D;
#endregion

namespace Roguelike.View
{

    public class View
    {

        /* A reference to the Game class*/
        Roguelike Game;

        public Model.Model gameModel;

        private int screenWidth_;
        public int ScreenWidth { get { return this.screenWidth_; } }

        private int screenHeight_;
        public int ScreenHeight { get { return this.screenHeight_; } }

        private int screenWidthOver2_;
        public int ScreenWidthOver2 { get { return this.screenWidthOver2_; } }

        private int screenHeightOver2_;
        public int ScreenHeightOver2 { get { return this.screenHeightOver2_; } }

        public GraphicsDeviceManager graphics;
        public SpriteBatchWrapper spriteBatch;
        public Viewport viewport;

        private Background background;
        private HUD gameHUD;

        /* More shit to organize*/
        LightsFX lightsFX;
        public ShadowMapResolver shadowmapResolverA;
        ShadowCasterMap shadowMapWithObstacles;
        ShadowCasterMap shadowMapWithoutObstacles;

        ShadowCasterMap shadowMapTransparency;

        RenderTarget2D screenLightsWith;
        RenderTarget2D screenLightsWithout;
        RenderTarget2D screenGroundWith;
        RenderTarget2D screenGroundWithout;

        RenderTarget2D screenShadowTransparency;

        public Color[] colorData;

        public Vector2 screenCenter;

        String currentToastText = "";
        String currentBabyToastText = "";
        long toastTimer;
        int toastDuration = 1500;
        bool toasting = true;
        bool fadingIn = true;
        Color toastColour = new Color(255, 255, 255, 0);
        Vector2 toastPosition;
        Vector2 babyToastPosition;
        Vector2 toastOrigin;
        Vector2 babyToastOrigin;
        SpriteFont toastFont;
        SpriteFont babyToastFont;

        public Minimap minimap;

        private Texture2D walkTutorial;
        private Texture2D shootTutorial;
        private Texture2D flareTutorial;
        private Texture2D currentTutorial;

        private Vector2 gameTutorialOrigin;
        private Color gameTutorialColour = new Color(255, 255, 255, 200);
        private int DrawingTutorial = 0;
        private Vector2 gameTutorialLocation;

        private Texture2D teleportAura;
        private Rectangle teleportDestination;
        private Texture2D whiteScreen;
        private Vector2 whiteLocation = new Vector2(-10, -10);
        public Color whiteTransparency = new Color(255, 255, 255, 0);
        private int goWhite = 1;

        private int shouldIGetData = 0;
        private int shouldIGetDataMod = 20;

        Texture2D torchAlert;
        Vector2 torchAlertPosition;

        public View(Roguelike game, GraphicsDeviceManager inGraphics)
        {
            Game = game;
            graphics = inGraphics;
            viewport = Game.GraphicsDevice.Viewport;
        }

        public void Initialize()
        {
            // TODO this probably wont work


            screenWidth_ = viewport.Width;
            screenHeight_ = viewport.Height;

            screenWidthOver2_ = screenWidth_ / 2;
            screenHeightOver2_ = screenHeight_ / 2;

            screenCenter = new Vector2(screenWidthOver2_, screenHeightOver2_);

            toastPosition = new Vector2(ScreenWidthOver2, 5 * screenHeight_ / 6);
            babyToastPosition = new Vector2(ScreenWidthOver2, 5 * screenHeight_ / 6 + 25);

            LoadContent();
        }

        public void setModel(Model.Model model)
        {
            gameModel = model;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        public void LoadContent()
        {
            spriteBatch = new SpriteBatchWrapper(Game.GraphicsDevice, gameModel);

            //From Pinghao
            LoadDynamicLight();

            toastFont = Game.Content.Load<SpriteFont>("Arial_s24");
            babyToastFont = Game.Content.Load<SpriteFont>("Arial");

            walkTutorial = Game.Content.Load<Texture2D>("Textures/UI/walkTutorial");
            shootTutorial = Game.content.Load<Texture2D>("Textures/UI/shootTutorial");
            flareTutorial = Game.Content.Load<Texture2D>("Textures/UI/flareTutorial");
            teleportAura = Game.Content.Load<Texture2D>("Textures/teleport");
            whiteScreen = Game.Content.Load<Texture2D>("Textures/whitescreen");
            teleportDestination = new Rectangle((int)(screenCenter.X - (teleportAura.Width / 2)), (int)(screenCenter.Y - (teleportAura.Height / 2)), teleportAura.Width, teleportAura.Height);
            currentTutorial = walkTutorial;
            gameTutorialOrigin = new Vector2(walkTutorial.Width / 2, walkTutorial.Height / 2);
            gameTutorialLocation = new Vector2(ScreenWidthOver2, 3 * ScreenHeight / 4);

            torchAlert = Game.Content.Load<Texture2D>("Textures/UI/flareAlert");
            torchAlertPosition = new Vector2(ScreenWidthOver2, ScreenHeightOver2 - 100);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        public void UnloadContent()
        {
            /* Call UnloadContent on the gameObjects */
            gameModel.currentLevel.mainChar.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            long t = (long)gameTime.TotalGameTime.TotalMilliseconds;
            if (toasting)
            {
                if (fadingIn)
                {
                    if (toastColour.A < 245)
                    {
                        if (t - toastTimer > 5)
                        {
                            toastColour.A += 5;
                            toastTimer = t;
                        }
                    }

                    else
                    {
                        toastColour.A = 255;

                        if (t - toastTimer > toastDuration)
                        {
                            fadingIn = false;
                            toastTimer = t;
                        }
                    }

                }

                else
                {
                    if (toastColour.A > 10)
                    {
                        if (t - toastTimer > 5)
                        {
                            toastColour.A -= 4;
                            toastTimer = t;
                        }
                    }
                    else
                    {
                        toasting = false;
                    }
                }
            }

            if (DrawingTutorial > 0)
            {
                switch (DrawingTutorial)
                {
                    // drawing tutorial, and the game doesn't want the tutorial around. (fade it out)
                    case 5:
                        if (!gameModel.currentLevel.walkTutorial)
                        {
                            if (gameTutorialColour.A > 10)
                            {
                                gameTutorialColour.A -= 8;
                            }
                            else
                            {
                                DrawingTutorial--;
                                currentTutorial = shootTutorial;
                            }
                        }
                        break;

                    // fading in next tutorial
                    case 4:
                        if (gameTutorialColour.A < 245)
                        {
                            gameTutorialColour.A += 8;
                        }
                        else
                        {
                            DrawingTutorial--;
                        }
                        break;

                    // fading out the tutorial
                    case 3:
                        if (!gameModel.currentLevel.shootTutorial)
                        {
                            if (gameTutorialColour.A > 10)
                            {
                                gameTutorialColour.A -= 8;
                            }
                            else
                            {
                                DrawingTutorial--;
                                currentTutorial = flareTutorial;
                            }
                        }
                        break;

                    // fading in next tutorial (flare)
                    case 2 :
                        if (gameTutorialColour.A < 245)
                        {
                            gameTutorialColour.A += 8;
                        }
                        else
                        {
                            DrawingTutorial--;
                        }
                        break;

                    // fading out flare tutorial
                    case 1:
                        if (!gameModel.currentLevel.flareTutorial)
                        {
                            if (gameTutorialColour.A > 10)
                            {
                                gameTutorialColour.A -= 8;
                            }
                            else
                            {
                                DrawingTutorial--;
                            }
                        }
                        break;

                }


            }

            minimap.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Draw(GameTime gameTime, GameState g)
        {
            switch (g)
            {
                case GameState.SplashScreen:
                    gameModel.splashScreen.Draw(gameTime);
                    break;
                case GameState.Menu:
                    gameModel.menuScreen.Draw(gameTime);
                    break;
                case GameState.SkillScreen:
                    gameModel.skillScreen.Draw(gameTime);
                    break;
                case GameState.Game:
                    /* Draw the main game */
                    GameDraw(gameTime);
                    break;
                case GameState.EndGame:
                    gameModel.endScreen.Draw(gameTime);
                    break;
                case GameState.PauseScreen:
                    GameDraw(gameTime);
                    gameModel.pauseScreen.Draw(spriteBatch);
                    break;
                default:
                    // this shouldn't happen
                    break;
            }
        }

        private void GameDraw(GameTime gameTime)
        {
            if (!gameModel.currentLevel.Ending)
            {
                /* Put the shadows in. */
                DealWithShadows();

                /* Draw the HUD ontop of everything */
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                Dictionary<LayerType, HashSet<GameObject>> layers = gameModel.currentLevel.getGameObjects();
                HashSet<GameObject> HUDLayer = layers[LayerType.HUD];
                foreach (GameObject g in HUDLayer)
                {
                    g.Draw(spriteBatch);
                }

                if (gameModel.currentLevel.mainChar.torchFireMode)
                {
                    spriteBatch.s.Draw(torchAlert, torchAlertPosition, Color.White);
                }

                /* Debug hud */
                if (gameModel.currentLevel.hud.active)
                {
                    gameModel.currentLevel.hud.Draw(spriteBatch);
                }

                minimap.Draw(spriteBatch);

                spriteBatch.End();

                if (toasting)
                {
                    spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied);
                    spriteBatch.s.DrawString(toastFont, currentToastText, toastPosition, toastColour, 0f, toastOrigin, 1f, SpriteEffects.None, 1.0f);
                    spriteBatch.s.DrawString(babyToastFont, currentBabyToastText, babyToastPosition, toastColour, 0f, babyToastOrigin, 1f, SpriteEffects.None, 1.0f);
                    spriteBatch.End();
                }

                if (DrawingTutorial > 0)
                {
                    spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied);
                    spriteBatch.s.Draw(currentTutorial, gameTutorialLocation, null, gameTutorialColour, 0f, gameTutorialOrigin, 1f, SpriteEffects.None, 1f);
                    spriteBatch.End();
                }
            }

            else
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

                spriteBatch.s.Draw(teleportAura, teleportDestination, Color.White);

                gameModel.currentLevel.portal.Draw(spriteBatch);
                gameModel.currentLevel.mainChar.Draw(spriteBatch);
                gameModel.currentLevel.portal.splat.Draw(spriteBatch);

                spriteBatch.s.Draw(whiteScreen, whiteLocation, whiteTransparency);

                if ((int)whiteTransparency.A < 250)
                {
                    goWhite++;
                    if (goWhite % 2 == 0)
                        whiteTransparency.A += 1;
                }

                spriteBatch.End();
            }

        }

        private void NewShadowCode()
        {
            GenerateLightTransparency();

            DynamicShadows();
        }

        private void DynamicShadows()
        {
            foreach (GameObject g in gameModel.currentLevel.castsShadowsDynamic)
            {
                //g.GenerateExclusiveShadow();
            }
        }

        /*This is being used for now until shadows are fixed*/
        private void DealWithShadows()
        {
            GenerateLightTransparency();

            StaticShadows();
            MonstersSaveShadowLevels();
            AllShadows();
            //ShadowsWithoutObstacles();

            graphics.GraphicsDevice.SetRenderTarget(screenGroundWith);
            graphics.GraphicsDevice.Clear(Color.Black);
            DrawAllThings();

            // This command impress a texture on another using 2xMultiplicative blend, which is perfect to paste our lights on the underlying image
            this.lightsFX.PrintLightsOverTexture(null, spriteBatch.s, graphics, screenLightsWith, screenGroundWith, 0.975f);
            //using (Stream stream = File.OpenWrite("picture.png"))
            //{
            //    screenLightsWith.SaveAsPng(stream, screenLightsWith.Width, screenLightsWith.Height);
            //}
            if (shouldIGetData % shouldIGetDataMod == 0)
            {
                screenLightsWithout.GetData<Color>(colorData);
            }

            shouldIGetData = (shouldIGetData + 1) % shouldIGetDataMod;
                
            // We re-print the elements not affected by the light (in this case the shadow casters)
            DrawThingsThatCastShadows();

            this.lightsFX.PrintLightsOverTextureTransparent(null, spriteBatch.s, graphics, screenShadowTransparency, screenGroundWith, 0.975f);

            //graphics.GraphicsDevice.SetRenderTarget(screenGroundWith);
            //this.lightsFX.PrintLightsOverTexture(null, spriteBatch.s, graphics, screenLightsWithout, screenGroundWithout, 0.95f);
            //spriteBatch.End();
        }

        private void GenerateLightTransparency()
        {
            /* Create ShadowMap With Obstacles */
            this.shadowMapTransparency.StartGeneratingShadowCasteMap(false);
            //            foreach (GameObject shadowObj in gameModel.currentLevel.castsShadows)
            //            {
            //                shadowObj.DrawCaster(this.shadowMapWithObstacles);
            //            }
            this.shadowMapTransparency.EndGeneratingShadowCasterMap();

            /* Resolve lights for map with obstacles*/
            foreach (GameObject lightObject in gameModel.currentLevel.castsLights)
            {
                LightSource light = lightObject.GetLightSource();
                this.shadowmapResolverA.ResolveShadows(this.shadowMapTransparency, light, PostEffect.LinearAttenuation, light.Position);
            }

            // We print the lights in an image
            graphics.GraphicsDevice.SetRenderTarget(screenShadowTransparency);

            graphics.GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            foreach (GameObject lightObject in gameModel.currentLevel.castsLights)
            {
                lightObject.GetLightSource().Draw(spriteBatch.s);
            }
            spriteBatch.End();


            //spriteBatch.Begin(SpriteBlendMode.Additive, SpriteSortMode.Immediate, SaveStateMode.None); 
            //graphics.GraphicsDevice.RenderState.DestinationBlend = Blend.Zero;
            //graphics.GraphicsDevice.RenderState.SourceBlend = Blend.DestinationColor;
            //graphics.GraphicsDevice.RenderState.BlendFunction = BlendFunction.Add;

            //graphics.GraphicsDevice.BlendFactor = Color.Transparent;
            //graphics.GraphicsDevice.BlendState = BlendState.AlphaBlend;

            //spriteBatch.Draw(lighting, Vector2.Zero, Color.White);
            //spriteBatch.End();

        }

        /// <summary>
        /// Only drawing to the unused screenLightsWithout rendertarget, discarded after shadow data saved in monsters.
        /// </summary>
        private void StaticShadows()
        {
            /* Create ShadowMap With Obstacles */
            this.shadowMapWithObstacles.StartGeneratingShadowCasteMap(false);
            foreach (GameObject shadowObj in gameModel.currentLevel.castsShadowsStatic)
            {
                shadowObj.DrawCaster(this.shadowMapWithObstacles);
            }
            this.shadowMapWithObstacles.EndGeneratingShadowCasterMap();

            /* Resolve lights for map with obstacles*/
            foreach (GameObject lightObject in gameModel.currentLevel.castsLights)
            {
                LightSource light = lightObject.GetLightSource();
                this.shadowmapResolverA.ResolveShadows(this.shadowMapWithObstacles, light, PostEffect.LinearAttenuation_BlurHigh, light.Position);
            }

            // We print the lights in an image
            graphics.GraphicsDevice.SetRenderTarget(screenLightsWithout);

            graphics.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            foreach (GameObject lightObject in gameModel.currentLevel.castsLights)
            {
                lightObject.GetLightSource().Draw(spriteBatch.s);
            }
            spriteBatch.End();
        }

        private void AllShadows()
        {
            /* Create ShadowMap With Obstacles */
            this.shadowMapWithObstacles.StartGeneratingShadowCasteMap(false);
            foreach (GameObject shadowObj in gameModel.currentLevel.castsShadowsStatic)
            {
                shadowObj.DrawCaster(this.shadowMapWithObstacles);
            }
            foreach (GameObject shadowObj in gameModel.currentLevel.castsShadowsDynamic)
            {
                shadowObj.DrawCaster(this.shadowMapWithObstacles);
            }
            this.shadowMapWithObstacles.EndGeneratingShadowCasterMap();

            /* Resolve lights for map with obstacles*/
            foreach (GameObject lightObject in gameModel.currentLevel.castsLights)
            {
                LightSource light = lightObject.GetLightSource();
                this.shadowmapResolverA.ResolveShadows(this.shadowMapWithObstacles, light, PostEffect.LinearAttenuation_BlurHigh, light.Position);
            }

            // We print the lights in an image
            graphics.GraphicsDevice.SetRenderTarget(screenLightsWith);

            graphics.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            foreach (GameObject lightObject in gameModel.currentLevel.castsLights)
            {
                lightObject.GetLightSource().Draw(spriteBatch.s);
            }
            spriteBatch.End();
        }

        //private void ShadowsWithoutObstacles()
        //{

        //    /* Create ShadowMap Without Obstacles */
        //    this.shadowMapWithoutObstacles.StartGeneratingShadowCasteMap(false);
        //    this.shadowMapWithoutObstacles.EndGeneratingShadowCasterMap();

        //    /* Resolve lights for map without obstacles*/
        //    foreach (GameObject lightObject in gameModel.currentLevel.castsLights)
        //    {
        //        LightSource light = lightObject.GetLightSource();
        //        this.shadowmapResolverA.ResolveShadows(this.shadowMapWithoutObstacles, light, PostEffect.LinearAttenuation_BlurHigh, light.Position);
        //    }

        //    // We print the lights in an image
        //    graphics.GraphicsDevice.SetRenderTarget(screenLightsWithout);
        //    graphics.GraphicsDevice.Clear(Color.Black);
        //    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
        //    foreach (GameObject lightObject in gameModel.currentLevel.castsLights)
        //    {
        //        lightObject.GetLightSource().Draw(spriteBatch.s);
        //    }
        //    spriteBatch.End();

        //}

        /* More functions that were in GameMain*/
        private void LoadDynamicLight()
        {
            GraphicsDevice device = graphics.GraphicsDevice;
            int vpWidth = device.Viewport.Width;
            int vpHeight = device.Viewport.Height;

            /* Create a new SpriteBatch, which can be used to draw textures. */
            this.spriteBatch = new SpriteBatchWrapper(graphics.GraphicsDevice, gameModel);
            this.lightsFX = new LightsFX(
                Game.Content.Load<Effect>("resolveShadowsEffect"),
                Game.Content.Load<Effect>("reductionEffect"),
                Game.Content.Load<Effect>("2xMultiBlend"));

            this.shadowmapResolverA = new ShadowMapResolver(device, this.lightsFX, 192);
            this.shadowMapWithObstacles = new ShadowCasterMap(PrecisionSettings.VeryHigh, graphics, this.spriteBatch);
            this.shadowMapWithoutObstacles = new ShadowCasterMap(PrecisionSettings.VeryHigh, graphics, this.spriteBatch);
            this.shadowMapTransparency = new ShadowCasterMap(PrecisionSettings.VeryHigh, graphics, this.spriteBatch);
            this.screenLightsWith = new RenderTarget2D(device, vpWidth, vpHeight);
            this.screenLightsWithout = new RenderTarget2D(device, vpWidth, vpHeight);
            this.screenGroundWith = new RenderTarget2D(device, vpWidth, vpHeight);
            this.screenGroundWithout = new RenderTarget2D(device, vpWidth, vpHeight);
            this.screenShadowTransparency = new RenderTarget2D(device, vpWidth, vpHeight);

            colorData = new Color[screenLightsWithout.Width * screenLightsWithout.Height];
        }



        private void DrawThingsThatCastShadows()
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            /*foreach (GameObject shadowObj in gameModel.currentLevel.castsShadows)
            {
                shadowObj.Draw(spriteBatch);
            }*/
            /* Draw the game objects */
            Dictionary<LayerType, HashSet<GameObject>> layers = gameModel.currentLevel.getGameObjects();
            foreach (LayerType layerNumber in Enum.GetValues(typeof(LayerType)))
            {
                if (layerNumber != LayerType.None)
                {
                    HashSet<GameObject> singleLayer = layers[layerNumber];
                    foreach (GameObject g in singleLayer)
                    {
                        if (g.CastsShadow() || g.GetType() == typeof(MainCharacter))
                        {
                            g.Draw(spriteBatch);
                        }
                    }
                }
            }

            /* Draw the player here so that it is on top of the things that cast
             * shadows that are being redrawn */
            //gameModel.currentLevel.mainChar.Draw(spriteBatch);

            spriteBatch.End();
        }

        private void DrawAllThings()
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            /* Draw the background stars */
            background.Draw(spriteBatch);

            /* Draw the game objects */
            Dictionary<LayerType, HashSet<GameObject>> layers = gameModel.currentLevel.getGameObjects();
            foreach (LayerType layerNumber in Enum.GetValues(typeof(LayerType)))
            {
                if (layerNumber != LayerType.None)
                {
                    HashSet<GameObject> singleLayer = layers[layerNumber];
                    foreach (GameObject g in singleLayer)
                    {
                        g.Draw(spriteBatch);
                    }
                }
            }

            spriteBatch.End();
        }

        private void DrawGround()
        {
            GraphicsDevice device = graphics.GraphicsDevice;
            int vpWidth = device.Viewport.Width;
            int vpHeight = device.Viewport.Height;

            //draw the tile texture tiles across the screen
            Rectangle source = new Rectangle(0, 0, vpWidth, vpHeight);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone);
            //spriteBatch.Draw(tileTexture, Vector2.Zero, source, Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
            //GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            gameModel.currentLevel.theWorld.DrawFloor(spriteBatch);
            spriteBatch.End();

        }

        public void StartGameInitialize()
        {
            currentTutorial = walkTutorial;
            background = new Background(gameModel.currentLevel, Game);
            gameHUD = new HUD(this);

            minimap = new Minimap(gameModel.currentLevel);

            DrawingTutorial = 5;
            gameTutorialColour = new Color(255, 255, 255, 200);

            /* Call LoadContent on the gameObjects */
            background.LoadContent();

            gameHUD.Initialize(gameModel.currentLevel);
        }

        public void AdvanceLevel()
        {
            //background = new Background(gameModel.currentLevel, Game);

            gameHUD.Reinitialize(gameModel.currentLevel);
            background.Reinitialize(gameModel.currentLevel);
            minimap = new Minimap(gameModel.currentLevel);
            /* Call LoadContent on the gameObjects */
            //background.LoadContent();
        }

        public void Toast(String text)
        {
            currentToastText = text;
            currentBabyToastText = "";
            toastOrigin = toastFont.MeasureString(currentToastText) / 2;
            toasting = true;
            fadingIn = true;
            toastColour = new Color(255, 255, 255, 0);
        }

        /// <summary>
        /// Let every monster in the map save their shadow level
        /// </summary>
        public void MonstersSaveShadowLevels()
        {
            Dictionary<LayerType, HashSet<GameObject>> layers = gameModel.currentLevel.getGameObjects();
            HashSet<GameObject> singleLayer = layers[LayerType.Stuff];

            foreach (GameObject go in singleLayer)
            {
//                if (go.CastsShadow())
//                {
                    Vector2 screenLocation = new Vector2(go.worldCenter.X - ((int)gameModel.currentLevel.mainChar.worldCenter.X - gameModel.gameView.ScreenWidthOver2),
        go.worldCenter.Y - ((int)gameModel.currentLevel.mainChar.worldCenter.Y - gameModel.gameView.ScreenHeightOver2));

                    Color pixel = getShadowColorAtPixel((int)screenLocation.X, (int)screenLocation.Y);


                    byte r = pixel.R;
                    byte g = pixel.G;
                    byte b = pixel.B;
                    byte level = Math.Max(r, Math.Max(g, b));
                    if (level >= 85)
                    {
                        level = 255;
                    }
                    else
                    {
                        level = (byte)(level * 3);
                    }
                    pixel.R = level;
                    pixel.G = level;
                    pixel.B = level;
                    go.ShadowLevel = pixel;
                }
            }
//        }



        /// <summary>
        /// Returns the color of the shadow texture at any pixel coord
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>Color object containing RGBA</returns>
        public Color getShadowColorAtPixel(int x, int y)
        {
            if (x < 0 || y < 0)
            {
                return Color.Black;
            }
            if (x >= 1600 || y >= 900)
            {
                return Color.Black;
            }

            int twoDimWidth = screenLightsWithout.Width;
            int oneDimIndex = x + y * twoDimWidth;

            return colorData[oneDimIndex];
        }

        public void Toast(String bigText, String littleText)
        {
            currentToastText = bigText;
            currentBabyToastText = littleText;
            toastOrigin = toastFont.MeasureString(currentToastText) / 2;
            babyToastOrigin = babyToastFont.MeasureString(currentBabyToastText) / 2;
            toasting = true;
            fadingIn = true;
            toastColour = new Color(255, 255, 255, 0);
        }
    }
}
