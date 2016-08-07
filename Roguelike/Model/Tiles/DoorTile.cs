#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Model.GameObjects.Interactables;
using Roguelike.View;
using Roguelike.Model.Infrastructure;
#endregion

namespace Roguelike.Model
{
    public class DoorTile : ATile
    {
        public DoorInteractable doorObject;

        public DoorTile(Level level, int centerX, int centerY, int topLeftI, int topLeftJ) : base(level, centerX, centerY, topLeftI, topLeftJ)
        {
            tileType = TileType.Door;
            LoadContent();
        }

        public override void SetTexture()
        {
            myTextureFileName = "Textures/Floor/darkGrayFloor";
        }

        override public void UnloadContent()
        {
            //throw new NotImplementedException();
        }

        override public void Draw(SpriteBatchWrapper spriteBatch)
        {
            DrawTile(spriteBatch, worldCenter);
            //doorObject.Draw(spriteBatch);
        }

        override public bool IsObstacle()
        {
            return doorObject.IsObstacle();
        }

        public override string ToString()
        {
            //For testing, feel free to delete.
            return "Door";
        }

    }
}
