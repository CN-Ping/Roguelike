using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Model.GameObjects;
using Roguelike.Model.Infrastructure;

namespace Roguelike.View
{
    public class TextureSplat : GameObject
    {
        /// <summary>
        /// For drawing asthetic only things.
        /// </summary>
        /// <param name="x">x position of the center </param>
        /// <param name="y">y position of the center </param>
        /// <param name="textureName">string of the texture</param>
        public TextureSplat(Level level, int x, int y, string name) : base(level, x, y)
        {
            myTextureFileName = name;
            layerType = Model.LayerType.WorldTexture;
            LoadContent();
        }

        public override void SetTexture()
        {
            return;
        }

        public override void UnloadContent()
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            return;
        }

        public override void Draw(SpriteBatchWrapper spriteBatch)
        {
            spriteBatch.Draw(texture2D, worldCenter, null, Color.White, base.textureRotation, origin, 1.0f, SpriteEffects.None, 1);
        }

    }
}
