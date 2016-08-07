using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.View;
using Roguelike.Model.Infrastructure;
//using Shadows2D;

namespace Roguelike.Model.GameObjects.Interactables
{
    public class AdvanceLevelInteractable : AInteractable
    {
        #region drawing variables
        //Rectangle sourceRectangle;
        public TextureSplat splat;
        #endregion drawing variables

        public AdvanceLevelInteractable(Level level, int startX, int startY) : base(level, startX, startY)
        {
            layerType = LayerType.Stuff;
            LoadContent();
            SetBoundingPointsOffset();

            /* Create the texture splat to go with this */
            splat = new TextureSplat(currentLevel, startX, startY, "Objects/AdvanceLevelTop");
            splat.layerType = LayerType.AbovePlayer;
            currentLevel.addGameObject(splat);

            //setRotation(MathHelper.Pi);
        }

        public override void SetTexture()
        {
            myTextureFileName = "Objects/AdvanceLevelBase";
        }

        override public void UnloadContent()
        {
            throw new NotImplementedException();
        }

        override public void Update(GameTime gameTime) 
        {
            base.Update(gameTime);
        }

        override public void Draw(SpriteBatchWrapper spriteBatch)
        {
            /*
             * To change the rotation, do something like base.textureRotation + Mathhelper.Pi if you want to flip it upside down
             */
            spriteBatch.Draw(texture2D, worldCenter, null, Color.White, base.textureRotation, origin, 1.0f, SpriteEffects.None, 1);
        }

        override public bool IsObstacle()
        {
            return false;
        }


        override public void SetBoundingPointsOffset()
        {
            SetBoundingPointsOffset(20, 20);
        }

        public override void TriggerPlayerInteraction()
        {
            //TODO move player to the center of the pad
            //currentLevel.mainChar.worldCenter = this.worldCenter;
            //currentLevel.mainChar.drawLocation.X = worldCenter.X - 50;
            //currentLevel.mainChar.drawLocation.Y = worldCenter.Y - 50;

            //foreach (Loot.ALoot l in currentLevel.mainChar.equippedLoot)
            //{
            //    l.worldCenter = currentLevel.mainChar.worldCenter;
            //    l.drawLocation.X = l.worldCenter.X - l.textureWidthOver2;
            //    l.drawLocation.Y = l.worldCenter.Y - l.textureHeightOver2;
            //}

            worldCenter = currentLevel.mainChar.worldCenter - new Vector2(5, 0);
            splat.worldCenter = currentLevel.mainChar.worldCenter - new Vector2(5, 0);
//            drawLocation = currentLevel.mainChar.drawLocation;

            currentLevel.gameModel.gameView.whiteTransparency.A = 0;

            currentLevel.Ending = true;
        }
    }
}
