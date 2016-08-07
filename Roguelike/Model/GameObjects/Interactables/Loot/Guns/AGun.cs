using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roguelike.Model.Infrastructure;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using Roguelike.Model.GameObjects.Projectiles;

namespace Roguelike.Model.GameObjects.Loot
{
    abstract public class AGun : ALoot
    {
        protected BulletType bulletType = BulletType.Filler;

        protected SoundEffect soundEffect;
        protected float soundEffectVolume = 0.05f;
        protected float soundEffectPitch = -1.0f;
        protected float soundEffectPan = 0.0f;

        protected String soundEffectFile;

        public AGun(Level level, int x, int y) : base(level, x, y)
        {
        }

        public override void applyStatMods(MainCharacter toMe)
        {
            toMe.ReplaceGun(this);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            soundEffect = currentLevel.gameModel.Game.Content.Load<SoundEffect>(soundEffectFile);
        }

        virtual public void shoot(int startX, int startY, float playerXVel, float playerYVel, int dirX, int dirY, double shotSpeed)
        {
             {
                int newXVel = (int)(playerXVel * 0.5);
                int newYVel = (int)(playerYVel * 0.5);

                ABullet newBullet = BulletPool.FetchBullet(BulletType.Laser, currentLevel, startX, startY);
                newBullet.SetStats(startX, startY, newXVel, newYVel, dirX, dirY, shotSpeed);
                currentLevel.addGameObject(newBullet);
            }

            soundEffect.Play(soundEffectVolume, soundEffectPitch, soundEffectPan);
        }

    }
}
