using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roguelike.Model;
using Roguelike.Model.GameObjects;
using Microsoft.Xna.Framework;
using Roguelike.Model.Infrastructure;

namespace Roguelike.util
{
    public class EvenWorseThanBug
    {
        Level currentLevel;
        public EvenWorseThanBug(Level levelIn)
        {
            currentLevel = levelIn;
        }

        public Vector2 GetChangeInPos(GameObject toChange, int speed)
        {
            //TODO this function
            Vector2 toReturn = new Vector2();
            int run = currentLevel.gamePosX - (int)toChange.worldCenter.X;
            int rise = currentLevel.gamePosY - (int)toChange.worldCenter.Y;
            return toReturn;
        }
    }
}
