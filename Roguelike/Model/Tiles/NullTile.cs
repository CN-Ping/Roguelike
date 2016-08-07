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
    public class NullTile : ATile
    {

        public NullTile(Level level, int startX, int startY, int coordI, int coordJ) : base(level, startX, startY, coordI, coordJ)
        {
            tileType = TileType.Null;
            
        }

        public override void SetTexture()
        {
            
        }

        override public void UnloadContent()
        {
            //throw new NotImplementedException();
        }

        override public bool IsObstacle()
        {
            return true;
        }

        public override string ToString()
        {
            //For testing, feel free to delete.
            return "Null";
        }

        public override void Draw(View.SpriteBatchWrapper spriteBatch)
        {
            // does not draw
        }
    }
}
