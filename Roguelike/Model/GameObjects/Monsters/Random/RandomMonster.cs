using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Model.Infrastructure;
using Microsoft.Xna.Framework;

namespace Roguelike.Model.GameObjects.Monsters.Randomized
{
    public class RandomMonster : AMonster
    {
        private Texture2D headTexture;
        private Texture2D bodyTexture;
        private Texture2D leftArmTexture;
        private Texture2D rightArmTexture;
        private Texture2D leftLegTexture;
        private Texture2D rightLegTexture;

        public RandomMonster(Level levelIn, int x, int y, Texture2D head, Texture2D body, Texture2D larm, Texture2D rarm, Texture2D lleg, Texture2D rleg) : base(levelIn, x, y) 
        {
            layerType = LayerType.Stuff;

            headTexture = head;
            bodyTexture = body;
            leftArmTexture = larm;
            rightArmTexture = rarm;
            leftLegTexture = lleg;
            rightLegTexture = rleg;

            LoadContent();
        }

        public override void SetTexture()
        {
        }

        public override void LoadContent()
        {
            textureHeight = headTexture.Height;
            textureHeightOver2 = textureHeight / 2;

            textureWidth = headTexture.Width;
            textureWidthOver2 = textureWidth / 2;

            drawLocation = new Vector2(worldCenter.X - textureWidthOver2, worldCenter.Y - textureHeightOver2);
            origin = new Vector2(textureWidthOver2, textureHeightOver2);
        }

        public override void UnloadContent()
        {
            throw new NotImplementedException();
        }

        public override void DealDamage(GameTime gameTime)
        {
        }

        public override void DrawCaster(Shadows2D.ShadowCasterMap shadowMap)
        {
        }

        public override void SecondaryUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        {
            
        }


        public override void DrawMonster(View.SpriteBatchWrapper spriteBatch, bool hit)
        {
            //int textureHeight = texture.Height / 2;
            //int textureWidth = texture.Width / (2*4);
            drawLocation.X = worldCenter.X - textureHeightOver2;
            drawLocation.Y = worldCenter.Y - textureWidthOver2;
            int alpha = shadowLevel_.R;
            //sprite.Draw(spriteBatch, drawLocation, alpha);

            spriteBatch.Draw(bodyTexture, worldCenter, null, Color.White * ((float)alpha / 255), 0f, origin, 1.0f, SpriteEffects.None, 1);
            spriteBatch.Draw(headTexture, worldCenter, null, Color.White * ((float)alpha / 255), 0f, origin, 1.0f, SpriteEffects.None, 1);
            spriteBatch.Draw(leftLegTexture, worldCenter, null, Color.White * ((float)alpha / 255), 0f, origin, 1.0f, SpriteEffects.None, 1);
            spriteBatch.Draw(rightLegTexture, worldCenter, null, Color.White * ((float)alpha / 255), 0f, origin, 1.0f, SpriteEffects.None, 1);
            spriteBatch.Draw(leftArmTexture, worldCenter, null, Color.White * ((float)alpha / 255), 0f, origin, 1.0f, SpriteEffects.None, 1);
            spriteBatch.Draw(rightArmTexture, worldCenter, null, Color.White * ((float)alpha / 255), 0f, origin, 1.0f, SpriteEffects.None, 1);
        }


    }
}
