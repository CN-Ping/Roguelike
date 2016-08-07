using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Roguelike.View;
using Roguelike.Model.Infrastructure;
//using Shadows2D;

namespace Roguelike.Model.GameObjects.Interactables
{
    public class DoorInteractable : AInteractable
    {

        protected Texture2D closedTexture;
        protected Texture2D openTexture;
        private bool isOpen;
        SoundEffect doorSound;
        SoundEffectInstance doorSoundInstance;

        #region drawing variables
        //Rectangle sourceRectangle;
        #endregion drawing variables

        public DoorInteractable(Level level, int startX, int startY) : base(level, startX, startY)
        {
            layerType = LayerType.AbovePlayer;
            isOpen = false;
            LoadContent();
            SetBoundingPointsOffset();
        }

        override public void LoadContent(){
            closedTexture = currentLevel.gameModel.Game.Content.Load<Texture2D>("Textures/doorClosed");
            openTexture = currentLevel.gameModel.Game.Content.Load<Texture2D>("Textures/doorOpen");
            
            textureHeight = openTexture.Height;
            textureWidth = openTexture.Width;
            textureHeightOver2 = textureHeight / 2;
            textureWidthOver2 = textureWidth / 2;

            doorSound = currentLevel.gameModel.Game.Content.Load<SoundEffect>("Sound/doorOpen");
            doorSoundInstance = doorSound.CreateInstance();

            origin = new Vector2(textureWidthOver2, textureHeightOver2);
            drawLocation = new Vector2(worldCenter.X, worldCenter.Y);

            //sourceRectangle = new Rectangle(0, 0, textureWidth, textureHeight);
        }

        public override void SetTexture()
        {
            myTextureFileName = "Textures/doorClosed";
        }

        override public void UnloadContent()
        {
            throw new NotImplementedException();
        }

        override public void Update(GameTime gameTime)
        {
            UpdateATiles();
        }

        override public void Draw(SpriteBatchWrapper spriteBatch)
        {
            /*
             * To change the rotation, do something like base.textureRotation + Mathhelper.Pi if you want to flip it upside down
             */
            if (isOpen == true)
            {
                spriteBatch.Draw(openTexture, drawLocation, null, Color.White, base.textureRotation, origin, 1.0f, SpriteEffects.None, 1);
            }
            else
            {
                spriteBatch.Draw(closedTexture, drawLocation, null, Color.White, base.textureRotation, origin, 1.0f, SpriteEffects.None, 1);
            }
        }

        override public bool IsObstacle()
        {
            if (isOpen == true)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public override bool CastsShadow()
        {
            if (isOpen == true)
            {
                return true;
            }
            else
            {
                return true;
            }
        }

        //public override void DrawCaster(ShadowCasterMap shadowMap)
        //{
        //    if (this.CastsShadow() && !isOpen)
        //    {
        //        shadowMap.AddShadowCaster(closedTexture, drawLocation, null, Color.White, base.textureRotation, origin, 1.0f, SpriteEffects.None, 1);
        //    }
        //}


        override public void SetBoundingPointsOffset()
        {
            SetBoundingPointsOffset(textureWidth, textureHeight);
        }

        public override void TriggerPlayerInteraction()
        {
            if (isOpen == false)
            {
                doorSoundInstance.Play();
            }
            isOpen = true;
        }
    }
}
