using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roguelike.Model.GameObjects.Monsters
{
    public class MothMonster : AMonster
    {

        private float damage = 2f;

        public MothMonster()
        {
            health = 2;
            speed = 4;
            dropChance = .15;
            layerType = LayerType.Stuff;
            SetBoundingPointsOffset();
            LoadContent();
            dropChance = 0.15; // this should be lowered.
            inertia_0_to_1 = 0.1f;

            //TODO make a attracted to light
            //myAIs.Add(new FlockingAI(this));

            maxForceClamp = 2 * speed;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            //sprite = new AnimatedSprite(texture2D, 1, 7);
        }

        public override void DrawMonster(View.SpriteBatchWrapper spriteBatch, bool hit)
        {
            throw new NotImplementedException();
        }

        public override void SecondaryUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        {
            sprite.Update(gameTime);

            UpdateATiles();
        }

        public override void DealDamage(GameTime gameTime)
        {
            currentLevel.mainChar.getDealtDamage(this, damage, gameTime);
            return;
        }

        public override void UnloadContent()
        {
            return;
        }

        public override void SetTexture()
        {
            //myTextureFileName = "Sprites/moth";
        }

        public override bool CastsShadow()
        {
            return false;
        }
    }
}
