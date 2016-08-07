using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Model.Infrastructure;

namespace Roguelike.Model.GameObjects.Loot
{
    public class CloverLoot : ALoot
    {
        public CloverLoot(Level level, int x, int y)
            : base(level, x, y)
        {
            gameObjectType = GameObjectType.Interactable;

            itemId = 3;
        }

        public override void applyStatMods(MainCharacter toMe)
        {
            // do some stats stuff
            toMe.stats.luck += 0.12f;
        }

        public override void setTextures()
        {
            myItemTextureFile = "Objects/Loot/Clover/clover";

            itemName = "4-Leaf Clover";
            itemDescription = "Luck Up!";
        }
    }
}
