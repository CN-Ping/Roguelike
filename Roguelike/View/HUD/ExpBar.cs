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
    public class ExpBar : GameObject
    {
        Texture2D barOutline;
        Texture2D barFill;

        Vector2 expNumPos;
        SpriteFont Font;

        Rectangle outlineDestinationRectangle;
        Rectangle outlineSource;
        Rectangle fillDestinationRectangle;
        Rectangle sourceRectangle;

        StatsInstance stats;

        double width;
        string levelString;
        Vector2 fontExpOrigin;

        public ExpBar(Level level, int startX, int startY)
            : base(level, startX, startY)
        {

        }

        override public void LoadContent()
        {
            barOutline = currentLevel.gameModel.Game.Content.Load<Texture2D>("HUD/expOutline");
            barFill = currentLevel.gameModel.Game.Content.Load<Texture2D>("HUD/expFill");
            layerType = LayerType.HUD;
            Font = currentLevel.gameModel.Game.Content.Load<SpriteFont>("Arial");
            expNumPos = new Vector2(barFill.Width / 2, barFill.Height / 2 + 60);

            /* If the amount of exp required to advance a level is ever NOT constant, the
            * width and fillDestinationRectangle and sourceRectangle will have to
            * be recalculated at each levelup. */
            stats = currentLevel.mainChar.stats;
            width = barFill.Width / currentLevel.gameModel.skillTree.expRequired;
            int height = barFill.Height;

            outlineDestinationRectangle = new Rectangle(0, 60, barOutline.Width, barOutline.Height);
            outlineSource = new Rectangle(0, 0, barOutline.Width, barOutline.Height);

        }

        override public void Draw(SpriteBatchWrapper spriteBatch)
        {

            /* Draw the outline */
            spriteBatch.s.Draw(barOutline, outlineDestinationRectangle, outlineSource, Color.White);

            /* Draw the fill */
            fillDestinationRectangle = new Rectangle(0, 60, (int)(width * stats.tempExp), barFill.Height);
            sourceRectangle = new Rectangle(0, 0, (int)(width * stats.tempExp), barFill.Height);
            spriteBatch.s.Draw(barFill, fillDestinationRectangle, sourceRectangle, Color.White);

            /* Draw the text */
            levelString = "Lvl " + currentLevel.mainChar.stats.playerLevel;
            fontExpOrigin = Font.MeasureString(levelString) / 2;
            spriteBatch.s.DrawString(Font, levelString, expNumPos, Color.White, 0, fontExpOrigin, 1.0f, SpriteEffects.None, 0.5f);

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
