using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Roguelike.View;
using Microsoft.Xna.Framework.Audio;
using Penumbra;
using Roguelike.Model.GameObjects.Loot;
//using Shadows2D;
using Roguelike.Model.Infrastructure;
using Roguelike.Model.GameObjects.Projectiles;
using Roguelike.Util;

namespace Roguelike.Model.GameObjects
{
    public class MainCharacter : GameObject
    {
        /* Stats */
        public StatsInstance stats;

        /* Sprites */
        private Texture2D mainChar;
        private AnimatedSprite mainCharFacings;

        private AnimatedSprite WUFUsprite;
        private AnimatedSprite WUFDsprite;
        private AnimatedSprite WUFLsprite;
        private AnimatedSprite WUFRsprite;
        private AnimatedSprite WDFUsprite;
        private AnimatedSprite WDFDsprite;
        private AnimatedSprite WDFLsprite;
        private AnimatedSprite WDFRsprite;
        private AnimatedSprite WLFUsprite;
        private AnimatedSprite WLFDsprite;
        private AnimatedSprite WLFLsprite;
        private AnimatedSprite WLFRsprite;
        private AnimatedSprite WRFUsprite;
        private AnimatedSprite WRFDsprite;
        private AnimatedSprite WRFLsprite;
        private AnimatedSprite WRFRsprite;

        public List<ALoot> equippedLoot = new List<ALoot>();
        List<ALoot> updatingLoot = new List<ALoot>();

        double teleportAnimationTimer = 0;
        int teleportAnimationIndex = 3;
        int teleportAnimationDuration = 150;

        /* Which buttons are pressed */
        private bool up = false;
        private bool down = false;
        private bool left = false;
        private bool right = false;
        private bool torchPress = false;

        /* To know if the player is holding down a key to shoot */
        public bool shooting = false;

        /*True if allowed to shoot again*/
        bool shoot;
        double lastShot; // in millisecords

        public bool torchFireMode = false;
        bool rainbowTorch = false;

        public int startingHeight = 90;
        public int startingWidth = 50;

        private long lastTimeHurtMillis = 0;

        SoundEffect footsteps;
        SoundEffectInstance footstepsInstance;

        bool moving = false;

        // Lighting
        public Light playerLight { get; } = new PointLight
        {
            Scale = new Vector2(80000),
            Color = Color.White,
            ShadowType = ShadowType.Occluded
        };

        public AGun myGun;

        public bool rave = false;

        Random rng;

        Vector2 leftForce = new Vector2();
        Vector2 rightForce = new Vector2();
        Vector2 upForce = new Vector2();
        Vector2 downForce = new Vector2();

        protected bool drawHit;
        protected int msToBeRed;
        protected double lastHit;

        public MainCharacter(Level level, int startX, int startY, StatsInstance statsIn)
            : base(level, startX, startY)
        {
            /* These are hardcoded and not based off the size of the sprite because of weird stuff needed for animating the sprites*/
            shoot = true;
            lastShot = 0;
            layerType = LayerType.Player;
            SetBoundingPointsOffset();
            inertia_0_to_1 = 2;
            isStaticObject = false;

            stats = statsIn;
        }

