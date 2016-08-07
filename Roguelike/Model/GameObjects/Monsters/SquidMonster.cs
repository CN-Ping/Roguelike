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

    public class SquidMonster : AMonster
    {
        private int damage = 10;
        
        bool moving = false;
        SoundEffect sploosh;
        SoundEffectInstance splooshInstance;

        int textureWidthOver8;

        public SquidMonster(Level level, int startX, int startY) : base(level, startX, startY)
        {
            health = 8;
            speed = 3;
            maxForceClamp = 2 * speed;
            layerType = LayerType.Stuff;
            SetBoundingPointsOffset();
            LoadContent();
            //myAIs.Add(new ShittyLightAI(this, 1));
            //myAIs.Add(new ShittyFollowAI(this));
            myAIs.Add(new AStarFollowAI(this));

            killText = "You were squished by a squid.";
        }

        public SquidMonster()
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();
            textureWidthOver8 = textureWidth / 8;

            sprite = new AnimatedSprite(texture2D, 1, 4);

            sploosh = currentLevel.gameModel.Game.Content.Load<SoundEffect>("Sound/splat1");
            splooshInstance = sploosh.CreateInstance();
            splooshInstance.Volume = 0.3f;

            dropChance = .3;
            //drawLocation = new Vector2(center.X - textureWidth, center.Y - textureHeight);
        }

        public override void SetTexture()
        {
            myTextureFileName = "Sprites/purpleSquid";
        }

        public override void UnloadContent()
        {
            throw new NotImplementedException();
        }

        public override void SecondaryUpdate(GameTime gameTime)
        {

            if ((forceVector.X != 0 || forceVector.Y != 0) && moving == false)
            {
                startedMoving();
            }
            else if ((forceVector.X == 0 && forceVector.Y == 0) && moving == true)
            {
                stoppedMoving();
            }

            sprite.Update(gameTime);

            UpdateATiles();

            /*
            List<GameObject> collisions = CollidesWith();
            for (int i = 0; i < collisions.Count; i++)
            {
                //TODO: check for specific collisions rather than just obstacles
                if (collisions[i].IsObstacle())
                {
                    
                }
            }
             * */
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
            // custom squid dimensions
            int height = 40;
            int width = 40;

            SetBoundingPointsOffset(width, height);
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
                splooshInstance.Stop();
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
            //splooshInstance.IsLooped = true;
            splooshInstance.Play();
        }

        private void stoppedMoving()
        {
            moving = false;
            //splooshInstance.IsLooped = false;
            splooshInstance.Stop();
        }

       

    }
}
