using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Roguelike.Model.GameObjects;
using Roguelike.View;
using Roguelike.Model.Infrastructure;

namespace Roguelike.Util
{
    public class DebugHUD : GameObject
    {
        private Vector2 position = new Vector2(20, 10);

        public SpriteFont Font { get; set; }

        public bool active { get; set; } 

        public DebugHUD(Level level)
        {
            active = false;
            currentLevel = level;
            Font = currentLevel.gameModel.Game.Content.Load<SpriteFont>("Arial");
        }

        public override void Draw(SpriteBatchWrapper spriteBatch)
        {
            // Draw the Score in the top-left of screen
            if (active)
            {
                spriteBatch.s.DrawString(
                    Font,                          // SpriteFont
                    "Level: " + currentLevel.LevelNumber + "\n" + currentLevel.playerStatsInstance.ToString() + "Pos: (" + currentLevel.mainChar.worldCenter.X + ", " + currentLevel.mainChar.worldCenter.Y + ")\n",  // Text
                    position,                      // Position
                    Color.White);                  // Tint
            }
            
        }

        public override void LoadContent()
        {
            
        }

        public override void SetTexture()
        {
            
        }

        public override void UnloadContent()
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public void Reinitialize(Level newLevel)
        {
            currentLevel = newLevel;
        }
    }
}