        override public void LoadContent()
        {
            Roguelike Game = currentLevel.gameModel.Game;
            mainChar = Game.Content.Load<Texture2D>("Sprites/mainChar/mainChar");

            mainCharFacings = new AnimatedSprite (Game.Content.Load<Texture2D>("Sprites/mainChar/mainCharFacings"), 1, 4);

            Texture2D WUFUtexture = Game.Content.Load<Texture2D>("Sprites/mainChar/mainCharWUFU");
            Texture2D WDFDtexture = Game.Content.Load<Texture2D>("Sprites/mainChar/mainCharWDFD");
            Texture2D WRFRtexture = Game.Content.Load<Texture2D>("Sprites/mainChar/mainCharWRFR");
            Texture2D WLFLtexture = Game.Content.Load<Texture2D>("Sprites/mainChar/mainCharWLFL");
            Texture2D WUFLtexture = Game.Content.Load<Texture2D>("Sprites/mainChar/mainCharWUFL");
            Texture2D WUFRtexture = Game.Content.Load<Texture2D>("Sprites/mainChar/mainCharWUFR");
            Texture2D WLFUtexture = Game.Content.Load<Texture2D>("Sprites/mainChar/mainCharWLFU");
            Texture2D WLFDtexture = Game.Content.Load<Texture2D>("Sprites/mainChar/mainCharWLFD");

            WUFUsprite = new AnimatedSprite(WUFUtexture, 1, 8);
            WUFDsprite = new AnimatedSprite(WDFDtexture, 1, 8, false);
            WUFLsprite = new AnimatedSprite(WUFLtexture, 1, 11);
            WUFRsprite = new AnimatedSprite(WUFRtexture, 1, 11);
            WDFUsprite = new AnimatedSprite(WUFUtexture, 1, 8, false);
            WDFDsprite = new AnimatedSprite(WDFDtexture, 1, 8);
            WDFLsprite = new AnimatedSprite(WUFLtexture, 1, 11, false);
            WDFRsprite = new AnimatedSprite(WUFRtexture, 1, 11, false);
            WLFUsprite = new AnimatedSprite(WLFUtexture, 1, 8);
            WLFDsprite = new AnimatedSprite(WLFDtexture, 1, 8);
            WLFLsprite = new AnimatedSprite(WLFLtexture, 1, 5);
            WLFRsprite = new AnimatedSprite(WRFRtexture, 1, 5, false);
            WRFUsprite = new AnimatedSprite(WLFUtexture, 1, 8, false);
            WRFDsprite = new AnimatedSprite(WLFDtexture, 1, 8, false);
            WRFLsprite = new AnimatedSprite(WLFLtexture, 1, 5, false);
            WRFRsprite = new AnimatedSprite(WRFRtexture, 1, 5);

            WUFLsprite.framesPerSecond = 30;
            WUFRsprite.framesPerSecond = 30;
            WDFLsprite.framesPerSecond = 30;
            WDFRsprite.framesPerSecond = 30;

            footsteps = currentLevel.gameModel.Game.Content.Load<SoundEffect>("Sound/footsteps1");
            footstepsInstance = footsteps.CreateInstance();
            footstepsInstance.Volume = 0.5f;

            //this.playerLight = new LightSource(gameModel.gameView.graphics, 500, LightAreaQuality.High, Color.White);
            //(int)stats.lightRange
            //this.playerLight = new LightSource(currentLevel.gameModel.gameView.graphics, (int)stats.LightRange, LightAreaQuality.VeryHigh, new Color(255, 255, 255, 128));

            myGun = new StdPistolGun(currentLevel, (int)worldCenter.X, (int)worldCenter.Y);

            rng = new Random();

            drawHit = false;
            msToBeRed = 100;
            lastHit = double.MinValue;
        }

        public override void SetTexture()
        {
            // this is not going to happen in the main character. too special cased. 
        }

        override public void UnloadContent()
        {
            //throw new NotImplementedException();
        }

