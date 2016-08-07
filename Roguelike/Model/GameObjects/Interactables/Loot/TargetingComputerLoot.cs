using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Model.Infrastructure;

namespace Roguelike.Model.GameObjects.Loot
{
    public class TargetingComputerLoot : ALoot
    {

        public TargetingComputerLoot(Level level, int x, int y)
            : base(level, x, y)
        {
            gameObjectType = GameObjectType.Interactable;

            updates = true;

            itemId = 13;
        }

        public override void applyStatMods(MainCharacter toMe)
        {
            currentLevel.gameModel.gameView.minimap.Reveal();
        }

        public override void setTextures()
        {
            myItemTextureFile = "Objects/Loot/targetingComputer/targetingComputer";

            itemName = "Targeting Computer";
            itemDescription = "You aren't Luke Skywalker";
        }

        public override void lootUpdate(GameTime gameTime, MainCharacter toMe)
        {

            if (!toMe.currentLevel.gameModel.gameView.minimap.IsRevealed)
            {
                toMe.currentLevel.gameModel.gameView.minimap.Reveal();
            }
        }
    }
}
