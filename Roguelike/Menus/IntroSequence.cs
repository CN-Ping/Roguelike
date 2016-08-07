using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Roguelike.Sound;
using Roguelike.View;

namespace Roguelike.Menus
{
    /// <summary>
    /// Splash screen and animation for DESERT BUS 3. 
    /// 
    /// Under heavy construction
    /// </summary>
    public class IntroSequence
    {
        Model.Model gameModel;

        SpriteBatch spriteBatch;
        GraphicsDeviceManager graphics;
        
        private long stateTimer; 
        private long privateTimer0;
        private long privateTimer1;
        private long privateTimer2;
        private long privateTimer3;
        private long privateTimer4;
        private long privateTimer5;
        private long privateTimer6;
        private long privateTimer7;
        private long privateTimer8;

        bool go = true;

        private bool changeState = false;

        int screenWidth;
        int screenHeight;

        Vector2 screenCenter;

        #region state assets
        Texture2D splashTexture;

        Rectangle screenRectangle;
        
        SpriteFont Font;
        SpriteFont otherFont;
        SpriteFont terminalFont;
        Color fontColor;
        Color overlayColor;

        
        #region state 0
        int alpha = 0;
        string state0Text = "Joe Warren Studios Presents";
        Vector2 state0TextOrigin;
        Vector2 textPositionCenter;
        #endregion state 0
        #region state 1
        string state1Text = "A Joe Warren Sanctioned Production";
        Vector2 state1TextOrigin;
        #endregion state 1
        #region state 2
        Texture2D bigStarfield;
        Texture2D moonTexture;
        Texture2D white;

        Rectangle starfieldSourceRectangle;
        Rectangle moonOutputRectangle;

        Rectangle alphaRectangle;

        int starfieldY;
        int moonY;
        #endregion state 2
        #region state 3
        Texture2D busFrontTexture;
        Rectangle busOutputRectangle;

        int busY = 0;
        #endregion state 3
        #region state 4
        int[] state4BumDelays = new int[] {200, 250, 200, 400, 350, 250, 450, 350, 350, 400, 500, 500};
        int bumCounter = 0;

        Vector2[] state4BumPositions;
        float[] state4BumRotations = new float[] { (float)((1.0 / 10.0) * MathHelper.Pi), (float)((-1.0 / 9.0) * MathHelper.Pi), (float)((1.0 / 8.0) * MathHelper.Pi), (float)((-1.0 / 7.0) * MathHelper.Pi), (float)((1.0 / 6.0) * MathHelper.Pi), (float)((-1.0 / 5.0) * MathHelper.Pi), (float)((1.0 / 4.0) * MathHelper.Pi), (float)((-3.0 / 8.0) * MathHelper.Pi), /*(float)((1.0 / 3.0) * MathHelper.Pi),*/ (float)((-2.0 / 5.0) * MathHelper.Pi), (float)((7.0 / 8.0) * MathHelper.Pi), (float)((-1.0 / 8.0) * MathHelper.Pi) };
        Vector2 state4BumOrigin;
        Vector2 state4BumScaleBase;
        float[] state4BumScaleMultipliers = new float[] {1.0f, 1.5f, 1.8f, 2.6f, 3.4f, 0.4f, /*1.0f,*/ 2.8f, 0.17f, 2.8f, 3.8f, 6.0f};
        Vector2[] state4BumScaleActuals;

        #endregion state 4
        #region state 5
        Texture2D bactaTexture;
        Texture2D busInteriorWallTexture;
        Texture2D nekkidTexture;
        Texture2D closedEyes;
        Texture2D eyeWhites;

        AnimatedSprite nekkidMan;

        Rectangle bactaOutputRectangle;
        Rectangle bactaSourceRectangle;

        Rectangle nekkidOutputRectangle;

        Rectangle busInteriorWallSource;

        int bactaY;
        int nekkidY;
        #endregion state 5
        #region state 6
        Texture2D leftPupil;
        Texture2D rightPupil;

        Rectangle leftPupilDrawBase;
        Rectangle rightPupilDrawBase;

        Rectangle leftPupilDrawMod;
        Rectangle rightPupilDrawMod;
        #endregion state 6
        #region state 7
        Vector2[] state7BumPositionsL = new Vector2[] { Vector2.Zero, new Vector2(-20, 0), new Vector2(0, 20), new Vector2(10, -10), new Vector2(-20, 10), new Vector2(-30, 0), new Vector2(30, 6), new Vector2(0, 0), new Vector2(-30, -10), new Vector2(-30, -5), new Vector2(-25, -10), new Vector2(0, 0) };
        Vector2[] state7BumPositionsR = new Vector2[] { Vector2.Zero, new Vector2(-20, 0), new Vector2(0, 20), new Vector2(-10, 10), new Vector2(-20, -20), new Vector2(30, 0), new Vector2(-20, 0), new Vector2(-10000, -10000), new Vector2(0, 0), new Vector2(5, 5), new Vector2(10, 10), new Vector2(0, 0) };
        #endregion state 7
        #region state 8
        Texture2D bactaEmpty;
        Texture2D bactaWater;