        override public void Update(GameTime gameTime)
        {
            if (teleportAnimationDuration < 150)
            {
                teleportAnimationDuration = 150;
            }

            /* Update sprites */
            WUFUsprite.Update(gameTime);
            WUFDsprite.Update(gameTime);
            WUFLsprite.Update(gameTime);
            WUFRsprite.Update(gameTime);
            WDFUsprite.Update(gameTime);
            WDFDsprite.Update(gameTime);
            WDFLsprite.Update(gameTime);
            WDFRsprite.Update(gameTime);
            WLFUsprite.Update(gameTime);
            WLFDsprite.Update(gameTime);
            WLFLsprite.Update(gameTime);
            WLFRsprite.Update(gameTime);
            WRFUsprite.Update(gameTime);
            WRFDsprite.Update(gameTime);
            WRFLsprite.Update(gameTime);
            WRFRsprite.Update(gameTime);

            /* Shooting events */
            #region Shooting Key Events

            if (gameTime.TotalGameTime.TotalMilliseconds - lastShot > stats.rateOfFire)
            {
                shoot = true;
            }

            int startX = (int)worldCenter.X;
            int startY = (int)worldCenter.Y;

            bool tempShooting = false;

            if (Keyboard.GetState().IsKeyDown(Keys.Left)) /* Shoot Left */
            {
                stats.lastShotDirection = 2;
                tempShooting = true;

                if (shoot == true)
                {
                    shoot = false;

                    if (torchFireMode == true)
                    {
                        throwTorch(startX, startY, stats.velocity.X, stats.velocity.Y, -1, 0, stats.shotSpeed);
                        torchFireMode = false;
                    }
                    else
                    {
                            myGun.shoot(startX, startY, stats.velocity.X, stats.velocity.Y, -1, 0, stats.shotSpeed);
                        
                    }
                    lastShot = gameTime.TotalGameTime.TotalMilliseconds;
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right)) /* Shoot Right */
            {
                stats.lastShotDirection = 0;
                tempShooting = true;

                if (shoot == true)
                {
                    shoot = false;
                    if (torchFireMode == true)
                    {
                        throwTorch(startX, startY, stats.velocity.X, stats.velocity.Y, 1, 0, stats.shotSpeed);
                        torchFireMode = false;
                    }
                    else
                    {
                            myGun.shoot(startX, startY, stats.velocity.X, stats.velocity.Y, 1, 0, stats.shotSpeed);
                    }
                    lastShot = gameTime.TotalGameTime.TotalMilliseconds;
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up)) /* Shoot Up */
            {
                stats.lastShotDirection = 3;
                tempShooting = true;

                if (shoot == true)
                {
                    shoot = false;
                    if (torchFireMode == true)
                    {
                        throwTorch(startX, startY, stats.velocity.X, stats.velocity.Y, 0, -1, stats.shotSpeed);
                        torchFireMode = false;
                    }
                    else
                    {
                            myGun.shoot(startX, startY, stats.velocity.X, stats.velocity.Y, 0, -1, stats.shotSpeed);
                    }
                    lastShot = gameTime.TotalGameTime.TotalMilliseconds;
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down)) /* Shoot Down */
            {
                stats.lastShotDirection = 1;
                tempShooting = true;

                if (shoot == true)
                {
                    shoot = false;
                    if (torchFireMode == true)
                    {
                        throwTorch(startX, startY, stats.velocity.X, stats.velocity.Y, 0, 1, stats.shotSpeed);
                        torchFireMode = false;
                    }
                    else
                    {
                            myGun.shoot(startX, startY, stats.velocity.X, stats.velocity.Y, 0, 1, stats.shotSpeed);
                    }
                    lastShot = gameTime.TotalGameTime.TotalMilliseconds;
                }
            }

            /* Check to see if the player is currently shooting. Need this so shooting can be set to false. */
            if (tempShooting)
            {
                shooting = true;
            }
            else
            {
                shooting = false;
            }

            #endregion

            foreach (ALoot loot in updatingLoot)
            {
                loot.Update(gameTime);
            }

            if (gameTime.TotalGameTime.TotalMilliseconds - lastHit > msToBeRed)
            {
                drawHit = false;
            }
        }

        public void LevelEndUpdate(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.TotalMilliseconds - teleportAnimationTimer > teleportAnimationDuration)
            {
                teleportAnimationTimer = gameTime.TotalGameTime.TotalMilliseconds;

                teleportAnimationIndex = (teleportAnimationIndex + 1) % 4;

                if (teleportAnimationDuration > 20)
                    teleportAnimationDuration -= 8;
            }
        }

