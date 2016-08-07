using Roguelike.Model.GameObjects.Interactables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Model.Infrastructure;

namespace Roguelike.Model.GameObjects.Pickups
{
    public abstract class APickup : AInteractable
    {
        public APickup()
        {

        }

        public APickup(Level level, int x, int y) : base(level, x, y)
        {
            SetBoundingPointsOffset();
            LoadContent();
        }
        public void SetData(Level level, int x, int y)
        {
            currentLevel = level;
            worldCenter.X = x;
            worldCenter.Y = y;
            SetBoundingPointsOffset();
            LoadContent();
        }

        public void applyStats(MainCharacter toMe)
        {
            applyStatsMod(toMe);
            RemoveFromATiles();
            currentLevel.removeGameObject(this);
        }

        abstract public void applyStatsMod(MainCharacter toMe);

        public override void SetBoundingPointsOffset()
        {
            // Aloot will always be centered at x, y, and be a square. Change it if you don't like it
            SetBoundingPointsOffset(textureWidth, textureHeight);
        }

        public override bool IsObstacle()
        {
            return false;
        }

    }
}