        Rectangle bactaWaterOutput;

        Texture2D missionScreen;
        Texture2D missionScreenGo;
        Vector2 missionScreenLocation;
        #endregion state 8
        #region state 9
        String fullTerminalText = "contract  objectives:\n  -infiltrate  station\n  -engage  hostiles\n  -salvage  database  intel \n \nSTATUS:  GO";
        String currentTerminalText = "";
        int terminalTextCounter = 0;
        Vector2 terminalTextPosition;

        int terminalTextDelay = 62;

        #endregion state 9
        #region state 10
        Texture2D doorSplash;

        Texture2D rightDoorTexture;
        Texture2D leftDoorTexture;
        Texture2D triangleTexture;

        Texture2D doorFloop;
        Texture2D doorLightRed;
        Texture2D doorLightGreen;
        Texture2D doorLight;

        Rectangle rightDoorRectangle;
        Rectangle leftDoorRectangle;

        Color triangleColour = new Color(255, 255, 255, 255);

        #endregion state 10
        #region state 11
        SpriteFont desertBusFont;

        String desertText = "D.E.S.E.R.T.";
        String busText = " B.U.S.";
        String threeText = " 3*";
        String desertBusText = "";

        String totalText = "D.E.S.E.R.T. B.U.S. 3";
        Vector2 totalTextOrigin;
        Vector2 totalTextPosition;

        String lolBullcrapText = "Duplication of Experimental Specimens for Exploration, Reconnaissance, \n                     Termination, Bounty, and Unauthorized Salvage 3";
        Vector2 lolBullcrapTextOrigin;
        Vector2 lolBullcrapTextPosition;
        Color lolBullcrapTextColor = new Color(255, 255, 255, 0);

        Color desertBusThreeActiveColor = new Color(255, 255, 255, 255);

        SpriteFont FontSmall;

        String lolMoreBullcrapText = "* The spiritual successor to D.E.S.E.R.T. B.U.S. 2**";
        Vector2 lolMoreBullcrapTextOrigin;
        Vector2 lolMoreBullcrapTextPosition;
        Color lolMoreBullcrapTextColor = new Color(255, 255, 255, 0);


        String lolEvenMoreBullcrapText = "** The spiritual successor to Penn and Teller's Desert Bus";
        Vector2 lolEvenMoreBullcrapTextOrigin;
        Vector2 lolEvenMoreBullcrapTextPosition;
        Color lolEvenMoreBullcrapTextColor = new Color(255, 255, 255, 0);

        
        #endregion state 11
        #endregion state assets

        SoundEffect sound2001;
        SoundEffectInstance sound2001Instance;

        
        

        int state = -1;

        public IntroSequence(Model.Model model, GraphicsDeviceManager inGraphics)
        {
            gameModel = model;
            graphics = inGraphics;

            fontColor = new Color(255, 255, 255, alpha);
            overlayColor = new Color(0, 0, 0, alpha);
            screenWidth = graphics.PreferredBackBufferWidth;
            screenHeight = graphics.PreferredBackBufferHeight;

            screenRectangle = new Rectangle(0, 0, screenWidth, screenHeight);
            starfieldSourceRectangle = new Rectangle(0, 0, screenWidth, screenHeight);

            white = new Texture2D(graphics.GraphicsDevice, 1, 1);
            white.SetData<Color>(new[] {Color.White});
            alphaRectangle = new Rectangle(0, 0, screenWidth, screenHeight);

            moonY = (screenHeight / 11) * 10;

            textPositionCenter = new Vector2(screenWidth / 2, screenHeight / 2);

            screenCenter = new Vector2(screenWidth / 2, screenHeight / 2);
        }

