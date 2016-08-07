using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Model.Infrastructure;

namespace Roguelike.Model.GameObjects.Loot
{
    public class DarthVaderHelmetLoot : ALoot
    {
        public DarthVaderHelmetLoot(Level level, int x, int y)
            : base(level, x, y)
        {
            gameObjectType = GameObjectType.Interactable;

            itemId = 10;
        }

        public override void applyStatMods(MainCharacter toMe)
        {
            // do some stats stuff
            toMe.stats.damage += 1;
        }

        public override void setTextures()
        {
            myItemTextureFile = "Objects/Loot/DarthVaderHelmet/DVHelmetD";

            myAppliedTextureFileD = "Objects/Loot/DarthVaderHelmet/DVHelmetD";
            myAppliedTextureFileL = "Objects/Loot/DarthVaderHelmet/DVHelmetL";
            myAppliedTextureFileR = "Objects/Loot/DarthVaderHelmet/DVHelmetR";
            myAppliedTextureFileU = "Objects/Loot/DarthVaderHelmet/DVHelmetU";

            itemName = "Darth Vader's Helmet";
            itemDescription = "The Force is strong with this one.";
        }
    }
}
