using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.View;
using Roguelike.Model.Infrastructure;

namespace Roguelike.Model.GameObjects.Pickups
{
    public class HealthPickup : APickup
    {
        public HealthPickup(Level levelIn, int x, int y) : base(levelIn, x, y)
        {
            gameObjectType = GameObjectType.Interactable;
            SetBoundingPointsOffset();
            LoadContent();
        }

        public override void applyStatsMod(MainCharacter toMe)
        {
            if (toMe.stats.health < toMe.stats.maxHealth)
            {
                toMe.stats.health += 10 * toMe.stats.pickupPotency;
                if (toMe.stats.health > toMe.stats.maxHealth)
                {
                    toMe.stats.health = toMe.stats.maxHealth;
                }
            }
        }

        public override void LoadContent()
        {
            base.LoadContent();

            layerType = LayerType.Stuff;
            gameObjectType = GameObjectType.Interactable;
            UpdateATiles();
        }

        public override void SetTexture()
        {
            myTextureFileName = "Objects/Pickups/HealthPickup/healthPill";
        }

        public override void UnloadContent()
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            return;
        }

        public override void Draw(SpriteBatchWrapper spriteBatch)
        {
            spriteBatch.Draw(texture2D, drawLocation, Color.White);
        }

        public override void TriggerPlayerInteraction()
        {
            if (currentLevel.mainChar.stats.health < currentLevel.mainChar.stats.maxHealth)
            {
                applyStats(currentLevel.mainChar);
            }
        }

    }
}
