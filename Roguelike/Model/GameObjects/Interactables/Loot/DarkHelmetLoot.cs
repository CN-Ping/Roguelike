using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Model.Infrastructure;

namespace Roguelike.Model.GameObjects.Loot
{
    public class DarkHelmetLoot : ALoot
    {
        public DarkHelmetLoot(Level level, int x, int y)
            : base(level, x, y)
        {
            gameObjectType = GameObjectType.Interactable;

            itemId = 8;
        }

        public override void applyStatMods(MainCharacter toMe)
        {
            // do some stats stuff
            toMe.stats.LightRange -= 80;

            toMe.stats.defense += 1;

            toMe.stats.torchCount += 10;
        }

        public override void setTextures()
        {
            myItemTextureFile = "Objects/Loot/DarkHelmet/DarkHelmetD";

            myAppliedTextureFileL = "Objects/Loot/DarkHelmet/DarkHelmetL";
            myAppliedTextureFileR = "Objects/Loot/DarkHelmet/DarkHelmetR";
            myAppliedTextureFileU = "Objects/Loot/DarkHelmet/DarkHelmetU";
            myAppliedTextureFileD = "Objects/Loot/DarkHelmet/DarkHelmetD";

            itemName = "Dark Helmet";
            itemDescription = "I can't breathe in this thing!";
        }
    }
}