        public override void ComputeVelocity(GameTime gameTime)
        {
            #region Movement Input

            if (currentLevel.gameModel.dvorak)
            {
                /* Key Down Events */
                if (Keyboard.GetState().IsKeyDown(Keys.OemComma)) /* Up */
                {
                    if (up == false)
                    {
                        up = true;
                        upForce.Y = -1 * stats.speed;
                    }
                }
                if (Keyboard.GetState().IsKeyDown(Keys.O)) /* Down */
                {
                    if (down == false)
                    {
                        down = true;
                        downForce.Y = stats.speed;
                    }
                }
                if (Keyboard.GetState().IsKeyDown(Keys.A)) /* Left */
                {
                    if (left == false)
                    {
                        left = true;
                        leftForce.X = -1 * stats.speed;
                    }
                }
                if (Keyboard.GetState().IsKeyDown(Keys.E)) /* Right */
                {
                    if (right == false)
                    {
                        right = true;
                        rightForce.X = stats.speed;
                    }
                }
                if (Keyboard.GetState().IsKeyDown(Keys.U)) /* Toggle Torch */
                {
                    if (stats.torchCount > 0)
                    {
                        torchPress = true;
                    }
                }

                /* Key Up Events */
                if (Keyboard.GetState().IsKeyUp(Keys.OemComma)) /* Up */
                {
                    if (up == true)
                    {
                        up = false;
                        upForce.Y = 0;
                    }
                }
                if (Keyboard.GetState().IsKeyUp(Keys.O)) /* Down */
                {
                    if (down == true)
                    {
                        down = false;
                        downForce.Y = 0;
                    }
                }
                if (Keyboard.GetState().IsKeyUp(Keys.A)) /* Left */
                {
                    if (left == true)
                    {
                        left = false;
                        leftForce.X = 0;
                    }
                }
                if (Keyboard.GetState().IsKeyUp(Keys.E)) /* Right */
                {
                    if (right == true)
                    {
                        right = false;
                        rightForce.X = 0;
                    }
                }
                if (Keyboard.GetState().IsKeyUp(Keys.U)) /* U */
                {
                    if (stats.torchCount > 0 && torchPress)
                    {
                        torchFireMode = !torchFireMode;
                    }
                    torchPress = false;
                }
            }

            else
            {
                /* Key Down Events */
                if (Keyboard.GetState().IsKeyDown(Keys.W)) /* Up */
                {
                    if (up == false)
                    {
                        up = true;
                        upForce.Y = -1 * stats.speed;
                    }
                }
                if (Keyboard.GetState().IsKeyDown(Keys.S)) /* Down */
                {
                    if (down == false)
                    {
                        down = true;
                        downForce.Y = stats.speed;
                    }
                }
                if (Keyboard.GetState().IsKeyDown(Keys.A)) /* Left */
                {
                    if (left == false)
                    {
                        left = true;
                        leftForce.X = -1 * stats.speed;
                    }
                }
                if (Keyboard.GetState().IsKeyDown(Keys.D)) /* Right */
                {
                    if (right == false)
                    {
                        right = true;
                        rightForce.X = stats.speed;
                    }
                }
                if (Keyboard.GetState().IsKeyDown(Keys.F)) /* Toggle Torch */
                {
                    if (stats.torchCount > 0)
                    {
                        torchPress = true;
                    }
                }

                /* Key Up Events */
                if (Keyboard.GetState().IsKeyUp(Keys.W)) /* Up */
                {
                    if (up == true)
                    {
                        up = false;
                        upForce.Y = 0;
                    }
                }
                if (Keyboard.GetState().IsKeyUp(Keys.S)) /* Down */
                {
                    if (down == true)
                    {
                        down = false;
                        downForce.Y = 0;
                    }
                }
                if (Keyboard.GetState().IsKeyUp(Keys.A)) /* Left */
                {
                    if (left == true)
                    {
                        left = false;
                        leftForce.X = 0;
                    }
                }
                if (Keyboard.GetState().IsKeyUp(Keys.D)) /* Right */
                {
                    if (right == true)
                    {
                        right = false;
                        rightForce.X = 0;
                    }
                }
                if (Keyboard.GetState().IsKeyUp(Keys.F)) /* U */
                {
                    if (stats.torchCount > 0 && torchPress)
                    {
                        torchFireMode = !torchFireMode;
                    }
                    torchPress = false;
                }

            }

            bool movement = false;

            if (up || down || left || right)
            {
                movement = true;
            }

            if (movement == true && moving == false)
            {
                startedMoving();
            }
            else if (movement == false && moving == true)
            {
                stoppedMoving();
            }

            #endregion

            forceVector = Vector2.Zero;

            stats.velocity = upForce + downForce + leftForce + rightForce;

            MathHelperHelper.Vector2Normalize(ref stats.velocity);
            stats.velocity *= stats.speed; // normalize

            MathHelperHelper.Vector2Int(ref stats.velocity);

            forceVector.X = stats.velocity.X;
            forceVector.Y = stats.velocity.Y;
        }

