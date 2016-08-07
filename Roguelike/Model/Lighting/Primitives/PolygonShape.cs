using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Roguelike.Model.GameObjects;
using Roguelike.View;
using Roguelike.Model.Infrastructure;

namespace Roguelike.Model.Lighting.Shape
{
    public class PolygonShape : GameObject
    {
        GraphicsDevice graphicsDevice;
        VertexPositionColor[] absoluteVertices;
        VertexPositionColor[] vertices;
        bool triangulated;
        bool isStatic;

        VertexPositionColor[] triangulatedVertices;
        int[] indexes;

        Effect myEffect;

        Vector3 centerPoint;

        /// <summary>
        /// constructor for polygonshape
        /// </summary>
        /// <param name="modelIn">the model. we do this everywhere. you should know by now</param>
        /// <param name="effect">Use model.be, unless you want to define your own effect</param>
        /// <param name="vertices">list of points</param>
        /// <param name="isStaticIn">boolean if the shape is static or not. (static: on screen; dynamic: on map{</param>
        public PolygonShape(Level levelIn, Effect effect, List<Vector2> vertices, bool isStaticIn)
        {
            //layerType = LayerType.Lighting;
            this.isStatic = isStaticIn;
            this.currentLevel = levelIn;
            this.triangulated = false;
            this.myEffect = effect;
            this.graphicsDevice = effect.GraphicsDevice;

            if (isStatic)
            {
                constructStatic(vertices);
            }

            else
            {
                constructRelative(vertices);
            }
        }

        public override void SetTexture()
        {
            ;
        }

        private void constructRelative(List<Vector2> vertices)
        {
            VertexPositionColor[] newVerts = new VertexPositionColor[vertices.Count];
            VertexPositionColor[] absolute = new VertexPositionColor[vertices.Count];

            for (int i = 0; i < vertices.Count; i++)
            {
                absolute[i] = new VertexPositionColor();
                //intended[i].Position = new Vector3(vertices[i].X, vertices[i].Y, 0);
                absolute[i].Position = new Vector3(
                    vertices[i].X + (currentLevel.mainChar.worldCenter.X),
                    vertices[i].Y + (currentLevel.mainChar.worldCenter.Y), 
                    0);
                absolute[i].Color = Color.Black;

                newVerts[i] = new VertexPositionColor();
                newVerts[i].Position = new Vector3(
                    graphicsDevice.Viewport.Bounds.Width / 2 + vertices[i].X, 
                    graphicsDevice.Viewport.Bounds.Height / 2 + vertices[i].Y,  
                    0);
                newVerts[i].Color = Color.Black;
            }

            this.absoluteVertices = absolute;
            this.vertices = newVerts;

            triangulatedVertices = new VertexPositionColor[newVerts.Length * 3];
            indexes = new int[newVerts.Length];
        }

        private void constructStatic(List<Vector2> vertices)
        {
            VertexPositionColor[] newVerts = new VertexPositionColor[vertices.Count];

            for (int i = 0; i < vertices.Count; i++)
            {
                newVerts[i] = new VertexPositionColor();
                newVerts[i].Position = new Vector3(vertices[i].X, vertices[i].Y, 0);
                newVerts[i].Color = Color.Black;
            }

            this.vertices = newVerts;

            triangulatedVertices = new VertexPositionColor[newVerts.Length * 3];
            indexes = new int[newVerts.Length];
        }

        /// <summary>
        /// A Polygon object that you will be able to draw.
        /// Animations are being implemented as we speak.
        /// </summary>
        /// <param name="graphicsDevice">The graphicsdevice from a Game object</param>
        /// <param name="vertices">The vertices in a clockwise order</param>
        public PolygonShape(Effect effect, VertexPositionColor[] vertices)
        {
            this.vertices = vertices;
            this.triangulated = false;
            this.myEffect = effect;
            this.graphicsDevice = effect.GraphicsDevice;

            triangulatedVertices = new VertexPositionColor[vertices.Length * 3];
            indexes = new int[vertices.Length];
        }

        /// <summary>
        /// Triangulate the set of VertexPositionColors so it will be drawn correcrly        
        /// </summary>
        /// <returns>The triangulated vertices array</returns>}
        public VertexPositionColor[] Triangulate()
        {
            calculateCenterPoint();
            setupIndexes();
            for (int i = 0; i < indexes.Length; i++)
            {
                setupDrawableTriangle(indexes[i]);
            }

            triangulated = true;
            return triangulatedVertices;
        }

        /// <summary>
        /// Calculate the center point needed for triangulation.
        /// The polygon will be irregular, so this isn't the actual center of the polygon
        /// but it will do for now, as we only need an extra point to make the triangles with</summary>
        private void calculateCenterPoint()
        {
            float xCount = 0, yCount = 0;

            foreach (VertexPositionColor vertice in vertices)
            {
                xCount += vertice.Position.X;
                yCount += vertice.Position.Y;
            }

            centerPoint = new Vector3(xCount / vertices.Length, yCount / vertices.Length, 0);
        }

        private void setupIndexes()
        {
            for (int i = 1; i < triangulatedVertices.Length; i = i + 3)
            {
                indexes[i / 3] = i - 1;
            }
        }

        private void setupDrawableTriangle(int index)
        {
            triangulatedVertices[index] = vertices[index / 3]; //No DividedByZeroException?...
            if (index / 3 != vertices.Length - 1)
                triangulatedVertices[index + 1] = vertices[(index / 3) + 1];
            else
                triangulatedVertices[index + 1] = vertices[0];
            triangulatedVertices[index + 2].Position = centerPoint;
        }

        /// <summary>
        /// Draw the polygon. If you haven't called Triangulate yet, I wil do it for you.
        /// </summary>
        /// <param name="effect">The BasicEffect needed for drawing</param>
        public override void Draw(SpriteBatchWrapper batch)
        {
            try
            {
                if (!triangulated)
                    Triangulate();

                batch.End();

                myEffect.CurrentTechnique.Passes[0].Apply();
                graphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                    PrimitiveType.TriangleList, triangulatedVertices, 0, vertices.Length);

                batch.Begin();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public override void Update(GameTime gameTime)
        {
            // only update if the shadow is connected to the world
            if (!isStatic)
            {
                //Console.WriteLine("updating");
                for (int i = 0; i < vertices.GetLength(0); i++)
                {
                    vertices[i].Position.X = absoluteVertices[i].Position.X - (currentLevel.mainChar.worldCenter.X - (graphicsDevice.Viewport.Bounds.Width / 2));
                    vertices[i].Position.Y = absoluteVertices[i].Position.Y - (currentLevel.mainChar.worldCenter.Y - (graphicsDevice.Viewport.Bounds.Height / 2));
                    triangulated = false;
                }
            }
        }

        public override void LoadContent()
        {
        }

        public override void UnloadContent()
        {
        }

        public override string ToString()
        {
            string verts = "rel:";

            for (int i = 0; i < this.vertices.GetLength(0); i++) {
                verts = verts + "<" + vertices[i].Position.X + "," + vertices[i].Position.Y + ">";
            }

            if (absoluteVertices != null)
            {
                verts += "; abs:";

                for (int i = 0; i < this.absoluteVertices.GetLength(0); i++)
                {
                    verts = verts + "<" + absoluteVertices[i].Position.X + "," + absoluteVertices[i].Position.Y + ">";
                }
            }


            return verts;
        }
    }
}

