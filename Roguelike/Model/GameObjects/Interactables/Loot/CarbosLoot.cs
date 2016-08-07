using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Model.Infrastructure;

namespace Roguelike.Model.GameObjects.Loot
{
    public class CarbosLoot : ALoot
    {
        public CarbosLoot(Level level, int x, int y)
            : base(level, x, y)
        {
            gameObjectType = GameObjectType.Interactable;

            itemId = 9;
        }

        public override void applyStatMods(MainCharacter toMe)
        {
            // do some stats stuff
            toMe.stats.speed += 1;
        }

        public override void setTextures()
        {
            myItemTextureFile = "Objects/Loot/Carbos/Carbos";

            itemName = "Carbos";
            itemDescription = "SPD +";
        }
    }
}