        public override void ApplyForceToOtherObjects(GameTime gameTime)
        {
            Vector2 temp = worldCenter + stats.velocity;

            bool validMovementX = ValidMovement(temp.X, worldCenter.Y, gameTime);
            bool validMovementY = ValidMovement(worldCenter.X, temp.Y, gameTime);

            if (stats.noClip)
            {
                validMovementX = true;
                validMovementY = true;
            }

            /* Update force */
            if (validMovementX == false)
            {
                forceVector.X = 0;
            }
            if (validMovementY == false)
            {
                forceVector.Y = 0;
            }


        }

        public override void Move(GameTime gameTime)
        {
            Vector2 force_int = MathHelperHelper.Vector2Int(forceVector);
            worldCenter += force_int;
            boundingBox.X = (int)worldCenter.X;
            boundingBox.Y = (int)worldCenter.Y;
            currentLevel.gamePosX = (int)worldCenter.X;
            currentLevel.gamePosY = (int)worldCenter.Y;

            stats.UpdateDistance(forceVector.Length());

            UpdateATiles();

            /* Update light */
            Vector2 location = new Vector2(worldCenter.X - ((int)currentLevel.mainChar.worldCenter.X - currentLevel.gameModel.gameView.ScreenWidthOver2), worldCenter.Y - ((int)currentLevel.mainChar.worldCenter.Y - currentLevel.gameModel.gameView.ScreenHeightOver2));
            playerLight.Position = location;
        }

