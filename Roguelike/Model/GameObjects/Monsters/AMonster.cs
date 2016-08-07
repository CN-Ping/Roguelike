using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.View;
using Roguelike.Model.GameObjects.Monsters.AI;
using Roguelike.Model.Lighting;
using Roguelike.Model.Infrastructure;
using Roguelike.Model.GameObjects.Pickups;
using Roguelike.Util;

namespace Roguelike.Model.GameObjects.Monsters
{
    abstract public class AMonster : GameObject
    {
        protected AnimatedSprite sprite;

        protected float speed;
        public Vector2 velocity = new Vector2();

        protected float health;

        protected List<AMonsterAI> myAIs = new List<AMonsterAI>();

        public Color shadowLevel = Color.White;

        public double dropChance = 0.0;

        public static Random rng = new Random();

        protected bool drawHit;
        protected int msToBeRed;
        protected double lastHit;

        public AMonster() : base()
        {
            Initialize();
        }

        public AMonster(Level level, int startX, int startY) : base(level, startX, startY)
        {
            Initialize();

        }

        public override void Update(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.TotalMilliseconds - lastHit > msToBeRed)
            {
                drawHit = false;
            }
            SecondaryUpdate(gameTime);
        }

        public override void ComputeVelocity(GameTime gameTime)
        {
            base.ComputeVelocity(gameTime);

            Vector2 direction = new Vector2();
            // this currently assumes we want every AI weighted equally. We may not actually want that behaviour. 
            foreach (AMonsterAI ai in myAIs) {
                direction += ai.computeMovementVector(gameTime.TotalGameTime.TotalMilliseconds);
            }

            MathHelperHelper.Vector2Normalize(ref direction);

            velocity = direction * speed;
            //MathHelperHelper.Vector2Int(velocity); // dont forget to do this later, dumbo

            forceVector.X = velocity.X;
            forceVector.Y = velocity.Y;

            foreach (Vector2 force in forcesActingOnMe) {
                forceVector += force;
            }

            forcesActingOnMe.Clear();
        }

        public override void ApplyForceToOtherObjects(GameTime gameTime)
        {
            base.ApplyForceToOtherObjects(gameTime);

            Vector2 temp = worldCenter + forceVector;

            bool validMovementX = ValidMovement(temp.X, worldCenter.Y, gameTime);
            bool validMovementY = ValidMovement(worldCenter.X, temp.Y, gameTime);

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

        public override void Draw(SpriteBatchWrapper spriteBatch)
        {
            //if (inLight == true){
                DrawMonster(spriteBatch, drawHit);
            //}
        }

        private void Initialize()
        {
            gameObjectType = GameObjectType.Monster;
            isStaticObject = false;
            drawHit = false;
            msToBeRed = 100;
            lastHit = double.MinValue;
        }

        abstract public void DrawMonster(SpriteBatchWrapper spriteBatch, bool hit);

        /// <summary>
        /// An update function to be implemented by the monster so that 
        /// AMonster and the derived Monster can have separate updates.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void getDealtDamage(GameObject fromMe, float thisMuch, GameTime gameTime)
        {
            // Damage animation
            drawHit = true;
            lastHit = gameTime.TotalGameTime.TotalMilliseconds;

            double dodgeRoll = rng.NextDouble();
            if (currentLevel.mainChar.stats.critChance > dodgeRoll)
            {
                thisMuch += thisMuch * currentLevel.mainChar.stats.critDamage;
            }
            health -= thisMuch;
        }

        public override bool CastsShadow()
        {
            return true;
        }

        public override void DrawCaster(Shadows2D.ShadowCasterMap shadowMap)
        {
            sprite.DrawCaster(shadowMap, drawLocation);
        }

        abstract public void SecondaryUpdate(GameTime gameTime);

        /// <summary>
        /// Call this to have the monster deal its damage to the player
        /// </summary>
        abstract public void DealDamage(GameTime gameTime);

        //public Color getShadowLevelAtRelLoc(Vector2 relLoc){
        //    Model gameModel = currentLevel.gameModel;
        //    View.View view = gameModel.gameView;
        //    if (view.colorData == null)
        //    {
        //        shadowLevel =  Color.White;
        //    }
        //    Vector2 screenLocation = new Vector2(worldCenter.X+relLoc.X - ((int)gameModel.currentLevel.mainChar.worldCenter.X - gameModel.gameView.ScreenWidthOver2),
        //        worldCenter.Y+relLoc.Y - ((int)gameModel.currentLevel.mainChar.worldCenter.Y - gameModel.gameView.ScreenHeightOver2));

        //    Color pixel = view.getShadowColorAtPixel((int)screenLocation.X, (int)screenLocation.Y);
        //    byte r = pixel.R;
        //    byte g = pixel.G;
        //    byte b = pixel.B;

        //    byte level = (byte)(((double)r + (double)g + (double)b) / 3);
        //    pixel.R = level;
        //    pixel.G = level;
        //    pixel.B = level;
        //    return pixel;
        //}

        virtual public void OnMonsterDeath()
        {
            double toBeat = rng.NextDouble();
            if (currentLevel.mainChar.stats.luck * dropChance > toBeat)
            {
                APickup drop = currentLevel.pickupGen.getRandomPickup(currentLevel, (int)worldCenter.X, (int)worldCenter.Y);
                currentLevel.addGameObject(drop);
            }
            currentLevel.removeGameObject(this);

            foreach(AMonsterAI ai in myAIs) {
                ai.MonsterDied(this);
            }
            
        }

        public override void ShotByBullet(BulletType bulletType, GameTime gameTime)
        {
            getDealtDamage(currentLevel.mainChar, (int)currentLevel.mainChar.stats.damage, gameTime);
            if (health <= 0)
            {
                OnMonsterDeath();
            }
        }
    }
}
