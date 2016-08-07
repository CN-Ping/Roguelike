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
    public class TorchPickup : APickup
    {

        public TorchPickup(Level levelIn, int x, int y)
            : base(levelIn, x, y)
        {
            gameObjectType = GameObjectType.Interactable;
            SetBoundingPointsOffset();
            LoadContent();
        }

        public override void applyStatsMod(MainCharacter toMe)
        {
            toMe.stats.torchCount += 1;
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
            myTextureFileName = "Objects/Pickups/TorchPickup/TorchPickup";
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
