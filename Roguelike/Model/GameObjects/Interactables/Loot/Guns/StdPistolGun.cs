using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Model.Infrastructure;

namespace Roguelike.Model.GameObjects.Loot
{
    public class StdPistolGun : AGun
    {
        public StdPistolGun(Level level, int x, int y)
            : base(level, x, y)
        {
            gameObjectType = GameObjectType.Interactable;
            bulletType = BulletType.Laser;

            itemId = 4;
        }

        public override void applyStatMods(MainCharacter toMe)
        {
            // do some stats stuff
        }

        public override void setTextures()
        {
            myItemTextureFile = "Objects/Loot/Guns/StdIssuePistol/StdIssuePistolR";

            myAppliedTextureFileL = "Objects/Loot/Guns/StdIssuePistol/StdIssuePistolL";
            myAppliedTextureFileR = "Objects/Loot/Guns/StdIssuePistol/StdIssuePistolR";
            myAppliedTextureFileD = "Objects/Loot/Guns/StdIssuePistol/StdIssuePistolD";

            soundEffectFile = "Sound/pew2";

            //toastText = "Basic Gun!";
            itemName = "";
            itemDescription = "";
        }
    }
}
