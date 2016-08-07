using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Roguelike.View;

namespace Shadows2D
{
    public enum LightAreaQuality
    {
        VeryLow,
        Low,
        Middle,
        High,
        VeryHigh
    }

    public class LightSource
    {
        private GraphicsDeviceManager graphics;

        public RenderTarget2D PrintedLight;
        public Vector2 Position { get; set; }
        public Vector2 RenderTargetSize { get; set; }
        public Vector2 Size { get; set; }
        private float qualityRatio;
        public Color Color;

        public int Radius;
        public float RenderRadius;

        public Vector2 PrintPosition
        {
            get { return this.Position - new Vector2(this.Radius, this.Radius); }
        }

        public LightSource(GraphicsDeviceManager graphics, int radius, LightAreaQuality quality, Color color)
        {
            switch (quality)
            {
                case LightAreaQuality.VeryLow:
                    this.qualityRatio = 0.1f;
                    break;
                case LightAreaQuality.Low:
                    this.qualityRatio = 0.25f;
                    break;
                case LightAreaQuality.Middle:
                    this.qualityRatio = 0.5f;
                    break;
                case LightAreaQuality.High:
                    this.qualityRatio = 0.75f;
                    break;
                case LightAreaQuality.VeryHigh:
                    this.qualityRatio = 1f;
                    break;
            }
            this.graphics = graphics;
            this.Radius = radius;
            this.RenderRadius = (float)radius * this.qualityRatio;
            float baseSize = (float)this.Radius * 2f;
            this.Size = new Vector2(baseSize);
            baseSize *= this.qualityRatio;
            this.RenderTargetSize = new Vector2(baseSize);
            PrintedLight = new RenderTarget2D(graphics.GraphicsDevice, (int)baseSize, (int)baseSize);
            this.Color = color;
        }

        public Vector2 ToRelativePosition(Vector2 worldPosition)
        {
            return worldPosition - (Position - RenderTargetSize * 0.5f);
        }

        public Vector2 RelativeZero
        {
            get
            {
                return new Vector2(Position.X - this.Radius, Position.Y - this.Radius);
            }
        }

        public Vector2 RelativeZeroHLSL(ShadowCasterMap shadowMap)
        {
            Vector2 sizedRelativeZero = this.RelativeZero * shadowMap.PrecisionRatio;
            float shadowmapRelativeZeroX = sizedRelativeZero.X / shadowMap.Size.X;
            shadowmapRelativeZeroX -= (shadowmapRelativeZeroX % shadowMap.PixelSizeHLSL.X) * shadowMap.PrecisionRatio;
            float shadowmapRelativeZeroY = sizedRelativeZero.Y / shadowMap.Size.Y;
            shadowmapRelativeZeroY -= (shadowmapRelativeZeroY % shadowMap.PixelSizeHLSL.Y) * shadowMap.PrecisionRatio;
            return new Vector2(shadowmapRelativeZeroX, shadowmapRelativeZeroY);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int size = (int)(this.Radius * 2f);
            //graphics.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.Stencil, Color.Transparent, 0, 0);

            //var m = Matrix.CreateOrthographicOffCenter(0,
            //    graphics.GraphicsDevice.PresentationParameters.BackBufferWidth,
            //    graphics.GraphicsDevice.PresentationParameters.BackBufferHeight,
            //    0, 0, 1
            //);

            //var a = new AlphaTestEffect(graphics.GraphicsDevice)
            //{
            //    Projection = m
            //};

            //var s1 = new DepthStencilState
            //{
            //    StencilEnable = true,
            //    StencilFunction = CompareFunction.Always,
            //    StencilPass = StencilOperation.Replace,
            //    ReferenceStencil = 1,
            //    DepthBufferEnable = false,
            //};

            //var s2 = new DepthStencilState
            //{
            //    StencilEnable = true,
            //    StencilFunction = CompareFunction.NotEqual,
            //    StencilPass = StencilOperation.Keep,
            //    ReferenceStencil = 1,
            //    DepthBufferEnable = false,
            //};



            //spriteBatch.Begin(SpriteSortMode.Immediate, null, null, s1, null, a);
            ////spriteBatch.Draw(lightTexture, new Vector2(700,  200), Color.White*0.3f); //The mask
            //float rotation = direction * 0.5f * Pi;
            //spriteBatch.Draw(lightTexture, new Vector2(xOffset, yOffset), null, Color.White, rotation, origin, 1.0f, SpriteEffects.None, 0f);
            ////spriteBatch.Draw(lightTexture, location, null, Color.White * 0.5f, 0f, origin, 1.0f, SpriteEffects.None, 0f);
            //spriteBatch.Draw(dotTexture, new Vector2(xOffset, yOffset), Color.White);

            //spriteBatch.End();


            //Vector2 location2 = new Vector2(playerX - 800, playerY - 450);


            //spriteBatch.Begin(SpriteSortMode.Immediate, null, null, s2, null, a);
            //spriteBatch.Draw(shadowTexture, Vector2.Zero, null, Color.Black, 0f, origin, 1.0f, SpriteEffects.None, 0f); //The background
            ////spriteBatch.Draw(shadowTexture, location2, Color.White * 0.7f); 
            //Game Game = V
            //Texture2D mask = Game.Content.Load<Texture2D>("Sprites/mainCharWUFU");
            //graphics.GraphicsDevice.SetRenderTarget(this.PrintedLight);
            //spriteBatch.Draw()
            spriteBatch.Draw(this.PrintedLight, new Rectangle((int)this.PrintPosition.X, (int)this.PrintPosition.Y, size, size), this.Color);
        }

