using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Model.Infrastructure;


namespace Roguelike.View
{
    class Background
    {
        Texture2D background;
        Level currentLevel;
        Roguelike Game;
        private int height;
        private int width;
        private int viewportHeight;
        private int viewportWidth;

        public Background(Level level, Roguelike inGame)
        {
            currentLevel = level;
            Game = inGame;
            viewportHeight = Game.GraphicsDevice.Viewport.Height;
            viewportWidth = Game.GraphicsDevice.Viewport.Width;
        }

        public void LoadContent()
        {
            Roguelike Game = currentLevel.gameModel.Game;
            background = Game.Content.Load<Texture2D>("Textures/starField");
            height = background.Height;
            width = background.Width;
        }

        public void Draw(SpriteBatchWrapper spriteBatch)
        {
            int scrollX = (1 * currentLevel.gamePosX) - (viewportWidth / 2);
            int scrollY = (1 * currentLevel.gamePosY) - (viewportHeight / 2);
            //Game.GraphicsDevice.SamplerStates[0].AddressU = TextureAddressMode.Wrap;
            //Game.GraphicsDevice.SamplerStates[0].AddressV = TextureAddressMode.Wrap;
            Rectangle source = new Rectangle(0, 0, width, height);
            Rectangle destination = new Rectangle(scrollX, scrollY, width, height);
            spriteBatch.Draw(background, destination, source, Color.White);
        }

        public void Reinitialize(Level newLevel)
        {
            currentLevel = newLevel;
        }
    }
}
