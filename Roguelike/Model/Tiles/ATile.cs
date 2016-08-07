#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Model.GameObjects;
using System.Collections;
using Roguelike.View;
//using Shadows2D;
using Roguelike.Model.Infrastructure;
using Roguelike.Model.GameObjects.Monsters.AI.Pathfinding.AStar;
#endregion

namespace Roguelike.Model
{
    public enum TileType
    {
        Floor,
        Door,
        Wall,
        Corner, 
        Null
    }

    abstract public class ATile : GameObject, IPathNode<Object> // here Object is unused
    {
        public HashSet<GameObject> overlappingGameObjects;
        public int lightLevel = 0;

        public TileType tileType;

        public Tuple<int, int> atileCoords;

        public ATile(Level level, int centerX, int centerY, int coordI, int coordJ) : base(level, centerX, centerY)
        {
            overlappingGameObjects = new HashSet<GameObject>();
            layerType = LayerType.World;
            SetBoundingPointsOffset();

            atileCoords = new Tuple<int, int>(coordI, coordJ);
        }

        override public void Update(GameTime gameTime)
        {
            /* Tiles don't need to use the Update function right now, 
             * so I just have it doing nothing in this abstract class */
        }

        override public void Draw(SpriteBatchWrapper spriteBatch)
        {
            DrawTile(spriteBatch, worldCenter);
        }

        public void DrawTile(SpriteBatchWrapper spriteBatch, Vector2 location)
        {
            Vector2 middle = new Vector2(textureWidth / 2, textureHeight / 2);
            //Vector2 scale = new Vector2(1, 1);
            //Rectangle sourceRectangle = new Rectangle(0, 0, textureWidth, textureHeight);
            /*
             * To change the rotation, do something like base.textureRotation + Mathhelper.Pi if you want to flip it upside down
             */
            spriteBatch.Draw(texture2D, location, null, Color.White, base.textureRotation, middle, 1.0f, SpriteEffects.None, 1);
        }

        //public override void DrawCaster(ShadowCasterMap shadowMap)
        //{
        //   // Vector2 location = SpriteBatchWrapper.transformVector(base.xPos - textureWidth / 2, base.yPos - textureHeight / 2);
        //    //CLEANUP
        //    //Vector2 location = new Vector2(base.xPos - textureWidth / 2, base.yPos - textureHeight / 2);
        //    shadowMap.AddShadowCaster(texture2D, drawLocation, textureWidth, textureHeight);
        //}

        //abstract public void DrawTile(SpriteBatch spriteBatch, Vector2 location);

        /// <summary>
        /// Adds a reference to the GameObject to this ATile
        /// </summary>
        /// <param name="overlappingObject"> the GameObject that overlaps this ATile. </param>
        public void addGameObjectReference(GameObject overlappingObject)
        {
//            if (overlappingGameObjects.Contains(overlappingObject) == false)
//            {
//                overlappingGameObjects.Add(overlappingObject);
//            }

            overlappingGameObjects.Add(overlappingObject);
        }

        /// <summary>
        /// Removes the reference to the GameObject
        /// </summary>
        /// <param name="overlappingObject">The GameObject that is nolonger overlapping this ATile. </param>
        public void removeGameObjectReference(GameObject overlappingObject)
        {
            if (overlappingGameObjects.Contains(overlappingObject) == true)
            {
                overlappingGameObjects.Remove(overlappingObject);
            }
        }

        override public void SetBoundingPointsOffset()
        {
            int tilewidth = currentLevel.theWorld.tileWidth;
            int tileheight = currentLevel.theWorld.tileHeight;

            SetBoundingPointsOffset(tilewidth, tileheight);
        }


        public Boolean IsWalkable(Object inContext) // unused
        {
            return !IsObstacle();
        }
    }
}
