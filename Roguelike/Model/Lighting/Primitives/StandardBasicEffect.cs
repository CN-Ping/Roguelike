using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Roguelike.Model.Lighting.Shape
{
    public class StandardBasicEffect : BasicEffect
    {
        public StandardBasicEffect(GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            this.VertexColorEnabled = false;
            this.Alpha = 0.8f; // this is the shadow-ey-ness-ocity. can be set externally :D
            this.DiffuseColor = new Vector3(0.0f, 0.0f, 0.0f);
            this.Projection = Matrix.CreateOrthographicOffCenter(
                0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height, 0, 0, 1);
        }

        public StandardBasicEffect(BasicEffect effect)
            : base(effect) { }

        //public BasicEffect Clone()
        //{
        //    return new StandardBasicEffect(this);
        //}
    }
}
