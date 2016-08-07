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
    public class WallTile : ATile
    {

        public WallTile(Level level, int startX, int startY, int coordI, int coordJ)
            : base(level, startX, startY, coordI, coordJ)
        {
            tileType = TileType.Wall;
            LoadContent();
        }

        public override void SetTexture()
        {
            myTextureFileName = "Textures/wallGrayX";
        }

        override public void UnloadContent()
        {
            //throw new NotImplementedException();
        }

        override public bool IsObstacle()
        {
            return true;
        }

        public override bool CastsShadow()
        {
            return true;
        }

        public override string ToString()
        {
            //For testing, feel free to delete.
            return "Wall";
        }

    }
}
