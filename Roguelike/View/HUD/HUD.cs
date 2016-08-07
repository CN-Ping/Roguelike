using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Model.Infrastructure;

namespace Roguelike.View
{
    class HUD
    {
        public int HUDheight;
        public int HUDwidth;
        private Level level;

        public HealthBar healthBar;
        public ExpBar expBar;

        private View view;

        public HUD (View viewIn)
        {
            view = viewIn;
            HUDheight = 50;
            HUDwidth = view.viewport.Width;
        }

        public void Initialize(Level levelIn)
        {
            level = levelIn;
            healthBar = new HealthBar(level, 0, 0);
            healthBar.LoadContent();
            expBar = new ExpBar(level, 0, 0);
            expBar.LoadContent();
            TorchCount torchCount = new TorchCount(level, 0, 0);
            torchCount.LoadContent();
            level.addGameObject(expBar);
            level.addGameObject(healthBar);
            level.addGameObject(torchCount);
        }

        public void Reinitialize(Level levelIn)
        {
            level = levelIn;
            healthBar = new HealthBar(level, 0, 0);
            healthBar.LoadContent();
            expBar = new ExpBar(level, 0, 0);
            expBar.LoadContent();
            TorchCount torchCount = new TorchCount(level, 0, 0);
            torchCount.LoadContent();
            level.addGameObject(expBar);
            level.addGameObject(healthBar);
            level.addGameObject(torchCount);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //healthBar.Draw(spriteBatch);
        }
    }
}
