using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Model.GameObjects.Interactables;
using Roguelike.View;
using Roguelike.Model.Infrastructure;

namespace Roguelike.Model.GameObjects.Loot
{
    public abstract class ALoot : AInteractable, IComparable<ALoot>
    {
        protected String myItemTextureFile;

        protected static Texture2D nullTexture;
        protected static AnimatedSprite nullAnimation;

        protected String myAppliedTextureFileL;
        protected String myAppliedTextureFileR;
        protected String myAppliedTextureFileU;
        protected String myAppliedTextureFileD;

        protected Texture2D itemTexture;
        protected AnimatedSprite appliedTextureL;
        protected AnimatedSprite appliedTextureR;
        protected AnimatedSprite appliedTextureU;
        protected AnimatedSprite appliedTextureD;

        protected int[] appliedTextureLDimensions = new int[] { 1, 1 };
        protected int[] appliedTextureRDimensions = new int[] { 1, 1 };
        protected int[] appliedTextureUDimensions = new int[] { 1, 1 };
        protected int[] appliedTextureDDimensions = new int[] { 1, 1 };

        protected bool equipped = false;
        public bool updates = false;

        protected String itemName;
        protected String itemDescription;

        // 0 draws first 
        protected int drawOrder = 0;
        public int itemId = 0;

        int shotDir;
        int moveDir;
        bool shooting;

        public ALoot()
            : base()
        {
            initializeNullTexture();
            LoadContent();
        }

        public ALoot(Level level, int x, int y)
            : base(level, x, y)
        {
            initializeNullTexture();
            LoadContent();
        }

