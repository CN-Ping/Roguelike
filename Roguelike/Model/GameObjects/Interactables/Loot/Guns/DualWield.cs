using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Model.Infrastructure;
using Roguelike.Model.GameObjects.Projectiles;

namespace Roguelike.Model.GameObjects.Loot
{
    public class DualWield : AGun
    {
        public DualWield(Level level, int x, int y)
            : base(level, x, y)
        {
            gameObjectType = GameObjectType.Interactable;

            itemId = 12;
        }

        public override void applyStatMods(MainCharacter toMe)
        {
            // do some stats stuff
            toMe.ReplaceGun(this);
        }

        public override void setTextures()
        {
            myItemTextureFile = "Objects/Loot/DualWield/DualWield";

            myAppliedTextureFileL = "Objects/Loot/Guns/DualWieldPistol/DualWieldPistolL";
            myAppliedTextureFileR = "Objects/Loot/Guns/DualWieldPistol/DualWieldPistolR";
            myAppliedTextureFileD = "Objects/Loot/Guns/DualWieldPistol/DualWieldPistolD";

            soundEffectFile = "Sound/pew2";

            itemName = "Dual Wield";
            itemDescription = "Huh, you picked up another pistol.";
        }

        

        /// <summary>
        /// Shoot two bullets diagonally
        /// </summary>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="playerXVel"></param>
        /// <param name="playerYVel"></param>
        /// <param name="dirX"></param>
        /// <param name="dirY"></param>
        /// <param name="shotSpeed"></param>
        public override void shoot(int startX, int startY, float playerXVel, float playerYVel, int dirX, int dirY, double shotSpeed)
        {
            {
                Vector2 dir1;
                Vector2 dir2;
                // left
                if (dirX == -1 && dirY == 0)
                {
                    dir1 = new Vector2(-1, -1);
                    dir2 = new Vector2(-1, 1);
                }
                // right
                else if (dirX == 1 && dirY == 0)
                {
                    dir1 = new Vector2(1, -1);
                    dir2 = new Vector2(1, 1);
                }
                // up
                else if (dirX == 0 && dirY == -1)
                {
                    dir1 = new Vector2(1, -1);
                    dir2 = new Vector2(-1, -1);
                }
                // down
                else
                {
                    dir1 = new Vector2(1, 1);
                    dir2 = new Vector2(-1, 1);
                }
                int newXVel = (int)(playerXVel * 0.5);
                int newYVel = (int)(playerYVel * 0.5);

                ABullet newBullet1 = BulletPool.FetchBullet(BulletType.Laser, currentLevel, startX, startY - 5);
                ABullet newBullet2 = BulletPool.FetchBullet(BulletType.Laser, currentLevel, startX, startY + 5);
                //diagonal code
                //newBullet1.SetStats(startX, startY, newXVel, newYVel, (int)dir1.X, (int)dir1.Y, shotSpeed);
                //newBullet2.SetStats(startX, startY, newXVel, newYVel, (int)dir2.X, (int)dir2.Y, shotSpeed);
                if (dirY == 0)
                {
                    newBullet1.SetStats(startX, startY - 10, newXVel, newYVel, dirX, dirY, shotSpeed);
                    newBullet2.SetStats(startX, startY + 10, newXVel, newYVel, dirX, dirY, shotSpeed);
                }
                else
                {
                    newBullet1.SetStats(startX + 10, startY, newXVel, newYVel, dirX, dirY, shotSpeed);
                    newBullet2.SetStats(startX - 10, startY, newXVel, newYVel, dirX, dirY, shotSpeed);
                }

                currentLevel.addGameObject(newBullet1);
                currentLevel.addGameObject(newBullet2);
            }

            soundEffect.Play(soundEffectVolume, soundEffectPitch, soundEffectPan);
        }
    }
}
