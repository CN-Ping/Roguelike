#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shadows2D;
#endregion

namespace Roguelike.View
{
    public class AnimatedSprite
    {
        public Texture2D Texture { get; set; }

        public int Rows { get; set; }

        public int Columns { get; set; }

        int row;
        int column;

        int row2;
        int col2;

        private int currentFrame;
        private int totalFrames;

        public int frameWidth;
        public int frameHeight;

        private bool forwards = true;

        Rectangle source = new Rectangle();
        Rectangle customSource = new Rectangle();

        Rectangle destination = new Rectangle();

        //Vector2 transformedLocation = new Vector2();
        //Rectangle transformedRectangle = new Rectangle();

        //TODO framesPerSecond may need to be something specified at initialization so enemies can have different framesPerSecond
        public int framesPerSecond = 15;
        private double lastFrameUpdate = 0;

        public AnimatedSprite(Texture2D texture, int rows, int columns)
        {
            Initialize(texture, rows, columns);
        }

        public AnimatedSprite(Texture2D texture, int rows, int columns, bool inForwards)
        {
            Initialize(texture, rows, columns);
            forwards = inForwards;
        }

        public void Initialize(Texture2D texture, int rows, int columns)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            totalFrames = Rows * Columns;

            frameWidth = Texture.Width / Columns;
            frameHeight = Texture.Height / Rows;
            destination.Width = frameWidth;
            destination.Height = frameHeight;

            source.Width = frameWidth;
            source.Height = frameHeight;

            customSource.Width = frameWidth;
            customSource.Height = frameHeight;
        }

        public void Update(GameTime gameTime)
        {
            lastFrameUpdate += gameTime.ElapsedGameTime.TotalSeconds;
            if (lastFrameUpdate * framesPerSecond > 1)
            {
                if (forwards)
                {
                    currentFrame++;
                    if (currentFrame == totalFrames)
                    {
                        currentFrame = 0;
                    }
                }
                else
                {
                    currentFrame--;
                    if (currentFrame == -1)
                    {
                        currentFrame = totalFrames - 1;
                    }
                }

                row = (int)((float)currentFrame / (float)Columns);
                column = currentFrame % Columns;

                source.X = frameWidth * column;
                source.Y = frameHeight * row;
                source.Width = frameWidth;
                source.Height = frameHeight;

                lastFrameUpdate = 0;
            }
        }

        public void Draw(SpriteBatchWrapper spriteBatch, Vector2 location)
        {
            destination.X = (int)location.X;
            destination.Y = (int)location.Y;

            spriteBatch.Draw(Texture, destination, source, Color.White);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            destination.X = (int)location.X;
            destination.Y = (int)location.Y;

            spriteBatch.Draw(Texture, destination, source, Color.White);
        }

        public void Draw(SpriteBatchWrapper spriteBatch, Rectangle outputRectangle)
        {
            spriteBatch.Draw(Texture, outputRectangle, source, Color.White);
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle outputRectangle)
        {
            spriteBatch.Draw(Texture, outputRectangle, source, Color.White);
        }

        public void Draw(SpriteBatchWrapper spriteBatch, int x, int y)
        {
            destination.X = x;
            destination.Y = y;

            spriteBatch.Draw(Texture, destination, source, Color.White);
        }

        public void DrawCaster(ShadowCasterMap shadowMap, Vector2 location)
        {
            destination.X = (int)location.X;
            destination.Y = (int)location.Y;

            shadowMap.AddShadowCaster(Texture, destination, source);
        }

        public Rectangle getSourceRectangle()
        {
            return source;
        }

        public void Draw(SpriteBatchWrapper spriteBatch, Vector2 location, int alpha)
        {
            destination.X = (int)location.X;
            destination.Y = (int)location.Y;
            Color alphaColor = Color.White*((float)alpha/255);
            //alphaColor.A = (byte)alpha;
            spriteBatch.Draw(Texture, destination, source, alphaColor);
        }

        public void Draw(SpriteBatchWrapper spriteBatch, Vector2 location, int alpha, bool hit)
        {
            destination.X = (int)location.X;
            destination.Y = (int)location.Y;
            Color alphaColor;
            if (hit)
            {
                alphaColor = Color.Red * ((float)alpha / 255);
            }
            else
            {
                alphaColor = Color.White * ((float)alpha / 255);
            }
            //alphaColor.A = (byte)alpha;
            spriteBatch.Draw(Texture, destination, source, alphaColor);
        }

        public void DrawFrame(SpriteBatchWrapper spriteBatch, Vector2 location, int frame)
        {
            DrawFrameHelper(spriteBatch, location, frame, false);
        }

        public void DrawFrame(SpriteBatchWrapper spriteBatch, Vector2 location, int frame, bool hit)
        {
            DrawFrameHelper(spriteBatch, location, frame, hit);
        }

        private void DrawFrameHelper(SpriteBatchWrapper spriteBatch, Vector2 location, int frame, bool hit)
        {
            destination.X = (int)location.X;
            destination.Y = (int)location.Y;

            row2 = (int)((float)frame / (float)Columns);
            col2 = frame % Columns;

            customSource.X = frameWidth * col2;
            customSource.Y = frameHeight * row2;
            customSource.Width = frameWidth;
            customSource.Height = frameHeight;

            if (hit)
            {
                spriteBatch.Draw(Texture, destination, customSource, Color.Red);
            }
            else
            {
                spriteBatch.Draw(Texture, destination, customSource, Color.White);
            }
        }
    }
}
