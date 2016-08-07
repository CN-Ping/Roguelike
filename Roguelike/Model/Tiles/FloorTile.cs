#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Model.Infrastructure;
#endregion

namespace Roguelike.Model
{
    public class FloorTile : ATile
    {

        public FloorTile(Level level, int startX, int startY, int coordI, int coordJ) : base(level, startX, startY, coordI, coordJ)
        {
            tileType = TileType.Floor;
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

        override public bool IsObstacle()
        {
            return false;
        }

        public override string ToString()
        {
            //For testing, feel free to delete.
            return "Floor";
        }
    }
}
