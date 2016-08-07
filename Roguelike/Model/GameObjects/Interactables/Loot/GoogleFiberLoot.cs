using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Model.Infrastructure;

namespace Roguelike.Model.GameObjects.Loot
{
    public class GoogleFiberLoot : ALoot
    {
        private float speed_backup;
        private float my_effect;
        private float oldLightRadius;

        public GoogleFiberLoot(Level level, int x, int y)
            : base(level, x, y)
        {
            gameObjectType = GameObjectType.Interactable;

            updates = true;

            itemId = 11;
        }

        public override void applyStatMods(MainCharacter toMe)
        {
            oldLightRadius = toMe.stats.LightRange;

            speed_backup = toMe.stats.speed;
            my_effect = (float)Math.Ceiling(toMe.stats.LightRange / (float)201);

            toMe.stats.speed = (float)(speed_backup + my_effect);
        }

        public override void setTextures()
        {
            myItemTextureFile = "Objects/Loot/GoogleFiber/GoogleFiber";

            itemName = "Google Fiber";
            //itemDescription = "Fuck Comcast";
            itemDescription = "Travel at the speed of light!";
        }

        public override void lootUpdate(GameTime gameTime, MainCharacter toMe)
        {

            if (toMe.stats.LightRange != oldLightRadius)
            {
                toMe.stats.speed -= my_effect;

                //speed_backup = speed;

                my_effect = (float)Math.Ceiling(toMe.stats.LightRange / (float)150);

                toMe.stats.speed += my_effect;

                oldLightRadius = toMe.stats.LightRange;
            }
        }
    }
}
