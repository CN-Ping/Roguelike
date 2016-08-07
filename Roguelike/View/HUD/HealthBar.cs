using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Model;
using Roguelike.Model.GameObjects;
using Roguelike.Model.Infrastructure;

namespace Roguelike.View
{
    public class HealthBar : GameObject
    {
        Texture2D barOutline;
        Texture2D barFill;

        Vector2 healthNumPos;
        SpriteFont Font_24;

        Rectangle outlineDestinationRectangle;
        Rectangle outlineSource;
        Rectangle fillDestinationRectangle;
        Rectangle sourceRectangle;

        StatsInstance stats;

        string healthString;
        Vector2 fontHealthOrigin;

        public HealthBar(Level level, int startX, int startY) : base(level, startX, startY)
        {

        }

        override public void LoadContent()
        {
            barOutline = currentLevel.gameModel.Game.Content.Load<Texture2D>("HUD/barOutline");
            barFill = currentLevel.gameModel.Game.Content.Load<Texture2D>("HUD/barFill");
            layerType = LayerType.HUD;
            Font_24 = currentLevel.gameModel.Game.Content.Load<SpriteFont>("Arial_s24");
            healthNumPos = new Vector2(barFill.Width / 2, barFill.Height / 2);

            stats = currentLevel.mainChar.stats;
            double width = barFill.Width / stats.maxHealth;
            int height = barFill.Height;

            outlineDestinationRectangle = new Rectangle(0, 0, barOutline.Width, barOutline.Height);
            outlineSource = new Rectangle(0, 0, barOutline.Width, barOutline.Height);
        }

        override public void Draw(SpriteBatchWrapper spriteBatch)
        {
            double width = barFill.Width / stats.maxHealth;

            /* Draw the outline */
            spriteBatch.s.Draw(barOutline, outlineDestinationRectangle, outlineSource, Color.White);

            /* Draw the fill */
            fillDestinationRectangle = new Rectangle(0, 0, (int)(width * stats.health), barFill.Height);
            sourceRectangle = new Rectangle(0, 0, (int)(width * stats.health), barFill.Height);
            spriteBatch.s.Draw(barFill, fillDestinationRectangle, sourceRectangle, Color.White);

            /* Draw the text */
            healthString = (currentLevel.mainChar.stats.health) + " / " + (currentLevel.mainChar.stats.maxHealth);
            fontHealthOrigin = Font_24.MeasureString(healthString) / 2;
            spriteBatch.s.DrawString(Font_24, healthString, healthNumPos, Color.White, 0, fontHealthOrigin, 1.0f, SpriteEffects.None, 0.5f);

        }

        override public void UnloadContent()
        {
            throw new NotImplementedException();
        }

        public override void SetTexture()
        {
            return;
        }

        override public void Update(GameTime gameTime)
        {
            return;
        }


        internal void SetNewLevel(Level level)
        {
            currentLevel = level;
        }
    }
}