        private void initializeNullTexture()
        {
            if (nullTexture == null)
            {
                nullTexture = new Texture2D(currentLevel.gameModel.Game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                Color[] color = new Color[1];
                for (int i = 0; i < color.Length; i++)
                {
                    color[i] = Color.Transparent;
                }
                nullTexture.SetData(color);

                nullAnimation = new AnimatedSprite(nullTexture, 1, 1);
            }
        }

        public void SetData(Level level, int x, int y)
        {
            currentLevel = level;
            LoadContent();
        }

        public override void SetTexture()
        {
            // this does nothing, and i won't call the other thing
        }

        public override void SetBoundingPointsOffset()
        {
            SetBoundingPointsOffset(textureWidth, textureHeight);
        }

        public override void LoadContent()
        {
            Roguelike Game = currentLevel.gameModel.Game;
            setTextures();
            itemTexture = Game.Content.Load<Texture2D>(myItemTextureFile);
            textureHeight = itemTexture.Height;
            textureWidth = itemTexture.Width;
            textureWidthOver2 = textureWidth / 2;
            textureHeightOver2 = textureHeight / 2;

            drawLocation = new Vector2(worldCenter.X - textureWidthOver2, worldCenter.Y - textureHeightOver2);

            if (myAppliedTextureFileL != null)
            {
                appliedTextureL = new AnimatedSprite(Game.Content.Load<Texture2D>(myAppliedTextureFileL), appliedTextureLDimensions[0], appliedTextureLDimensions[1]);
            }
            else
            {
                appliedTextureL = nullAnimation;
            }

            if (myAppliedTextureFileR != null)
            {
                appliedTextureR = new AnimatedSprite(Game.Content.Load<Texture2D>(myAppliedTextureFileR), appliedTextureRDimensions[0], appliedTextureRDimensions[1]);
            }
            else
            {
                appliedTextureR = nullAnimation;
            }

            if (myAppliedTextureFileU != null)
            {
                appliedTextureU = new AnimatedSprite(Game.Content.Load<Texture2D>(myAppliedTextureFileU), appliedTextureUDimensions[0], appliedTextureUDimensions[1]);
            }
            else
            {
                appliedTextureU = nullAnimation;
            }

            if (myAppliedTextureFileD != null)
            {
                appliedTextureD = new AnimatedSprite(Game.Content.Load<Texture2D>(myAppliedTextureFileD), appliedTextureDDimensions[0], appliedTextureDDimensions[1]);
            }
            else
            {
                appliedTextureD = nullAnimation;
            }

            layerType = LayerType.Stuff;
            gameObjectType = GameObjectType.Interactable;

            SetBoundingPointsOffset();

            UpdateATiles();

        }

        public override void UnloadContent()
        {
            //throw new NotImplementedException();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            if (updates && equipped)
            {
                // this could probably be handled more gracefully
                lootUpdate(gameTime, currentLevel.mainChar);
            }

        }

        public override void Draw(SpriteBatchWrapper spriteBatch)
        {

            if (!equipped)
            {
                spriteBatch.Draw(itemTexture, drawLocation, Color.White);
            }

            else
            {
                shotDir = currentLevel.mainChar.stats.lastShotDirection;
                moveDir = currentLevel.mainChar.stats.lastMoveDirection;
                shooting = currentLevel.mainChar.shooting;

                if ((shotDir == 0 && shooting) || (moveDir == 0 && !shooting)) /* Sprite walks right*/
                {
                    // draw on the player at their position. 
                    drawLocation.X = currentLevel.mainChar.worldCenter.X - appliedTextureR.frameWidth / 2;
                    drawLocation.Y = currentLevel.mainChar.worldCenter.Y - appliedTextureR.frameHeight / 2;

                    //spriteBatch.Draw(appliedTextureR, drawLocation, Color.White);
                    appliedTextureR.Draw(spriteBatch, drawLocation);
                }
                else if ((shotDir == 2 && shooting) || (moveDir == 2 && !shooting)) /* Sprite walks left*/
                {
                    drawLocation.X = currentLevel.mainChar.worldCenter.X - appliedTextureL.frameWidth / 2;
                    drawLocation.Y = currentLevel.mainChar.worldCenter.Y - appliedTextureL.frameHeight / 2;

                    appliedTextureL.Draw(spriteBatch, drawLocation);
                }
                else if ((shotDir == 3 && shooting) || (moveDir == 3 && !shooting)) /*Sprite walks up*/
                {
                    drawLocation.X = currentLevel.mainChar.worldCenter.X - appliedTextureU.frameWidth / 2;
                    drawLocation.Y = currentLevel.mainChar.worldCenter.Y - appliedTextureU.frameHeight / 2;

                    appliedTextureU.Draw(spriteBatch, drawLocation);
                }
                else /*Sprite walks down or is still (and facing down)*/
                {
                    drawLocation.X = currentLevel.mainChar.worldCenter.X - appliedTextureD.frameWidth / 2;
                    drawLocation.Y = currentLevel.mainChar.worldCenter.Y - appliedTextureD.frameHeight / 2;

                    appliedTextureD.Draw(spriteBatch, drawLocation);
                }
            }
        }

        public void DrawDirection(SpriteBatchWrapper spriteBatch, int direction)
        {

            switch (direction)
            {
                case 0:
                    drawLocation.X = currentLevel.mainChar.drawLocation.X + 50 - appliedTextureD.frameWidth / 2;
                    drawLocation.Y = currentLevel.mainChar.drawLocation.Y + 50 - appliedTextureD.frameHeight / 2;

                    appliedTextureD.DrawFrame(spriteBatch, drawLocation, 0);
                    break;

                case 1:
                    drawLocation.X = currentLevel.mainChar.drawLocation.X + 50 - appliedTextureL.frameWidth / 2;
                    drawLocation.Y = currentLevel.mainChar.drawLocation.Y + 50 - appliedTextureL.frameHeight / 2;

                    appliedTextureL.DrawFrame(spriteBatch, drawLocation, 0);
                    break;

                case 2:
                    drawLocation.X = currentLevel.mainChar.drawLocation.X + 50 - appliedTextureU.frameWidth / 2;
                    drawLocation.Y = currentLevel.mainChar.drawLocation.Y + 50 - appliedTextureU.frameHeight / 2;

                    appliedTextureU.DrawFrame(spriteBatch, drawLocation, 0);
                    break;

                case 3:
                    drawLocation.X = currentLevel.mainChar.drawLocation.X + 50 - appliedTextureR.frameWidth / 2;
                    drawLocation.Y = currentLevel.mainChar.drawLocation.Y + 50 - appliedTextureR.frameHeight / 2;

                    appliedTextureR.DrawFrame(spriteBatch, drawLocation, 0);
                    break;
            }

        }

        public override bool IsObstacle()
        {
            return false;
        }

        public override void TriggerPlayerInteraction()
        {
            applyStatMods(currentLevel.mainChar);

            equipped = true;
            RemoveFromATiles();
            currentLevel.removeGameObject(this);

            if (myAppliedTextureFileL == null &&
                myAppliedTextureFileR == null &&
                myAppliedTextureFileU == null &&
                myAppliedTextureFileD == null &&
                !updates)
            {
                UnloadContent();
            }

            // if there are graphical mods to apply, do them.
            else
            {
                currentLevel.mainChar.equipLoot(this);
                //applyGraphicalMods(gameModel.mainChar);
            }

            if (updates)
            {
                currentLevel.mainChar.addUpdatingLoot(this);
            }

            currentLevel.gameModel.gameView.Toast(itemName, itemDescription);
            RemoveFromATiles();
        }

        public int CompareTo(ALoot toMe)
        {
            return drawOrder - toMe.drawOrder;
        }

        /// <summary>
        /// Applies stat changes to the player as necessary.
        /// </summary>
        /// <param name="toMe">The player to apply stat changes to. This should probably not be blank.</param>
        abstract public void applyStatMods(MainCharacter toMe);


        abstract public void setTextures();

        /// <summary>
        /// This method is called for loot that needs to be updated (asserts the "updates" boolean (which is false by default))
        /// 
        /// </summary>
        virtual public void lootUpdate(GameTime gameTime, MainCharacter toMe)
        {
            // by default, this does nothing.
        }
    }
}
