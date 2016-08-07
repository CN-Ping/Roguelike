using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.View;
using Roguelike.Model.GameObjects.Monsters.AI;
using Roguelike.Model.Infrastructure;

namespace Roguelike.Model.GameObjects.Monsters
{
    public class ZombieMonster : AMonster
    {
        private AnimatedSprite up;
        private AnimatedSprite down;
        private AnimatedSprite left;
        private AnimatedSprite right;
        private Texture2D upTexture;
        private Texture2D downTexture;
        private Texture2D leftTexture;
        private Texture2D rightTexture;

        private int damage = 15;

        public ZombieMonster(Level level, int startX, int startY)
            : base(level, startX, startY)
        {
            health = 20;
            speed = 3;
            maxForceClamp = 2 * speed;
            layerType = LayerType.Stuff;
            SetBoundingPointsOffset();
            LoadContent();

            //myAIs.Add(new ShittyLightAI(this, 1));
            myAIs.Add(new ShittyFollowAI(this));

            killText = "You were mauled by a zombie.";
        }

        public override void LoadContent()
        {
            base.LoadContent();

            Roguelike Game = currentLevel.gameModel.Game;

            upTexture = Game.Content.Load<Texture2D>("Sprites/Zombie/ZombieWUFU");
            downTexture = Game.Content.Load<Texture2D>("Sprites/Zombie/ZombieWDFD");
            leftTexture = Game.Content.Load<Texture2D>("Sprites/Zombie/ZombieWLFL");
            rightTexture = Game.Content.Load<Texture2D>("Sprites/Zombie/ZombieWRFR");

            up = new AnimatedSprite(upTexture, 1, 8);
            down = new AnimatedSprite(downTexture, 1, 8);
            left = new AnimatedSprite(leftTexture, 1, 5);
            right = new AnimatedSprite(rightTexture, 1, 5);


            //CLEANUP
            drawLocation = new Vector2(worldCenter.X - 50, worldCenter.Y - 75 / 2);

            dropChance = 1;
        }

        public override void SetTexture()
        {
            myTextureFileName = "Sprites/Zombie/Zombie";
        }

        public override void UnloadContent()
        {
            throw new NotImplementedException();
        }

        public override void SecondaryUpdate(GameTime gameTime)
        {

            /* Check if the monster moves 
            int tempX = (int)(worldCenter.X + velocity.X);
            int tempY = (int)(worldCenter.Y + velocity.Y);

            bool validMovementX = ValidMovement(tempX, (int)worldCenter.Y);
            bool validMovementY = ValidMovement((int)worldCenter.X, tempY);

            if (validMovementX)
            {
                worldCenter.X += velocity.X;
            }
            if (validMovementY)
            {
                worldCenter.Y += velocity.Y;
            }*/

            /* Update Sprites */
            up.Update(gameTime);
            down.Update(gameTime);
            left.Update(gameTime);
            right.Update(gameTime);

            UpdateATiles();

            /* what is this
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

            #region Draw Zombie
            drawLocation.X = worldCenter.X - 50;
            drawLocation.Y = worldCenter.Y - 75 / 2;
            int alpha = shadowLevel_.R;
            if (velocity.X > 0) /* Sprite walks right*/
            {
                right.Draw(spriteBatch, drawLocation,alpha, hit);
            }
            else if (velocity.X < 0) /* Sprite walks left*/
            {
                left.Draw(spriteBatch, drawLocation,alpha, hit);
            }
            else if (velocity.Y < 0) /*Sprite walks up*/
            {
                up.Draw(spriteBatch, drawLocation,alpha, hit);
            }
            else if (velocity.Y > 0) /*Sprite walks down*/
            {
                down.Draw(spriteBatch, drawLocation,alpha, hit);
            }
            else /* Draw standing character */
            {
                Color alphaColor;
                if (hit)
                {
                    alphaColor = Color.Red * ((float)alpha / 255);
                }
                else
                {
                    alphaColor = Color.White * ((float)alpha / 255);
                }
                spriteBatch.Draw(texture2D, drawLocation, alphaColor);
            }
            #endregion

        }

        override public void SetBoundingPointsOffset()
        {
            // i dont know why these are defined here
            int height = 75;
            int width = 45;

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
                OnMonsterDeath();
            }
        }


        override public void DealDamage(GameTime gameTime)
        {
            currentLevel.mainChar.getDealtDamage(this, damage, gameTime);
            return;
        }

        public override void DrawCaster(Shadows2D.ShadowCasterMap shadowMap)
        {
            drawLocation.X = worldCenter.X - 50;
            drawLocation.Y = worldCenter.Y - 75 / 2;

            if (velocity.X > 0) /* Sprite walks right*/
            {
                right.DrawCaster(shadowMap, drawLocation);
            }
            else if (velocity.X < 0) /* Sprite walks left*/
            {
                left.DrawCaster(shadowMap, drawLocation);
            }
            else if (velocity.Y < 0) /*Sprite walks up*/
            {
                up.DrawCaster(shadowMap, drawLocation);
            }
            else if (velocity.Y > 0) /*Sprite walks down*/
            {
                down.DrawCaster(shadowMap, drawLocation);
            }
            else /* Draw standing character */
            {
                shadowMap.AddShadowCaster(texture2D, drawLocation, null, Color.White, base.textureRotation, origin, 1.0f, SpriteEffects.None, 1);
            }
        }

        public override void ApplyForceToOtherObjects(GameTime gameTime)
        {
            base.ApplyForceToOtherObjects(gameTime);
        }

        public override void ComputeVelocity(GameTime gameTime)
        {
            base.ComputeVelocity(gameTime);
        }
    }
}
