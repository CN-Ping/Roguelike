using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.View;
using Roguelike.Model.GameObjects.Monsters.AI;
using Roguelike.Model.Infrastructure;
//using Shadows2D;

namespace Roguelike.Model.GameObjects.Monsters
{
    public class GhostMonster : AMonster
    {
        private AnimatedSprite left;
        private AnimatedSprite right;
        private Texture2D leftTexture;
        private Texture2D rightTexture;

        private bool lastDirectionWasLeft = true;

        private int damage = 8;

        //private LightSource glowingThingy;

        private Model gameModel;

        static Random random;

        double lightTimer = 0;
        int alphaBase = 64;
        int alphaRadius = 50;


        public GhostMonster(Level level, int startX, int startY)
            : base(level, startX, startY)
        {
            health = 30;
            speed = 2;
            maxForceClamp = 2 * speed;
            layerType = LayerType.Stuff;
            //this.glowingThingy = new LightSource(currentLevel.gameModel.gameView.graphics, 50, LightAreaQuality.High, Color.Pink*0.5f);
            gameModel = level.gameModel;
            if (random == null)
            {
                random = new Random();
            }
            LoadContent();
            SetBoundingPointsOffset();

            //myAIs.Add(new ShittyLightAI(this, 1));
            myAIs.Add(new AStarFollowAI(this, 1500, 400, 1200));
            //isStaticObject = false;

            killText = "You were possessed by a ghost!";
        }

        public GhostMonster() : base()
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();

            Roguelike Game = currentLevel.gameModel.Game;

            leftTexture = Game.Content.Load<Texture2D>("Sprites/GhostL");
            rightTexture = Game.Content.Load<Texture2D>("Sprites/GhostR");

            left = new AnimatedSprite(leftTexture, 1, 14);
            right = new AnimatedSprite(rightTexture, 1, 14);


            //CLEANUP
            textureWidth = leftTexture.Width / 14;
            textureWidthOver2 = leftTexture.Width / 28;

            textureHeight = leftTexture.Height;
            textureHeightOver2 = leftTexture.Height / 2;

            drawLocation = new Vector2(worldCenter.X - (textureWidthOver2), worldCenter.Y - (textureHeightOver2));

            dropChance = .3;
        }

        public override void SetTexture()
        {
            myTextureFileName = "Sprites/GhostL";
        }

        public override void UnloadContent()
        {
            throw new NotImplementedException();
        }

        public override void SecondaryUpdate(GameTime gameTime)
        {
            Vector2 screenLocation = new Vector2(worldCenter.X - ((int)gameModel.currentLevel.mainChar.worldCenter.X - gameModel.gameView.ScreenWidthOver2),
               worldCenter.Y - ((int)gameModel.currentLevel.mainChar.worldCenter.Y - gameModel.gameView.ScreenHeightOver2));

            //glowingThingy.Position = new Vector2(screenLocation.X, screenLocation.Y);

            if (gameTime.TotalGameTime.TotalMilliseconds - 50 > lightTimer)
            {
                //glowingThingy.Color.A = (byte)(alphaBase + (random.Next(alphaRadius) - (alphaRadius / 2)));

                lightTimer = gameTime.TotalGameTime.TotalMilliseconds;
            }

            left.Update(gameTime);
            right.Update(gameTime);

            UpdateATiles();
        }

        public override void DrawMonster(SpriteBatchWrapper spriteBatch, bool hit)
        {

            #region Draw Ghost
            drawLocation.X = worldCenter.X - textureWidthOver2;
            drawLocation.Y = worldCenter.Y - textureHeightOver2;
            int alpha = shadowLevel_.R;


            if (velocity.X > 0) /* Sprite walks right*/
            {
                lastDirectionWasLeft = false;
            }
            else if (velocity.X < 0) /* Sprite walks left*/
            {
                lastDirectionWasLeft = true;
            }


            if (lastDirectionWasLeft)
            {
                left.Draw(spriteBatch, drawLocation, 255 - alpha, hit);
            }
            else 
            {
                right.Draw(spriteBatch, drawLocation, 255 - alpha, hit);
            }
            #endregion

        }

        override public void SetBoundingPointsOffset()
        {
            SetBoundingPointsOffset(textureWidth, textureHeight);
        }


        override public bool IsObstacle()
        {
            //return false;
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

        /*
        public override void DrawCaster(Shadows2D.ShadowCasterMap shadowMap)
        {
            drawLocation.X = worldCenter.X - textureWidthOver2;
            drawLocation.Y = worldCenter.Y - textureHeightOver2;

            if (lastDirectionWasLeft)
            {
                left.DrawCaster(shadowMap, drawLocation);
            }
            else
            {
                right.DrawCaster(shadowMap, drawLocation);
            }
        }
        */
         
        public override void ApplyForceToOtherObjects(GameTime gameTime)
        {
            base.ApplyForceToOtherObjects(gameTime);
        }

        public override void ComputeVelocity(GameTime gameTime)
        {
            base.ComputeVelocity(gameTime);
        }

        public override bool CastsShadow()
        {
            return false;
        }

        public override bool CastsLight()
        {
            return true;
        }

        //public override LightSource GetLightSource()
        //{
        //    return this.glowingThingy;
        //}
    }
}
