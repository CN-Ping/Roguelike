using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Model.Infrastructure;

namespace Roguelike.Model.GameObjects.Loot
{
    public class JaynesHatLoot : ALoot
    {
        public JaynesHatLoot(Level level, int x, int y)
            : base(level, x, y)
        {
            gameObjectType = GameObjectType.Interactable;

            itemId = 1;
        }

        public override void applyStatMods(MainCharacter toMe)
        {
            // do some stats stuff
            toMe.stats.damage++;
        }

        public override void setTextures()
        {
            myItemTextureFile = "Objects/Loot/Hat/HatD";
            // you would also set the other texture string here if you were int othat kind of thing.
            myAppliedTextureFileL = "Objects/Loot/Hat/HatL";
            myAppliedTextureFileR = "Objects/Loot/Hat/HatR";
            myAppliedTextureFileU = "Objects/Loot/Hat/HatU";
            myAppliedTextureFileD = "Objects/Loot/Hat/HatD";

            itemName = "Jayne's Hat";
            itemDescription = "Damage Up!";
        }
    }
}