        public void LoadContent(Game g)
        {
            spriteBatch = new SpriteBatch(g.GraphicsDevice); 
            splashTexture = g.Content.Load<Texture2D>("Textures/Misc/DesertBus");
            bigStarfield = g.Content.Load<Texture2D>("Textures/biggerStarField");
            moonTexture = g.Content.Load<Texture2D>("Textures/thatsNoMoon");
            busFrontTexture = g.Content.Load<Texture2D>("Textures/Animation/DesertBusFront");
            bactaTexture = g.Content.Load<Texture2D>("Textures/Animation/bactaTank");
            busInteriorWallTexture = g.Content.Load<Texture2D>("Textures/Animation/busInteriorWall");


            Font = g.Content.Load<SpriteFont>("Arial_s24");
            otherFont = g.Content.Load<SpriteFont>("Surprise");
            //terminalFont = g.Content.Load<SpriteFont>("Terminal");
            FontSmall = g.Content.Load<SpriteFont>("Arial");

            starfieldY = bigStarfield.Height - screenHeight;

            sound2001 = g.Content.Load<SoundEffect>("Sound/2001");
            sound2001Instance = sound2001.CreateInstance();

            moonOutputRectangle = new Rectangle((screenWidth / 2) - (moonTexture.Width / 2), (screenHeight / 4) * 3, moonTexture.Width, moonTexture.Height);
            busOutputRectangle = new Rectangle((screenWidth / 2) - (busFrontTexture.Width / 3), (screenHeight), 2 * busFrontTexture.Width / 3, 2 * busFrontTexture.Height / 3) ;
            state0TextOrigin = Font.MeasureString(state0Text) / 2;
            state1TextOrigin = Font.MeasureString(state1Text) / 2;

            state4BumPositions = new Vector2[] { new Vector2(screenWidth / 2, screenHeight / 3), new Vector2(screenWidth / 3, (screenHeight / 3) + 100), new Vector2(6 * screenWidth / 15, screenHeight / 3 - 50), new Vector2(5 * screenWidth / 6 - 150, 4 * screenHeight / 5 - 150), /*new Vector2(screenWidth / 3, 2 * screenHeight / 3),*/ new Vector2(3 * screenWidth / 8, 3 * screenHeight / 7), new Vector2(3 * screenWidth / 5, 2 * screenHeight / 7), new Vector2(600, 600), new Vector2(screenWidth / 2 - 84, screenHeight / 2 + 60), new Vector2(700, screenHeight / 3 + 68), new Vector2(3 * screenWidth / 4 + 30, 3 * screenHeight / 4 - 55), new Vector2(screenWidth / 2 + 34, screenHeight / 2 - 19) };
            state4BumOrigin = new Vector2(busFrontTexture.Width / 2, busFrontTexture.Height / 2);
            state4BumScaleActuals = new Vector2[state4BumPositions.Length];

            bactaOutputRectangle = new Rectangle((screenWidth / 6), 0, 2 * screenWidth / 3, screenHeight);
            bactaSourceRectangle = new Rectangle(0, bactaTexture.Height - ((3 * bactaTexture.Width * screenHeight) / (2 * screenWidth)), bactaTexture.Width, (3 * bactaTexture.Width * screenHeight) / (2 * screenWidth));
            bactaY = bactaSourceRectangle.Y;

            //busInteriorWallOutput = new Rectangle(((screenWidth / 2) - (busInteriorWallTexture.Width / 2)), ((screenHeight / 2) - (busInteriorWallTexture.Height / 2)), busInteriorWallTexture.Width, busInteriorWallTexture.Height);
            //busInteriorWallOutput = new Rectangle(0, 0, busInteriorWallTexture.Width, busInteriorWallTexture.Height);
            busInteriorWallSource = new Rectangle(0, busInteriorWallTexture.Height - bactaSourceRectangle.Height, (3 * bactaSourceRectangle.Width / 2), bactaSourceRectangle.Height);
            //busInteriorWallSource = new Rectangle(0, 0, busInteriorWallTexture.Width, busInteriorWallTexture.Height);

            /* Makes the nekkid man animated sprite */
            nekkidTexture = g.Content.Load<Texture2D>("Textures/Animation/lessNekkidDood2");
            nekkidMan = new AnimatedSprite(nekkidTexture, 2, 4);
            eyeWhites = g.Content.Load<Texture2D>("Textures/Animation/eyeWhites");
            closedEyes = g.Content.Load<Texture2D>("Textures/Animation/eyesClosed");
            leftPupil = g.Content.Load<Texture2D>("Textures/Animation/leftPupil");
            rightPupil = g.Content.Load<Texture2D>("Textures/Animation/rightPupil");

            nekkidMan.framesPerSecond = 0;

            int w = 4 * bactaOutputRectangle.Width / 9;
            nekkidOutputRectangle = new Rectangle(screenWidth / 2 - (w / 2), -1 * ((w) * (nekkidMan.frameHeight) / (nekkidMan.frameWidth)) / 2, w, (w) * (nekkidMan.frameHeight) / (nekkidMan.frameWidth));
            nekkidY = nekkidOutputRectangle.Y;

            leftPupilDrawBase = new Rectangle(nekkidOutputRectangle.X, nekkidOutputRectangle.Y, nekkidOutputRectangle.Width, nekkidOutputRectangle.Height);
            rightPupilDrawBase = new Rectangle(nekkidOutputRectangle.X, nekkidOutputRectangle.Y, nekkidOutputRectangle.Width, nekkidOutputRectangle.Height);
            leftPupilDrawMod = new Rectangle(nekkidOutputRectangle.X, nekkidOutputRectangle.Y, nekkidOutputRectangle.Width, nekkidOutputRectangle.Height);
            rightPupilDrawMod = new Rectangle(nekkidOutputRectangle.X, nekkidOutputRectangle.Y, nekkidOutputRectangle.Width, nekkidOutputRectangle.Height);

            bactaEmpty = g.Content.Load<Texture2D>("Textures/Animation/bactaTankEmpty");
            bactaWater = g.Content.Load<Texture2D>("Textures/Animation/bactaTankWater");
            missionScreen = g.Content.Load<Texture2D>("Textures/Animation/missionOverlay");
            missionScreenGo = g.Content.Load<Texture2D>("Textures/Animation/missionOverlayGo");
            missionScreenLocation = new Vector2(screenWidth/2 - missionScreen.Width / 2, screenHeight /2 - missionScreen.Height / 2);

            terminalTextPosition = new Vector2(missionScreenLocation.X + 100, missionScreenLocation.Y + 120);
            
            bactaWaterOutput = new Rectangle(bactaOutputRectangle.X, bactaOutputRectangle.Y, bactaOutputRectangle.Width, bactaOutputRectangle.Height);

            doorSplash = g.Content.Load<Texture2D>("Textures/Animation/doorThingy");
            doorFloop = g.Content.Load<Texture2D>("Textures/Animation/doorFloop");
            doorLightRed = g.Content.Load<Texture2D>("Textures/Animation/doorLightRed");
            doorLightGreen = g.Content.Load<Texture2D>("Textures/Animation/doorLightGreen");
            doorLight = doorLightRed;

            leftDoorTexture = g.Content.Load<Texture2D>("Textures/Animation/leftDoor");
            rightDoorTexture = g.Content.Load<Texture2D>("Textures/Animation/rightDoor");
            triangleTexture = g.Content.Load<Texture2D>("Textures/Animation/triangle");
            rightDoorRectangle = new Rectangle(0, 0, screenWidth, screenHeight);
            leftDoorRectangle = new Rectangle(0, 0, screenWidth, screenHeight);

            desertBusFont = g.Content.Load<SpriteFont>("Arial_sBig");

            totalTextOrigin = desertBusFont.MeasureString(totalText) / 2;
            totalTextPosition = new Vector2(screenWidth / 2, screenHeight / 3);

            lolBullcrapTextOrigin = Font.MeasureString(lolBullcrapText) / 2;
            lolBullcrapTextPosition = new Vector2(screenWidth / 2,  2 * screenHeight / 3);

            lolMoreBullcrapTextOrigin = FontSmall.MeasureString(lolMoreBullcrapText) / 2;
            lolMoreBullcrapTextPosition = new Vector2(screenWidth / 2, 2 * screenHeight / 3 + 100);

            lolEvenMoreBullcrapTextOrigin = FontSmall.MeasureString(lolEvenMoreBullcrapText) / 2;
            lolEvenMoreBullcrapTextPosition = new Vector2(screenWidth / 2, 2 * screenHeight / 3 + 125);
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            long t = DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond;

            switch (state)
            {
                #region state 0
                case -1:
                    gameModel.ConsoleWriteLine("start song");
                    stateTimer = DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond;
                    state = 0;
                    sound2001Instance.Play();
                    break;

                case 0:
                    if (t - stateTimer > 6000 || changeState)
                    {
                        changeState = false;
                        stateTimer = t;
                        state = 1;
                        alpha = 0;
                    }

                    else if (t - stateTimer > 500 && t - stateTimer < 4500)
                    {
                        if (alpha < 253)
                        {
                            alpha += 2;
                        }
                    }

                    else
                    {
                        if (alpha > 3)
                        {
                            alpha -= 3;
                        }
                    }
                    break;
                #endregion state 0
                #region state 1
                case 1:
                    if (t - stateTimer > 5750 || changeState)
                    {
                        changeState = false;
                        stateTimer = t;
                        state = 2;
                        alpha = 255;
                    }

                    if (t - stateTimer > 500 && t - stateTimer < 4250)
                    {
                        if (alpha < 253)
                        {
                            alpha += 2;
                        }
                    }

                    else
                    {
                        if (alpha > 3)
                        {
                            alpha -= 3;
                        }
                    }
                    break;
                #endregion state 1
                #region state 2
                case 2:
                    starfieldSourceRectangle = new Rectangle(0, 0, screenWidth, screenHeight);

                    if (t - stateTimer > 8500 || changeState)
                    {
                        changeState = false;
                        busY = moonY + 30;
                        stateTimer = t;
                        state = 3;
                    }

                    if (t - privateTimer0 > 20)
                    {
                        if (alpha > 2) {
                            alpha -= 3;
                        }
                        privateTimer0 = t;
                    }

                    if (t - privateTimer1 > 90) {
                        starfieldY--;
                        privateTimer1 = t;
                    }

                    if (t - privateTimer2 > 80)
                    {
                        moonY--;
                        privateTimer2 = t;
                    }

                    break;
                #endregion state 2
                #region state 3
                case 3:
                    if (t - stateTimer > 4900 || changeState)
                    {
                        state4BumScaleBase = new Vector2(((float)busOutputRectangle.Width / (float)busFrontTexture.Width), ((float)busOutputRectangle.Height / (float)busFrontTexture.Height));
                        
                        for (int i = 0; i < state4BumPositions.Length; i++)
                        {
                            state4BumScaleActuals[i].X = state4BumScaleBase.X * state4BumScaleMultipliers[i];
                            state4BumScaleActuals[i].Y = state4BumScaleBase.Y * state4BumScaleMultipliers[i];
                        }

                        changeState = false;
                        stateTimer = t;
                        state = 4;
                    }

                    if (t - privateTimer1 > 90) {
                        starfieldY--;
                        privateTimer1 = t;
                    }

                    if (t - privateTimer2 > 80)
                    {
                        moonY--;
                        privateTimer2 = t;
                    }

                    if (t - privateTimer3 > 1)
                    {
                        busY--;
                        privateTimer3 = t;
                    }

                    if (t - privateTimer4 > 120)
                    {
                        privateTimer4 = t;
                        busOutputRectangle.Inflate(3, 1);
                    }
                        
                    break;
                #endregion state 3
                #region state 4
                case 4:

                    if (t - privateTimer1 > 90)
                    {
                        starfieldY--;
                        privateTimer1 = t;
                    }

                    if (t - privateTimer2 > 80)
                    {
                        moonY--;
                        privateTimer2 = t;
                    }

                    if (t - privateTimer3 > state4BumDelays[bumCounter])
                    {
                        Console.Write("Bum ");
                        bumCounter++;
                        privateTimer3 = t;
                    }

                    // move this down here to catch issues with bumcounter exceeding vector length
                    if (t - stateTimer > 8000 || changeState || bumCounter >= state4BumPositions.Length)
                    {
                        changeState = false;
                        stateTimer = t;
                        bumCounter = 0;
                        state = 5;
                        alpha = 255;
                        starfieldY = bigStarfield.Height - screenHeight;
                        Console.WriteLine();
                    }

                    break;
                #endregion state 4
                #region state 5
                case 5:
                    if (t - stateTimer > 8700 || changeState)
                    {
                        changeState = false;
                        stateTimer = t;
                        bumCounter = 0;
                        leftPupilDrawBase.Y = nekkidY;
                        rightPupilDrawBase.Y = nekkidY;
                        state = 6;
                    }

                    if (t - privateTimer0 > 20)
                    {
                        if (alpha > 2)
                        {
                            alpha -= 3;
                        }
                        privateTimer0 = t;
                    }

                    if (t - privateTimer1 > 90)
                    {
                        starfieldY--;
                        privateTimer1 = t;
                    }

                    if (t - privateTimer2 > 32)
                    {
                        bactaY -= 3;
                        nekkidY += 2;
                        privateTimer2 = t;

                        nekkidMan.Update(gameTime);
                    }


                    break;
                #endregion state 5
                #region state 6
                case 6:
                    if (t - stateTimer > 5200 || changeState)
                    {
                        changeState = false;
                        stateTimer = t;
                        bumCounter = 0;

                        leftPupilDrawBase.Y = nekkidOutputRectangle.Y;
                        rightPupilDrawBase.Y = nekkidOutputRectangle.Y;

                        leftPupilDrawMod.Y = nekkidOutputRectangle.Y;
                        rightPupilDrawMod.Y = nekkidOutputRectangle.Y;

                        state = 7;
                    }

                    if (t - privateTimer1 > 90)
                    {
                        starfieldY--;
                        privateTimer1 = t;
                    }
                    break;
                #endregion state 6
                #region state 7
                case 7:
                    if (t - privateTimer1 > 50)
                    {
                        starfieldY--;
                        privateTimer1 = t;
                    }

                    if (t - privateTimer3 > state4BumDelays[bumCounter])
                    {
                        leftPupilDrawMod.X = leftPupilDrawBase.X + (int)state7BumPositionsL[bumCounter].X; // plus whatever offset
                        leftPupilDrawMod.Y = leftPupilDrawBase.Y + (int)state7BumPositionsL[bumCounter].Y; // plus the offset

                        rightPupilDrawMod.X = rightPupilDrawBase.X + (int)state7BumPositionsR[bumCounter].X;
                        rightPupilDrawMod.Y = rightPupilDrawBase.Y + (int)state7BumPositionsR[bumCounter].Y;

                        bumCounter++;
                        Console.Write("Bum ");
                        privateTimer3 = t;
                    }

                    // move this down here to catch issues with bumcounter exceeding vector length
                    if (t - stateTimer > 8000 || changeState || bumCounter >= state4BumPositions.Length)
                    {
                        changeState = false;
                        stateTimer = t;
                        bumCounter = 0;
                        state = 8;
                        alpha = 255;
                        //starfieldY = bigStarfield.Height - screenHeight;

                        bactaWaterOutput.X = bactaOutputRectangle.X;
                        bactaWaterOutput.Y = bactaOutputRectangle.Y;
                        bactaWaterOutput.Width = bactaOutputRectangle.Width;
                        bactaWaterOutput.Height = bactaOutputRectangle.Height;
                        
                        Console.WriteLine();
                    }
                    break;
                #endregion state 7
                #region state 8
                case 8:
                    if (t - stateTimer > 8500 || changeState)
                    {
                        changeState = false;
                        stateTimer = t;
                        bumCounter = 0;

                        terminalTextCounter = 0;

                        state = 9;
                    }

                    else
                    {

                        if (t - privateTimer1 > 90)
                        {
                            starfieldY--;
                            privateTimer1 = t;
                        }

                        if ((terminalTextCounter < fullTerminalText.Length - 2) && (t - privateTimer2 > terminalTextDelay))
                        {
                            terminalTextCounter++;
                            currentTerminalText = fullTerminalText.Substring(0, terminalTextCounter);

                            if (currentTerminalText.EndsWith("\n"))
                            {
                                terminalTextDelay = 120;
                            }
                            else
                            {
                                terminalTextDelay = 65;
                            }

                            privateTimer2 = t;
                        }
                    }
                    break;
                #endregion state 8
                #region state 9
                case 9:
//                    if (t - stateTimer > 4700 || changeState)
                    if (t - stateTimer > 6700 || changeState)
                    {
                        changeState = false;
                        stateTimer = t;
                        bumCounter = 0;
                        privateTimer0 = t;
                        alpha = 255;

                        state = 10;
                    }

                    else
                    {

                        if (t - privateTimer1 > 90)
                        {
                            starfieldY--;
                            privateTimer1 = t;
                        }

                        if (t - privateTimer2 > 32)
                        {
                            bactaWaterOutput.Y += 7;
                            nekkidOutputRectangle.Y += 2;
                            leftPupilDrawBase.Y += 2;
                            rightPupilDrawBase.Y += 2;

                            privateTimer2 = t;

                            nekkidMan.Update(gameTime);
                        }

                        if ((t - privateTimer3 > 500))
                        {
                            switch (terminalTextCounter)
                            {
                                case 0:
                                    currentTerminalText = fullTerminalText;

                                    terminalTextCounter = 1;
                                    break;
                                case 1:
                                    currentTerminalText = fullTerminalText.Substring(0, fullTerminalText.Length - 2);

                                    terminalTextCounter = 0;
                                    break;
                            }


                            privateTimer3 = t;
                        }
                    }
                    break;
                #endregion state 9
                    //probably more states
                #region state 10
                case 10:
                    //if (t - stateTimer > 9000 || changeState)
                    if (t - stateTimer > 7000 || changeState)
                    {
                        changeState = false;
                        stateTimer = t;

                        privateTimer0 = t;
                        privateTimer1 = t;
                        privateTimer2 = t;
                        privateTimer3 = t;
                        privateTimer4 = t;
                        privateTimer5 = t;
                        privateTimer6 = t;
                        privateTimer7 = t;
                        privateTimer8 = t;

                        state = 11;
                    }

                    else
                    {
                        //if (go && t - privateTimer0 > 3100)
                        if (go && t - privateTimer0 > 1100)
                        {
                            doorLight = doorLightGreen;

                            if (t - privateTimer1 > 20)
                            {
                                leftDoorRectangle.X -= 1;
                                rightDoorRectangle.X += 1;
                                privateTimer1 = t;

                                if (leftDoorRectangle.X < -250)
                                {
                                    go = false;
                                }

                                if (triangleColour.A > 5)
                                {
                                    triangleColour.A -= 5;
                                }
                                else
                                {
                                    triangleColour.A = 0;
                                }
                            }
                        }
                    }
                    break;
                #endregion state 10
                #region state 11
                case 11:
                    if ((sound2001Instance.State == SoundState.Stopped) || changeState)
                    {
                        gameModel.startMenu();
                    }

                    else
                    {
                        if (t - privateTimer0 > 1300)
                        {
                            desertBusText = desertText;

                        }

                        if (t - privateTimer1 > 2700)
                        {
                            desertBusText += busText;
                        }

                        if (t - privateTimer2 > 3600)
                        {
                            desertBusText += threeText;
                        }

                        if (t - privateTimer3 > 5500)
                        {
                            if (t - privateTimer4 > 10)
                            {
                                if (lolBullcrapTextColor.A < 244)
                                {
                                    lolBullcrapTextColor.A += 10;
                                }

                                privateTimer4 = t;
                            }

                        }

                        if (t - privateTimer5 > 7500)
                        {
                            if (t - privateTimer6 > 10)
                            {
                                if (lolMoreBullcrapTextColor.A < 244)
                                {
                                    lolMoreBullcrapTextColor.A += 10;
                                }

                                privateTimer6 = t;
                            }

                        }

                        if (t - privateTimer7 > 9000)
                        {
                            if (t - privateTimer8 > 10)
                            {
                                if (lolEvenMoreBullcrapTextColor.A < 239)
                                {
                                    lolEvenMoreBullcrapTextColor.A += 15;
                                }

                                privateTimer8 = t;
                            }

                        }
                    }
                    break;
                #endregion state 11
            }


            #region key events
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                gameModel.startMenu();
            }
            #endregion key events
        }

