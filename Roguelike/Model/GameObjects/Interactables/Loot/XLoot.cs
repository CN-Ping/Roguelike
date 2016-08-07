using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Model.Infrastructure;
using Shadows2D;

namespace Roguelike.Model.GameObjects.Loot
{
    public class XLoot : ALoot
    {
        Random rng = new Random();

        int updateCounter = 0;
        int updateMod = 3;

        public XLoot(Level level, int x, int y)
            : base(level, x, y)
        {
            gameObjectType = GameObjectType.Interactable;

            updates = true;

            itemId = 14;
        }

        public override void applyStatMods(MainCharacter toMe)
        {
            toMe.rave = true;
        }

        public override void setTextures()
        {
            myItemTextureFile = "Objects/Loot/X/x";

            itemName = "X";
            itemDescription = "Party on!";
        }

        public override void lootUpdate(GameTime gameTime, MainCharacter toMe)
        {
            if (updateCounter % updateMod == 0)
            {
                Color c = toMe.GetLightSource().Color;

                byte r = (byte)rng.Next(0, 255);
                byte g = (byte)rng.Next(0, 255);
                byte b = (byte)rng.Next(0, 255);

                toMe.GetLightSource().Color = Color.Lerp(c, new Color(r, g, b), 0.1f);
            }

            updateCounter = (updateCounter + 1) % updateMod;
        }
    }
}