        override public void Draw(SpriteBatchWrapper spriteBatch)
        {
            if (!currentLevel.Ending)
            {
                #region Draw Player
                drawLocation.X = worldCenter.X - 50;
                drawLocation.Y = worldCenter.Y - 50;

                if (stats.velocity.X > 0) /* Sprite walks right*/
                {
                    if (stats.lastShotDirection == 0 && shooting) /* shoot right */
                    {
                        WRFRsprite.Draw(spriteBatch, drawLocation, 255, drawHit);
                    }
                    else if (stats.lastShotDirection == 1 && shooting) /* shoot down */
                    {
                        WRFDsprite.Draw(spriteBatch, drawLocation, 255, drawHit);
                    }
                    else if (stats.lastShotDirection == 2 && shooting) /* shoot left */
                    {
                        WRFLsprite.Draw(spriteBatch, drawLocation, 255, drawHit);
                    }
                    else if (stats.lastShotDirection == 3 && shooting) /* shoot up*/
                    {
                        WRFUsprite.Draw(spriteBatch, drawLocation, 255, drawHit);
                    }
                    else
                    {
                        WRFRsprite.Draw(spriteBatch, drawLocation, 255, drawHit);
                    }
                    stats.lastMoveDirection = 0;
                }

                else if (stats.velocity.X < 0)
                { /* Sprite walks left */
                    if (stats.lastShotDirection == 0 && shooting) /* shoot right */
                    {
                        WLFRsprite.Draw(spriteBatch, drawLocation, 255, drawHit);
                    }
                    else if (stats.lastShotDirection == 1 && shooting) /* shoot down */
                    {
                        WLFDsprite.Draw(spriteBatch, drawLocation, 255, drawHit);
                    }
                    else if (stats.lastShotDirection == 2 && shooting) /* shoot left */
                    {
                        WLFLsprite.Draw(spriteBatch, drawLocation, 255, drawHit);
                    }
                    else if (stats.lastShotDirection == 3 && shooting) /* shoot up*/
                    {
                        WLFUsprite.Draw(spriteBatch, drawLocation, 255, drawHit);
                    }
                    else
                    {
                        WLFLsprite.Draw(spriteBatch, drawLocation, 255, drawHit);
                    }
                    stats.lastMoveDirection = 2;
                }

                else if (stats.velocity.Y < 0) /*Sprite walks up*/
                {
                    if (stats.lastShotDirection == 0 && shooting) /* shoot right */
                    {
                        WUFRsprite.Draw(spriteBatch, drawLocation, 255, drawHit);
                    }
                    else if (stats.lastShotDirection == 1 && shooting) /* shoot down */
                    {
                        WUFDsprite.Draw(spriteBatch, drawLocation, 255, drawHit);
                    }
                    else if (stats.lastShotDirection == 2 && shooting) /* shoot left */
                    {
                        WUFLsprite.Draw(spriteBatch, drawLocation, 255, drawHit);
                    }
                    else if (stats.lastShotDirection == 3 && shooting) /* shoot up*/
                    {
                        WUFUsprite.Draw(spriteBatch, drawLocation, 255, drawHit);
                    }
                    else
                    {
                        WUFUsprite.Draw(spriteBatch, drawLocation, 255, drawHit);
                    }
                    stats.lastMoveDirection = 3;
                }
                else if (stats.velocity.Y > 0) /*Sprite walks down*/
                {
                    if (stats.lastShotDirection == 0 && shooting) /* shoot right */
                    {
                        WDFRsprite.Draw(spriteBatch, drawLocation, 255, drawHit);
                    }
                    else if (stats.lastShotDirection == 1 && shooting) /* shoot down */
                    {
                        WDFDsprite.Draw(spriteBatch, drawLocation, 255, drawHit);
                    }
                    else if (stats.lastShotDirection == 2 && shooting) /* shoot left */
                    {
                        WDFLsprite.Draw(spriteBatch, drawLocation, 255, drawHit);
                    }
                    else if (stats.lastShotDirection == 3 && shooting) /* shoot up*/
                    {
                        WDFUsprite.Draw(spriteBatch, drawLocation, 255, drawHit);
                    }
                    else
                    {
                        WDFDsprite.Draw(spriteBatch, drawLocation, 255, drawHit);
                    }
                    stats.lastMoveDirection = 1;
                }
                else /* Draw standing character */
                {
                    if ((stats.lastShotDirection == 0 && shooting)||(stats.lastMoveDirection == 0 && !shooting)) /* face right */
                    {
                        mainCharFacings.DrawFrame(spriteBatch, drawLocation, 2, drawHit);
                    }
                    else if ((stats.lastShotDirection == 1 && shooting) || (stats.lastMoveDirection == 1 && !shooting)) /* face down */
                    {
                        mainCharFacings.DrawFrame(spriteBatch, drawLocation, 0, drawHit);
                    }
                    else if ((stats.lastShotDirection == 2 && shooting) || (stats.lastMoveDirection == 2 && !shooting)) /* face left */
                    {
                        mainCharFacings.DrawFrame(spriteBatch, drawLocation, 3, drawHit);
                    }
                    else if ((stats.lastShotDirection == 3 && shooting) || (stats.lastMoveDirection == 3 && !shooting)) /* face up*/
                    {
                        mainCharFacings.DrawFrame(spriteBatch, drawLocation, 1, drawHit); 
                    }
                }

                //TODO Find a more permanent place for this after redrawing of shadow casting things are redone.
                #region Draw Loots
                foreach (ALoot l in equippedLoot)
                {
                    l.Draw(spriteBatch);
                }

                #endregion Draw Loots
            }
                #endregion
            else
            {
                //draw the teleport animation

                switch (teleportAnimationIndex)
                {
                    case 0:
                        WDFDsprite.DrawFrame(spriteBatch, drawLocation, 0);
                        break;
                    case 1:
                        WLFLsprite.DrawFrame(spriteBatch, drawLocation, 0);
                        break;

                    case 2:
                        WUFUsprite.DrawFrame(spriteBatch, drawLocation, 0);
                        break;

                    case 3:
                        WRFRsprite.DrawFrame(spriteBatch, drawLocation, 0);
                        break;
                }

                foreach (ALoot l in equippedLoot)
                {
                    l.DrawDirection(spriteBatch, teleportAnimationIndex);
                }
            }
        }

