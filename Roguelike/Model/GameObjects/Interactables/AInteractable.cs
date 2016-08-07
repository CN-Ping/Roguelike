using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roguelike.Model.Infrastructure;

namespace Roguelike.Model.GameObjects.Interactables
{
    abstract public class AInteractable : GameObject
    {
        protected bool reupdated = true;

        public AInteractable()
        {
            gameObjectType = GameObjectType.Interactable;
        }

        public AInteractable(Level level, int startX, int startY) : base(level, startX, startY)
        {
            gameObjectType = GameObjectType.Interactable;
        }

        public override bool IsObstacle()
        {
            return true;
        }

        /// <summary>
        /// My hack to make sure interactables apply themselves to the world. 
        /// 
        /// Make sure you call this via base.Update(gameTime) in extending classes
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (reupdated)
            {
                UpdateATiles();
                reupdated = false;
            }
        }

        abstract public void TriggerPlayerInteraction();
    }
}
