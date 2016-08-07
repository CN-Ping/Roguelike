using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Roguelike.Model;

namespace Roguelike.Menus
{
    public class MenuScreen
    {
        Model.Model gameModel;

        //Texture2D splashTexture;
        SpriteBatch spriteBatch;
        GraphicsDeviceManager graphics;
        SpriteFont Font;

        int state = 0;
        int numOptions = 3;

        bool down = false;
        bool up = false;
        bool enter = false;

        Vector2 position0;
        Vector2 position1;
        Vector2 position2;

        Texture2D DesertBus;
        Rectangle destinationRect;

        public MenuScreen(Model.Model model, GraphicsDeviceManager inGraphics)
        {
            gameModel = model;
            graphics = inGraphics;
            LoadContent();
        }

        public void LoadContent()
        {
            spriteBatch = new SpriteBatch(gameModel.Game.GraphicsDevice);
            Font = gameModel.Game.Content.Load<SpriteFont>("Arial_s24");

            int screenWidth = graphics.PreferredBackBufferWidth;
            int screenHeight = graphics.PreferredBackBufferHeight;
            position0 = new Vector2(screenWidth / 2, 2 * screenHeight / 3);
            position1 = new Vector2(screenWidth / 2, (2 * screenHeight / 3) + 50);
            position2 = new Vector2(screenWidth / 2, (2 * screenHeight / 3) + 100);
            DesertBus = gameModel.Game.Content.Load<Texture2D>("Textures/Misc/DesertBus");
            destinationRect = new Rectangle(0,0,gameModel.Game.GraphicsDevice.Viewport.Width, gameModel.Game.GraphicsDevice.Viewport.Height);
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            bool changeState = false;

            #region key events
            /* Key down events */
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                enter = true;
            }

            if (!down && Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                down = true;
            }
            else if (!up && Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                up = true;
            }

            /* Key up events */
            if (enter && Keyboard.GetState().IsKeyUp(Keys.Enter))
            {
                enter = false;
                changeState = true;
            }

            if (down && Keyboard.GetState().IsKeyUp(Keys.Down))
            {
                down = false;
                state += 1;
                if (state >= numOptions)
                {
                    state = 0;
                }
            }
            else if (up && Keyboard.GetState().IsKeyUp(Keys.Up))
            {
                up = false;
                state -= 1;
                if (state < 0)
                {
                    state = numOptions - 1;
                }
            }
            #endregion key events

            if (changeState == true)
            {
                switch (state)
                {
                    case 0:
                        gameModel.startGame();
                        break;
                    case 1:
                        gameModel.startSkillScreen();
                        break;
                    case 2:
                        gameModel.onExit();
                        gameModel.Game.Exit();
                        break;
                    default:
                        break;
                }
            }

        }

        public void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {

            spriteBatch.Begin();

            /* Draw background */
            spriteBatch.Draw(DesertBus, destinationRect, Color.White);

            //Rectangle screenRectangle = new Rectangle(0, 0, screenWidth, screenHeight);
            //spriteBatch.Draw(splashTexture, screenRectangle, Color.White);
            string prompt0 = "Start Game";
            string prompt1 = "Skill Tree";
            string prompt2 = "Exit Game";
            Vector2 fontOrigin0 = Font.MeasureString(prompt0) / 2;
            Vector2 fontOrigin1 = Font.MeasureString(prompt1) / 2;
            Vector2 fontOrigin2 = Font.MeasureString(prompt2) / 2;

            switch(state)
            {
                case 0:
                    prompt0 = ">StartGame";
                    fontOrigin0 = Font.MeasureString(prompt0) / 2;
                    break;
                case 1:
                    prompt1 = ">Skill Tree";
                    fontOrigin1 = Font.MeasureString(prompt1) / 2;
                    break;
                case 2:
                    prompt2 = ">Exit Game";
                    fontOrigin2 = Font.MeasureString(prompt2) / 2;
                    break;
                default:
                    break;
            }

            /* Black bit */
            DrawBlackOutline(spriteBatch, Font, prompt0, position0, fontOrigin0);
            DrawBlackOutline(spriteBatch, Font, prompt1, position1, fontOrigin1);
            DrawBlackOutline(spriteBatch, Font, prompt2, position2, fontOrigin2);

            /* White bit */
            spriteBatch.DrawString(Font, prompt0, position0, Color.White, 0, fontOrigin0, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(Font, prompt1, position1, Color.White, 0, fontOrigin1, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(Font, prompt2, position2, Color.White, 0, fontOrigin2, 1.0f, SpriteEffects.None, 0.5f);

            spriteBatch.End();
        }

        public void ExitMenuScreen()
        {
        }

        private void DrawBlackOutline(SpriteBatch spritebatch, SpriteFont Font, string prompt, Vector2 position, Vector2 fontOrigin)
        {
            /* Black outline bit */
            spriteBatch.DrawString(Font, prompt, new Vector2(position.X - 2, position.Y - 2), Color.Black, 0, fontOrigin, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(Font, prompt, new Vector2(position.X + 2, position.Y - 2), Color.Black, 0, fontOrigin, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(Font, prompt, new Vector2(position.X - 2, position.Y + 2), Color.Black, 0, fontOrigin, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(Font, prompt, new Vector2(position.X + 2, position.Y + 2), Color.Black, 0, fontOrigin, 1.0f, SpriteEffects.None, 0.5f);
            
        }
    }
}