        override public bool IsObstacle()
        {
            return true;
        }

        public override void ShotByBullet(BulletType bulletType, GameTime gameTime)
        {
            //TODO
            return;
        }

        override public void SetBoundingPointsOffset()
        {
            SetBoundingPointsOffset(startingWidth, startingHeight);
        }


        private List<Vector2> GetFuturePoints(int x, int y)
        {
            List<Vector2> points = new List<Vector2>();

            points.Add(new Vector2((float)(x - stats.width / 2), (float)(y - stats.height / 2))); //topleft
            points.Add(new Vector2((float)(x + stats.width / 2), (float)(y - stats.height / 2))); //top right
            points.Add(new Vector2((float)(x + stats.width / 2), (float)(y + stats.height / 2))); //bottom right
            points.Add(new Vector2((float)(x - stats.width / 2), (float)(y + stats.height / 2))); //bottom left

            return points;
        }

        public override void getDealtDamage(GameObject fromMe, float thisMuch, GameTime gameTime)
        {
            long currentTime = DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond;

            if (currentTime - lastTimeHurtMillis > 500)
            {
                lastTimeHurtMillis = currentTime;

                drawHit = true;
                lastHit = gameTime.TotalGameTime.TotalMilliseconds;

                double dodgeRoll = rng.NextDouble();

                if (stats.evasion < dodgeRoll)
                {
                    float toLose = thisMuch - stats.defense;
                    if (toLose < 0)
                    {
                        toLose = 0;
                    }
                    stats.health -= toLose;
                    if (stats.health <= 0)
                    {
                        currentLevel.gameModel.endGame(false, fromMe.killText);
                    }
                }
            }
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
        //    return playerLight;
        //}

        private void startedMoving()
        {
            moving = true;
            //footstepsInstance.IsLooped = true;
            //footstepsInstance.Play();
        }

        private void stoppedMoving()
        {
            moving = false;
            //footstepsInstance.IsLooped = false;
            //footstepsInstance.Stop();
        }

        public void equipLoot(ALoot equipThis)
        {
            equippedLoot.Add(equipThis);
            equippedLoot.Sort();
        }

        public void addUpdatingLoot(ALoot thisUpdates)
        {
            updatingLoot.Add(thisUpdates);
        }

        public void setNewLevel(Level newLevel)
        {
            currentLevel = newLevel;
            stats = currentLevel.playerStatsInstance;
            myGun.currentLevel = currentLevel;
        }


        internal void ReplaceGun(AGun aGun)
        {
            if (myGun != null)
            {
                equippedLoot.Remove(myGun);
                updatingLoot.Remove(myGun);
            }
            myGun = aGun;

            equippedLoot.Add(myGun);

            if (myGun.updates)
            {
                addUpdatingLoot(myGun);
            }
        }

        //internal void LefreshLight()
        //{
        //    playerLight.Radius = (int)stats.LightRange;
        //    playerLight.Size = new Vector2((float)playerLight.Radius * 2f);
        //}

        //private void dropTorch(int relX, int relY)
        //{
        //    Console.WriteLine("Dropping torch");
        //    int actualX = (int)worldCenter.X+relX;
        //    int actualY = (int)worldCenter.Y+relY;
        //    Torch torch = new Torch(currentLevel,actualX, actualY);
        //    torch = null;
        //}

        private void throwTorch(int startX, int startY, float velX, float velY, int dirX, int dirY, double shotSpeed)
        {
            Console.WriteLine(this.stats.torchCount);
            if (!rave)
            {
                TorchBullet torch = new TorchBullet(currentLevel, startX, startY, (int)velX, (int)velY, dirX, dirY, shotSpeed);
            }
            else
            {
                GlowStick glowstick = new GlowStick(currentLevel, startX, startY, (int)velX, (int)velY, dirX, dirY, shotSpeed);
            }
            this.stats.torchCount -= 1;
            this.torchFireMode = false;
        }

    }
}
