using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Model.Infrastructure;
using Roguelike.Model.GameObjects.Projectiles;

namespace Roguelike.Model.GameObjects
{
    public enum BulletType 
    {
        Laser,
        Filler
    }

    abstract public class ABullet : GameObject
    {

        protected BulletType type;

        protected int speed;
        protected int xVel;
        protected int yVel;

        public ABullet(Level level)
            : base()
        {
            layerType = LayerType.Stuff;
            currentLevel = level;
        }

        public ABullet(Level level, int startX, int startY) : base(level, startX, startY)
        {
            layerType = LayerType.Stuff;
        }

        public virtual void Refresh(Level l, int startX, int startY)
        {
            this.currentLevel = l;

            this.worldCenter = new Vector2(startX, startY);
            this.boundingBox.X = startX;
            this.boundingBox.Y = startY;
        }

        public virtual void SetStats(int startX, int startY, int playerXVel, int playerYVel, int dirX, int dirY, double shotSpeed)
        {
            speed = (int)shotSpeed;
            xVel = playerXVel + dirX * speed; /*playerXVel and playerYVel are the speed the player was going when they shot */
            yVel = playerYVel + dirY * speed;
            worldCenter.X = startX;
            worldCenter.Y = startY;
        }
        
        public BulletType GetBulletType()
        {
            return type;
        }

        public override bool IsObstacle()
        {
            return false;
        }

        public override void Update(GameTime gameTime)
        {
            worldCenter.X += xVel;
            worldCenter.Y += yVel;
            boundingBox.X = (int)worldCenter.X - boundingBox.Width / 2;
            boundingBox.Y = (int)worldCenter.Y;

            UpdateATiles();

            List<GameObject> collisions = CollidesWith();
            for (int i = 0; i < collisions.Count; i++)
            {
                //TODO: check for specific collisions rather than just obstacles
                if ((collisions[i].IsObstacle() && collisions[i].GetType() != typeof(MainCharacter))&&collisions[i].GetType()!=typeof(TorchBullet))
                {
                    collisions[i].ShotByBullet(type, gameTime);

                    RemoveFromATiles();
                    currentLevel.removeGameObject(this);

                    BulletPool.ReturnBullet(this);

                    

                    break;
                }
            }
        }

        public void CleanUp()
        {
            xVel = 0;
            yVel = 0;
            RemoveFromATiles();
        }

    }
}
