using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Model.Infrastructure;
using Shadows2D;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Roguelike.Model.GameObjects.Projectiles
{
    class TorchBullet : GameObject
    {
        LightSource torchLight;
        //const double LIFE_SPAN = 15;
        const double DIM_SPAN = 4;
        //const int LIGHT_RADIUS = 350;
        const double THROW_SPEED_RATIO = 0.7;
        const double FLY_TIME = 1.2;
        Stopwatch watch;
        bool alive;
        Level level;
        Model gameModel;
        int speed;
        int xVel;
        int yVel;
        int dirX;
        int dirY;
        bool rColor = false;

        Color lightColor;
        static Random random;

        double lightTimer = 0;
        int alphaBase = 128;
        int alphaRadius = 30;

        Texture2D starTexture;

        public TorchBullet(Level level, int startX, int startY, int velX, int velY, int dirX, int dirY, double shotSpeed)
            : base(level, startX, startY)
        {
            if (random == null)
            {
                random = new Random();
            }
            
            layerType = LayerType.Stuff;
            lightColor = new Color(255, 180, 120, 128);

            this.torchLight = new LightSource(currentLevel.gameModel.gameView.graphics, (int)currentLevel.mainChar.stats.torchRadius, LightAreaQuality.High, lightColor);
            //DateTime startTime = new DateTime (); // the start time;
            alive = true;
            this.level = level;
            this.gameModel = level.gameModel;
            watch = new Stopwatch();
            watch.Start();
            level.addGameObject(this);
            LoadContent();
            SetBoundingPointsOffset();

            speed = (int)shotSpeed;
            this.dirX = dirX;
            this.dirY = dirY;
            xVel = (int)(velX + dirX * speed * THROW_SPEED_RATIO); /*playerXVel and playerYVel are the speed the player was going when they shot */
            yVel = (int)(velY + dirY * speed * THROW_SPEED_RATIO);
            worldCenter.X = startX;
            worldCenter.Y = startY;
        }
        public override void UnloadContent()
        {
            throw new NotImplementedException();
        }

        public override void LoadContent()
        {
            base.LoadContent();
            SetTexture();
            starTexture = gameModel.Game.Content.Load<Texture2D>("Objects/Bullets/torchBulletStar");
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            double elapsed = (double)watch.Elapsed.Seconds + 0.001 * watch.Elapsed.Milliseconds;

            if (elapsed >= FLY_TIME)
            {
                xVel = 0;
                yVel = 0;
            }
            //else
            //{
            //xVel = (int)(xVel * (FLY_TIME - elapsed) / FLY_TIME);
            //yVel = (int)(yVel * (FLY_TIME - elapsed) / FLY_TIME);
            //Console.WriteLine(xVel+" "+yVel);
            //}

            worldCenter.X += xVel;
            worldCenter.Y += yVel;

            boundingBox.X = (int)worldCenter.X - boundingBoxOffset.X;
            boundingBox.Y = (int)worldCenter.Y - boundingBoxOffset.Y;


            UpdateATiles();

            List<GameObject> collisions = CollidesWith();
            for (int i = 0; i < collisions.Count; i++) 
            {
                //TODO: check for specific collisions rather than just obstacles
                if ((collisions[i].IsObstacle() && collisions[i].GetType() != typeof(MainCharacter)) && collisions[i].GetType() != typeof(TorchBullet))
                {
                    //if (collideWatch == null)
                    //{
                    //    collideWatch = new Stopwatch();
                    //    collideWatch.Start();
                    //}
                    //double collideElapsed = (double)collideWatch.Elapsed.Seconds + 0.001 * collideWatch.Elapsed.Milliseconds;
                    //if (collideElapsed >= 0.5)
                    //{
                    xVel = 0;
                    yVel = 0;
                    //}
                    //else
                    //{
                    //    xVel = (int) (-xVel * (0.5 - collideElapsed));
                    //    yVel = (int) (-yVel * (0.5 - collideElapsed));
                    //}
                    break;
                }
            }

            Vector2 screenLocation = new Vector2(worldCenter.X - ((int)gameModel.currentLevel.mainChar.worldCenter.X - gameModel.gameView.ScreenWidthOver2),
                worldCenter.Y - ((int)gameModel.currentLevel.mainChar.worldCenter.Y - gameModel.gameView.ScreenHeightOver2));

            torchLight.Position = new Vector2(screenLocation.X + 17, screenLocation.Y - 9);

            //double elapsed = (double)watch.Elapsed.Seconds + 0.001 * watch.Elapsed.Milliseconds;

            float torchDuration = currentLevel.mainChar.stats.torchDuration;
            int torchRadius = (int)currentLevel.mainChar.stats.torchRadius;

            if (elapsed > torchDuration)
            {
                //Console.WriteLine("Torch died");
                alive = false;
                currentLevel.castsLights.Remove(this);
                //level.removeGameObject(this);
            }

            if (torchDuration - elapsed <= DIM_SPAN)
            {
                //Console.WriteLine("Smaller Radius");
                int newRadius = torchRadius - (int)(torchRadius / DIM_SPAN * (DIM_SPAN - (torchDuration - elapsed)));
                //Console.WriteLine(newRadius);
                if (newRadius <= 0)
                {
                    newRadius = 1;
                }
                this.torchLight.SetRadius(newRadius);
            }

            // make torches flicker, but not glowsticks
            if (!rColor)
            {
                if (gameTime.TotalGameTime.TotalMilliseconds - 50 > lightTimer)
                {
                    torchLight.Color.A = (byte)(alphaBase + (random.Next(alphaRadius) - (alphaRadius / 2)));

                    lightTimer = gameTime.TotalGameTime.TotalMilliseconds;
                }
            }

            ////the substraction of two datetime is a TimeSpan , if you want that the
            //double seconds = ((TimeSpan)(DateTime.Now - startTime)).TotalSeconds;
        }

        public override void Draw(View.SpriteBatchWrapper spriteBatch)
        {
            drawLocation.X = worldCenter.X - textureWidthOver2;
            drawLocation.Y = worldCenter.Y - textureHeightOver2;
            spriteBatch.Draw(texture2D, drawLocation, Color.White);
            if (alive)
            {
                spriteBatch.Draw(starTexture, new Vector2(drawLocation.X + 18, drawLocation.Y - 20), Color.White);
            }
        }

        public override void SetTexture()
        {
            myTextureFileName = "Objects/Bullets/TorchBullet";
        }

        public override bool IsObstacle()
        {
            return true;
        }

        public override bool CastsLight()
        {
            if (alive)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override LightSource GetLightSource()
        {
            return torchLight;
        }

        override public void SetBoundingPointsOffset()
        {
            SetBoundingPointsOffset(textureWidth + 10, textureHeight + 5);
        }



    }
}