        public void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {

            switch (state)
            {
                #region state 0
                case 0: 
                    
                    spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied);
                    spriteBatch.Draw(splashTexture, screenRectangle, Color.Black);

                    fontColor.A = (byte)alpha;
                    spriteBatch.DrawString(Font, state0Text, textPositionCenter, fontColor, 0, state0TextOrigin, 1.0f, SpriteEffects.None, 0.5f);

                    spriteBatch.End();
                    break;
                #endregion state 0
                #region state 1
                case 1:
                    spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied);

                    spriteBatch.Draw(splashTexture, screenRectangle, Color.Black);

                    fontColor.A = (byte)alpha;
                    spriteBatch.DrawString(Font, state1Text, textPositionCenter, fontColor, 0, state1TextOrigin, 1.0f, SpriteEffects.None, 0.5f);

                    spriteBatch.End();
                    break;
                #endregion state 1
                #region state 2
                case 2:
                    spriteBatch.Begin();

                    starfieldSourceRectangle.Y = starfieldY;
                    spriteBatch.Draw(bigStarfield, screenRectangle, starfieldSourceRectangle, Color.White);

                    moonOutputRectangle.Y = moonY;
                    spriteBatch.Draw(moonTexture, moonOutputRectangle, Color.White);

                    spriteBatch.End();