        public void DrawTransparent(SpriteBatch spriteBatch)
        {
            int size = (int)(this.Radius * 2f);
            spriteBatch.Draw(this.PrintedLight, new Rectangle((int)this.PrintPosition.X, (int)this.PrintPosition.Y, size, size), Color.Transparent);
        }

        public void Draw(SpriteBatch spriteBatch, byte opacity)
        {
            Color colorA = this.Color;
            colorA.A = opacity;
            int size = (int)(this.Radius * 2f);
            spriteBatch.Draw(this.PrintedLight, new Rectangle((int)this.PrintPosition.X, (int)this.PrintPosition.Y, size, size), colorA);
        }

        public void Draw(SpriteBatch spriteBatch, Color color, byte opacity)
        {
            color.A = opacity;
            int size = (int)(this.Radius * 2f);
            spriteBatch.Draw(this.PrintedLight, new Rectangle((int)this.PrintPosition.X, (int)this.PrintPosition.Y, size, size), color);
        }

        public void Draw(SpriteBatchWrapper spriteBatch)
        {
            int size = (int)(this.Radius * 2f);
            spriteBatch.Draw(this.PrintedLight, new Rectangle((int)this.PrintPosition.X, (int)this.PrintPosition.Y, size, size), this.Color);
        }

        public void Draw(SpriteBatchWrapper spriteBatch, byte opacity)
        {
            Color colorA = this.Color;
            colorA.A = opacity;
            int size = (int)(this.Radius * 2f);
            spriteBatch.Draw(this.PrintedLight, new Rectangle((int)this.PrintPosition.X, (int)this.PrintPosition.Y, size, size), colorA);
        }

        public void Draw(SpriteBatchWrapper spriteBatch, Color color, byte opacity)
        {
            color.A = opacity;
            int size = (int)(this.Radius * 2f);
            spriteBatch.Draw(this.PrintedLight, new Rectangle((int)this.PrintPosition.X, (int)this.PrintPosition.Y, size, size), color);
        }

        public void SetRadius(int radius)
        {
            this.Radius = radius;
            this.RenderRadius = (float)radius * this.qualityRatio;
            float baseSize = (float)this.Radius * 2f;
            this.Size = new Vector2(baseSize);
            baseSize *= this.qualityRatio;
            this.RenderTargetSize = new Vector2(baseSize);
            PrintedLight = new RenderTarget2D(graphics.GraphicsDevice, (int)baseSize, (int)baseSize);
        }
    }
}
