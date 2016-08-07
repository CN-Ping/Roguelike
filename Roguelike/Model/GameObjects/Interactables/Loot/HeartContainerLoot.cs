using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Model.Infrastructure;

namespace Roguelike.Model.GameObjects.Loot
{
    public class HeartContainerLoot : ALoot
    {
        public HeartContainerLoot(Level level, int x, int y)
            : base(level, x, y)
        {
            gameObjectType = GameObjectType.Interactable;

            itemId = 2;
        }

        public override void applyStatMods(MainCharacter toMe)
        {
            // do some stats stuff
            toMe.stats.health += 10;
            toMe.stats.maxHealth += 10;
        }

        public override void setTextures()
        {
            myItemTextureFile = "Objects/Loot/HeartContainer/heartContainer";

            itemName = "Heart Container";
            itemDescription = "Health Up!";
        }
    }
}
