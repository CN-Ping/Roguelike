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
    public class TorchCount : GameObject
    {

        Vector2 location;
        SpriteFont Font;

        public TorchCount(Level level, int startX, int startY)
            : base(level, startX, startY)
        {
            location = new Vector2(1400, 50);
        }

        override public void LoadContent()
        {
            layerType = LayerType.HUD;
            Font = currentLevel.gameModel.Game.Content.Load<SpriteFont>("Arial");
        }

        override public void Draw(SpriteBatchWrapper spriteBatch)
        {
            StatsInstance stats = currentLevel.mainChar.stats;
            string levelString;
            /* Draw the text */
            levelString = "Flares Left: " + currentLevel.mainChar.stats.torchCount;
            Vector2 fontOrigin = Font.MeasureString(levelString);
            spriteBatch.s.DrawString(Font, levelString, location, Color.White, 0, fontOrigin, 1.0f, SpriteEffects.None, 0.5f);

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
