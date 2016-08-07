using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Model.Infrastructure;

namespace Roguelike.Model.GameObjects.Loot
{
    public class HM05Loot : ALoot
    {
        public HM05Loot(Level level, int x, int y)
            : base(level, x, y)
        {
            gameObjectType = GameObjectType.Interactable;

            itemId = 6;
        }

        public override void applyStatMods(MainCharacter toMe)
        {
            // do some stats stuff
            toMe.stats.LightRange += 150;
        }

        public override void setTextures()
        {
            myItemTextureFile = "Objects/Loot/HM05/HM05_still";

            myAppliedTextureFileL = "Objects/Loot/HM05/HM05_L";
            myAppliedTextureFileR = "Objects/Loot/HM05/HM05_R";
            myAppliedTextureFileU = "Objects/Loot/HM05/HM05_U";

            itemName = "HM05";
            itemDescription = "A blinding FLASH lights the area!";
        }
    }
}
