using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Model.Infrastructure;

namespace Roguelike.Model.GameObjects.Loot
{
    public class DCellLoot : ALoot
    {
        public DCellLoot(Level level, int x, int y)
            : base(level, x, y)
        {
            gameObjectType = GameObjectType.Interactable;

            itemId = 5;
        }

        public override void applyStatMods(MainCharacter toMe)
        {
            // do some stats stuff
            toMe.stats.LightRange += 150;
        }

        public override void setTextures()
        {
            myItemTextureFile = "Objects/Loot/DCell/DCell";

            itemName = "D-Cell";
            itemDescription = "Let there be light!";
        }
    }
}