                    spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied);

                    overlayColor.A = (byte)alpha;
                    spriteBatch.Draw(white, alphaRectangle, overlayColor);

                    spriteBatch.End();
                    break;
                #endregion state 2
                #region state 3
                case 3:
                    spriteBatch.Begin();

                    starfieldSourceRectangle.Y = starfieldY;
                    spriteBatch.Draw(bigStarfield, screenRectangle, starfieldSourceRectangle, Color.White);

                    busOutputRectangle.Y = busY;
                    spriteBatch.Draw(busFrontTexture, busOutputRectangle, Color.White);

                    moonOutputRectangle.Y = moonY;
                    spriteBatch.Draw(moonTexture, moonOutputRectangle, Color.White);

                    spriteBatch.End();
                    break;
                #endregion state 3
                #region state 4
                case 4:
                    spriteBatch.Begin();

                    starfieldSourceRectangle.Y = starfieldY;
                    spriteBatch.Draw(bigStarfield, screenRectangle, starfieldSourceRectangle, Color.White);
                     
                    moonOutputRectangle.Y = moonY;
                    spriteBatch.Draw(moonTexture, moonOutputRectangle, Color.White);

                    spriteBatch.Draw(busFrontTexture,
                        state4BumPositions[bumCounter],
                        null,
                        Color.White,
                        state4BumRotations[bumCounter],
                        state4BumOrigin,
                        state4BumScaleActuals[bumCounter],
                        SpriteEffects.None,
                        0f);

                    spriteBatch.End();
                    break;
                #endregion state 4
                #region state 5
                case 5:
                    spriteBatch.Begin();

                    starfieldSourceRectangle.Y = starfieldY;
                    spriteBatch.Draw(bigStarfield, screenRectangle, starfieldSourceRectangle, Color.White);

                    busInteriorWallSource.Y = bactaY;
                    spriteBatch.Draw(busInteriorWallTexture, screenRectangle, busInteriorWallSource, Color.White);

                    nekkidOutputRectangle.Y = nekkidY;
                    nekkidMan.Draw(spriteBatch, nekkidOutputRectangle);
                    spriteBatch.Draw(closedEyes, nekkidOutputRectangle, Color.White);

                    bactaSourceRectangle.Y = bactaY;
                    spriteBatch.Draw(bactaTexture, bactaOutputRectangle, bactaSourceRectangle, Color.White);

                    spriteBatch.End();

                    spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied);

                    overlayColor.A = (byte)alpha;
                    spriteBatch.Draw(white, alphaRectangle, overlayColor);

                    spriteBatch.End();
                    break;
                #endregion state 5
                #region state 6
                case 6:
                    spriteBatch.Begin();

                    starfieldSourceRectangle.Y = starfieldY;
                    spriteBatch.Draw(bigStarfield, screenRectangle, starfieldSourceRectangle, Color.White);

                    spriteBatch.Draw(busInteriorWallTexture, screenRectangle, busInteriorWallSource, Color.White);

                    nekkidMan.Draw(spriteBatch, nekkidOutputRectangle);
                    spriteBatch.Draw(eyeWhites, nekkidOutputRectangle, Color.White);
                    spriteBatch.Draw(leftPupil, leftPupilDrawBase, Color.White);
                    spriteBatch.Draw(rightPupil, rightPupilDrawBase, Color.White);

                    spriteBatch.Draw(bactaTexture, bactaOutputRectangle, bactaSourceRectangle, Color.White);

                    spriteBatch.End();

                    spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied);

                    spriteBatch.Draw(white, alphaRectangle, overlayColor);

                    spriteBatch.End();
                    break;
                #endregion state 6
                #region state 7
                case 7:
                    spriteBatch.Begin();

                    starfieldSourceRectangle.Y = starfieldY;
                    spriteBatch.Draw(bigStarfield, screenRectangle, starfieldSourceRectangle, Color.White);

                    spriteBatch.Draw(busInteriorWallTexture, screenRectangle, busInteriorWallSource, Color.White);

                    nekkidMan.Draw(spriteBatch, nekkidOutputRectangle);
                    spriteBatch.Draw(eyeWhites, nekkidOutputRectangle, Color.White);

                    spriteBatch.Draw(leftPupil, leftPupilDrawMod, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
                    spriteBatch.Draw(rightPupil, rightPupilDrawMod, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);

                    spriteBatch.Draw(bactaTexture, bactaOutputRectangle, bactaSourceRectangle, Color.White);

                    spriteBatch.End();
                    break;
                #endregion state 7
                #region state 8
                case 8:
                    spriteBatch.Begin();

                    starfieldSourceRectangle.Y = starfieldY;
                    spriteBatch.Draw(bigStarfield, screenRectangle, starfieldSourceRectangle, Color.White);

                    spriteBatch.Draw(busInteriorWallTexture, screenRectangle, busInteriorWallSource, Color.White);

                    nekkidMan.Draw(spriteBatch, nekkidOutputRectangle);
                    spriteBatch.Draw(eyeWhites, nekkidOutputRectangle, Color.White);

                    spriteBatch.Draw(leftPupil, leftPupilDrawBase, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
                    spriteBatch.Draw(rightPupil, rightPupilDrawBase, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);

                    spriteBatch.Draw(bactaWater, bactaWaterOutput, bactaSourceRectangle, Color.White);
                    spriteBatch.Draw(bactaEmpty, bactaOutputRectangle, bactaSourceRectangle, Color.White);

                    spriteBatch.Draw(missionScreen, missionScreenLocation, Color.White);
                    //spriteBatch.DrawString(terminalFont, currentTerminalText, terminalTextPosition, Color.Yellow);

                    //TODO text overlay?

                    spriteBatch.End();
                    break;
                #endregion state 8
                #region state 9
                case 9:
                    spriteBatch.Begin();

                    starfieldSourceRectangle.Y = starfieldY;
                    spriteBatch.Draw(bigStarfield, screenRectangle, starfieldSourceRectangle, Color.White);

                    spriteBatch.Draw(busInteriorWallTexture, screenRectangle, busInteriorWallSource, Color.White);

                    nekkidMan.Draw(spriteBatch, nekkidOutputRectangle);
                    spriteBatch.Draw(eyeWhites, nekkidOutputRectangle, Color.White);

                    spriteBatch.Draw(leftPupil, leftPupilDrawBase, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
                    spriteBatch.Draw(rightPupil, rightPupilDrawBase, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);

                    spriteBatch.Draw(bactaWater, bactaWaterOutput, bactaSourceRectangle, Color.White);
                    spriteBatch.Draw(bactaEmpty, bactaOutputRectangle, bactaSourceRectangle, Color.White);

                    spriteBatch.Draw(missionScreenGo, missionScreenLocation, Color.White);

                    //spriteBatch.DrawString(terminalFont, currentTerminalText, terminalTextPosition, Color.Yellow);
                     
                    //TODO text overlay?

                    spriteBatch.End();
                    break;
                #endregion state 9
                #region state 10
                case 10:
                    spriteBatch.Begin();

                    spriteBatch.Draw(doorFloop, screenRectangle, Color.White);

                    spriteBatch.Draw(rightDoorTexture, rightDoorRectangle, Color.White);
                    spriteBatch.Draw(leftDoorTexture, leftDoorRectangle, Color.White);

                    spriteBatch.Draw(doorSplash, screenRectangle, Color.White);
                    spriteBatch.Draw(doorLight, screenRectangle, Color.White);

                    spriteBatch.End(); 

                    spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied);
                    spriteBatch.Draw(triangleTexture, screenRectangle, triangleColour);

                    spriteBatch.End();
                    break;
                #endregion state 10
                #region state 11
                case 11:
                    spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied);
                    spriteBatch.Draw(splashTexture, screenRectangle, Color.Black);

                    spriteBatch.DrawString(desertBusFont, desertBusText, totalTextPosition, desertBusThreeActiveColor, 0, totalTextOrigin, 1.0f, SpriteEffects.None, 0.5f);

                    spriteBatch.DrawString(Font, lolBullcrapText, lolBullcrapTextPosition, lolBullcrapTextColor, 0, lolBullcrapTextOrigin, 1.0f, SpriteEffects.None, 0.5f);

                    spriteBatch.DrawString(FontSmall, lolMoreBullcrapText, lolMoreBullcrapTextPosition, lolMoreBullcrapTextColor, 0, lolMoreBullcrapTextOrigin, 1.0f, SpriteEffects.None, 0.5f);

                    spriteBatch.DrawString(FontSmall, lolEvenMoreBullcrapText, lolEvenMoreBullcrapTextPosition, lolEvenMoreBullcrapTextColor, 0, lolEvenMoreBullcrapTextOrigin, 1.0f, SpriteEffects.None, 0.5f);
                    

                    spriteBatch.End();
                    break;
                #endregion state 11
            }
        }

        public void ExitSplashScreen()
        {
            sound2001Instance.Stop();
        }
    }
}
