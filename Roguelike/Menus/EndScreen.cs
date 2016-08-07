using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Model;
using Microsoft.Xna.Framework.Input;

namespace Roguelike.Menus
{
    public class EndScreen
    {
        Model.Model gameModel;

        //Texture2D splashTexture;
        SpriteBatch spriteBatch;
        GraphicsDeviceManager graphics;
        SpriteFont Font;
        SpriteFont Font_14;
        bool success;
        bool enter = false;
        Vector2 position;
        string pressAnyKey;
        Vector2 pressAnyKeyPos;
        Vector2 pressAnyKeyOrigin;

        Vector2 textOrigin;

        string myWinLoseText;

        public EndScreen(Model.Model model, GraphicsDeviceManager inGraphics, bool wasSuccess, string killTextOrWinText)
        {
            gameModel = model;
            graphics = inGraphics;
            myWinLoseText = killTextOrWinText;
            LoadContent();
            success = wasSuccess;

            
        }

        public void LoadContent()
        {
            int screenWidth = graphics.PreferredBackBufferWidth;
            int screenHeight = graphics.PreferredBackBufferHeight;

            spriteBatch = new SpriteBatch(gameModel.Game.GraphicsDevice);
            Font = gameModel.Game.Content.Load<SpriteFont>("Arial_s24");
            Font_14 = gameModel.Game.Content.Load<SpriteFont>("Arial");
            position = new Vector2(screenWidth/2, 3*screenHeight/4);
            pressAnyKey = "Press any key to continue.";
            pressAnyKeyPos = new Vector2(screenWidth/2, 3*screenHeight/4 + 44);
            pressAnyKeyOrigin = Font_14.MeasureString(pressAnyKey)/2;

            textOrigin = Font.MeasureString(myWinLoseText) / 2;
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            bool changeState = false;

            #region key events
            /* Key down events */
            if (Keyboard.GetState().GetPressedKeys().Length > 0)
            {
                enter = true;
            }

            /* Key up events */
            if (enter && Keyboard.GetState().GetPressedKeys().Length == 0)
            {
                enter = false;
                changeState = true;
            }
            #endregion key events

            if(changeState)
            {
                gameModel.startMenu();
            }
        }

        public void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {

            spriteBatch.Begin();
            
            /* Black outline bit */
            spriteBatch.DrawString(Font, myWinLoseText, new Vector2(position.X - 2, position.Y - 2), Color.Black, 0, textOrigin, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(Font, myWinLoseText, new Vector2(position.X + 2, position.Y - 2), Color.Black, 0, textOrigin, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(Font, myWinLoseText, new Vector2(position.X - 2, position.Y + 2), Color.Black, 0, textOrigin, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(Font, myWinLoseText, new Vector2(position.X + 2, position.Y + 2), Color.Black, 0, textOrigin, 1.0f, SpriteEffects.None, 0.5f);

            /* White bit */
            spriteBatch.DrawString(Font, myWinLoseText, position, Color.White, 0, textOrigin, 1.0f, SpriteEffects.None, 0.5f);

            /* Press any key to continue. */
            spriteBatch.DrawString(Font_14, pressAnyKey, new Vector2(pressAnyKeyPos.X - 2, pressAnyKeyPos.Y - 2), Color.Black, 0, pressAnyKeyOrigin, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(Font_14, pressAnyKey, new Vector2(pressAnyKeyPos.X + 2, pressAnyKeyPos.Y - 2), Color.Black, 0, pressAnyKeyOrigin, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(Font_14, pressAnyKey, new Vector2(pressAnyKeyPos.X - 2, pressAnyKeyPos.Y + 2), Color.Black, 0, pressAnyKeyOrigin, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(Font_14, pressAnyKey, new Vector2(pressAnyKeyPos.X + 2, pressAnyKeyPos.Y + 2), Color.Black, 0, pressAnyKeyOrigin, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(Font_14, pressAnyKey, pressAnyKeyPos, Color.White, 0, pressAnyKeyOrigin, 1.0f, SpriteEffects.None, 0.5f);

            spriteBatch.End();
        }

        public void ExitEndScreen()
        {
        }
    }
}
