using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.View;
//using Shadows2D;
using Roguelike.Model.Infrastructure;

namespace Roguelike.Model.GameObjects
{
    public class LaserBullet : ABullet
    {
        //LightSource bulletLight;

        public LaserBullet(Level currentLevel)
            : base(currentLevel)
        {
            type = BulletType.Laser;
            LoadContent();
            SetBoundingPointsOffset();

            /* Light is adjusted for shot size */
            //this.bulletLight = new LightSource(currentLevel.gameModel.gameView.graphics, (int)(75 * currentLevel.mainChar.stats.shotSize), LightAreaQuality.High, new Color(255, 160, 100, 128));

            /* adjustments for shotsize */
            textureWidthOver2 = (int)(textureWidth * currentLevel.mainChar.stats.shotSize) / 2;
            textureHeightOver2 = (int)(textureHeight * currentLevel.mainChar.stats.shotSize) / 2;
        }

        public LaserBullet(Level level, int startX, int startY) : base(level, startX, startY)
        {
            type = BulletType.Laser;
            LoadContent();
            SetBoundingPointsOffset();

            /* Light is adjusted for shot size */
            //this.bulletLight = new LightSource(currentLevel.gameModel.gameView.graphics, (int)(75*currentLevel.mainChar.stats.shotSize), LightAreaQuality.High, new Color(255, 160, 100, 128));
            
            /* adjustments for shotsize */
            textureWidthOver2 = (int)(textureWidth * currentLevel.mainChar.stats.shotSize) / 2;
            textureHeightOver2 = (int)(textureHeight * currentLevel.mainChar.stats.shotSize) / 2;
        }

        public override void SetTexture()
        {
            myTextureFileName = "Objects/Bullets/laserBullet";
        }

        public override void UnloadContent()
        {
            throw new NotImplementedException();
        }

        public override void Draw(SpriteBatchWrapper spriteBatch)
        {
                drawLocation.X = worldCenter.X - textureWidthOver2;
                drawLocation.Y = worldCenter.Y - textureHeightOver2;
                //spriteBatch.Draw(texture2D, drawLocation, Color.White);
                spriteBatch.Draw(texture2D, drawLocation, null, Color.White, 0f, new Vector2(0,0), currentLevel.mainChar.stats.shotSize, SpriteEffects.None, 1);
        }

        override public void SetBoundingPointsOffset()
        {
            SetBoundingPointsOffset((int)(textureWidth*currentLevel.mainChar.stats.shotSize), (int)(textureHeight*currentLevel.mainChar.stats.shotSize));
        }

        public override bool CastsLight()
        {
            return true;
        }

        //public override LightSource GetLightSource()
        //{
        //    return bulletLight;
        //}

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (CastsLight())
            {
                Vector2 location = new Vector2(worldCenter.X - ((int)currentLevel.mainChar.worldCenter.X - currentLevel.gameModel.gameView.ScreenWidthOver2), worldCenter.Y - ((int)currentLevel.mainChar.worldCenter.Y - currentLevel.gameModel.gameView.ScreenHeightOver2));
                //bulletLight.Position = location;
            }
        }

        public override void SetStats(int startX, int startY, int playerXVel, int playerYVel, int dirX, int dirY, double shotSpeed)
        {
            base.SetStats(startX, startY, playerXVel, playerYVel, dirX, dirY, shotSpeed);

            //bullshit to make the animation smoother
            //bulletLight.Position = new Microsoft.Xna.Framework.Vector2(-80, -80);
        }

        public override void Refresh(global::Roguelike.Model.Infrastructure.Level l, int startX, int startY)
        {
            base.Refresh(l, startX, startY);

            /* Light is adjusted for shot size */
            //this.bulletLight.Radius = (int)(75 * currentLevel.mainChar.stats.shotSize);
            //this.bulletLight.Color = new Color(255, 160, 100, 128);

            /* adjustments for shotsize */
            textureWidthOver2 = (int)(textureWidth * currentLevel.mainChar.stats.shotSize) / 2;
            textureHeightOver2 = (int)(textureHeight * currentLevel.mainChar.stats.shotSize) / 2;
        }

    }
}
