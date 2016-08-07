using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Model.Infrastructure;

namespace Roguelike.Model.GameObjects.Loot
{
    public class RabbitFootLoot : ALoot
    {
        public RabbitFootLoot(Level level, int x, int y)
            : base(level, x, y)
        {
            gameObjectType = GameObjectType.Interactable;

            itemId = 7;
        }

        public override void applyStatMods(MainCharacter toMe)
        {
            // do some stats stuff
            toMe.stats.luck += 0.1f;
        }

        public override void setTextures()
        {
            myItemTextureFile = "Objects/Loot/RabbitFoot/RabbitFoot";

            itemName = "Rabbit Foot";
            itemDescription = "Lucky!";
        }
    }
}
