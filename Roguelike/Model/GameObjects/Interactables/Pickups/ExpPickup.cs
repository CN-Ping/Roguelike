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
    public class ExpPickup : APickup
    {
        private float expAmount;

        public ExpPickup(Level levelIn, int x, int y, float exp) : base(levelIn, x, y)
        {
            gameObjectType = GameObjectType.Interactable;
            expAmount = exp;
            SetBoundingPointsOffset();
            LoadContent();
        }

        public override void applyStatsMod(MainCharacter toMe)
        {
            toMe.stats.tempExp += (expAmount * toMe.stats.pickupPotency);
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
            myTextureFileName = "Objects/Pickups/ExpPickup/ExpPickup";
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
            applyStats(currentLevel.mainChar);
        }

    }
}
