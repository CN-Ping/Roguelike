using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roguelike.Model.Infrastructure;
using System.Collections.Concurrent;

namespace Roguelike.Model.GameObjects.Projectiles
{
    public class BulletPool
    {

        static Dictionary<BulletType, ConcurrentQueue<ABullet>> pools;

        public static void Init(Level currentLevel)
        {
            pools = new Dictionary<BulletType, ConcurrentQueue<ABullet>>();


            // replace this with an enum to create multiple bullet types
            pools[BulletType.Laser] = new ConcurrentQueue<ABullet>();
            for (int i = 0; i < 100; i++)
            {
                pools[BulletType.Laser].Enqueue(new LaserBullet(currentLevel));
            }
        }

        public static void ReturnBullet(ABullet returnMe) {
            pools[returnMe.GetBulletType()].Enqueue(returnMe);
        }


        public static ABullet FetchBullet(BulletType t, Level currentLevel, int startX, int startY)
        {
            ABullet b;

            if (pools[t].TryDequeue(out b)) {
                b.Refresh(currentLevel, startX, startY);
                return b;
            }

            else
            {
                switch (t)
                {
                    case BulletType.Laser:
                        return new LaserBullet(currentLevel, startX, startY);
                    default:
                        throw new Exception("ERROR: Bullet Type specified is not instantiable.\n");
                }
            }

        }
    }
}
