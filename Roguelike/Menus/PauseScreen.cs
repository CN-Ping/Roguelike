using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Model;
using Microsoft.Xna.Framework.Input;
using Roguelike.Model.Infrastructure;
using Roguelike.View;

namespace Roguelike.Menus
{
    public class PauseScreen
    {
        Model.Model gameModel;

        //Texture2D splashTexture;
        GraphicsDevice graphics;
        SpriteFont Font;

        Texture2D screenOverlay;
        Color overlayColour = new Color(0, 0, 0, 128);

        public Level CurrentLevel {get; set;}
        
        public PauseScreen(Model.Model model)
        {
            gameModel = model;
            graphics = model.Game.GraphicsDevice;
            LoadContent();
        }

        public void LoadContent()
        {
            screenOverlay = new Texture2D(graphics, graphics.Viewport.Width, graphics.Viewport.Height);

            Color[] data = new Color[graphics.Viewport.Width * graphics.Viewport.Height];
            for (int i = 0; i < data.Length; ++i) data[i] = overlayColour;
            screenOverlay.SetData(data);

            Font = gameModel.Game.content.Load<SpriteFont>("Arial_sBig");
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {

        }

        public void Draw(SpriteBatchWrapper spriteBatch)
        {
            spriteBatch.Begin();

            Vector2 fontOrigin = Font.MeasureString("PAUSED LOL")/2;
            Vector2 position = new Vector2(800, 450);

            spriteBatch.s.Draw(screenOverlay, Vector2.Zero, Color.Black);

            /* White bit */
            //spriteBatch.s.DrawString(Font, "PAUSED LOL", position, Color.White, 0, fontOrigin, 1.0f, SpriteEffects.None, 0.5f);
            gameModel.gameView.minimap.DrawBig(spriteBatch);

            spriteBatch.End();
        }

        public void ExitEndScreen()
        {
        }
    }
}
