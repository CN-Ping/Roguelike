using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Model.Infrastructure;

namespace Roguelike.Model.GameObjects.Loot
{
    public class FillerLoot : ALoot
    {
        public FillerLoot(Level level, int x, int y)
            : base(level, x, y)
        {
            gameObjectType = GameObjectType.Interactable;

            // this is the default value. included for completeness.
            itemId = 0;
        }

        public override void applyStatMods(MainCharacter toMe)
        {
            // do some stats stuff
            toMe.stats.maxHealth++;
        }

        public override void setTextures()
        {
            myItemTextureFile = "Textures/Misc/fillerTexture";
            // you would also set the other texture string here if you were int othat kind of thing.
            myAppliedTextureFileL = "Textures/Misc/fillerTexture";
            myAppliedTextureFileR = "Textures/Misc/fillerTexture";
            myAppliedTextureFileU = "Textures/Misc/fillerTexture";
            myAppliedTextureFileD = "Textures/Misc/fillerTexture";

            // if your applied textures were animated, you'd need to overwrite the appliedTexture*Dimensions variables
            // i.e. appliedTextureUDimensions = new int[] {6, 1};
            // NOTE: syncing this with the player is not yet implemented. dont animate stuff for now :b
            itemName = "Filler Loot";
            itemDescription = "Does nothing probably!";
        }
    }
}
