using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.View;
using Roguelike.Model.GameObjects.Monsters.AI;
using Microsoft.Xna.Framework.Audio;
using Roguelike.Model.Infrastructure;
using Roguelike.Model.GameObjects.Pickups;

namespace Roguelike.Model.GameObjects.Monsters
{

    public class SpiderMonster : AMonster
    {
        private float damage = 2f;
        
        bool moving = false;

        int textureWidthOver8;

        public SpiderMonster(Level level, int startX, int startY) : base(level, startX, startY)
        {
            health = 2;
            speed = 4;
            layerType = LayerType.Stuff;
            SetBoundingPointsOffset();
            LoadContent();
            dropChance = 0.05;
            inertia_0_to_1 = 0.25f;

            myAIs.Add(new FlockingAI(this));

            maxForceClamp = 2 * speed;

            killText = "You were overrun by spiders.";
        }

        public SpiderMonster()
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();
            textureWidthOver8 = textureWidth / 14;

            sprite = new AnimatedSprite(texture2D, 1, 7);

            dropChance = .3;
            //drawLocation = new Vector2(center.X - textureWidth, center.Y - textureHeight);
        }

        public override void SetTexture()
        {
            myTextureFileName = "Sprites/spider";
        }

        public override void UnloadContent()
        {
            throw new NotImplementedException();
        }

        public override void SecondaryUpdate(GameTime gameTime)
        {
            sprite.Update(gameTime);

            UpdateATiles();
        }

        public override void DrawMonster(SpriteBatchWrapper spriteBatch, bool hit)
        {
            //int textureHeight = texture.Height / 2;
            //int textureWidth = texture.Width / (2*4);
            drawLocation.X = worldCenter.X - textureHeightOver2;
            drawLocation.Y = worldCenter.Y - textureWidthOver8;
            int alpha = shadowLevel_.R;
            sprite.Draw(spriteBatch, drawLocation, alpha, hit);
        }

        override public void SetBoundingPointsOffset()
        {
            // custom for spiders because reasons
            SetBoundingPointsOffset(20, 20);
        }


        override public bool IsObstacle()
        {
            return true;
        }

        override public void ShotByBullet(BulletType bulletType, GameTime gameTime)
        {
            getDealtDamage(currentLevel.mainChar, (int)currentLevel.mainChar.stats.damage, gameTime);
            if (health <= 0)
            {
                OnMonsterDeath();
            }
        }

        override public void DealDamage(GameTime gameTime)
        {
            currentLevel.mainChar.getDealtDamage(this, damage, gameTime);
            return;
        }

        private void startedMoving()
        {
            moving = true;
        }

        private void stoppedMoving()
        {
            moving = false;
        }

       

    }
}
