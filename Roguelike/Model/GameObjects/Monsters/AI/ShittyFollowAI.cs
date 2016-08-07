using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Roguelike.Model.Infrastructure;
using Roguelike.Util;

namespace Roguelike.Model.GameObjects.Monsters.AI
{
    public class ShittyFollowAI : AMonsterAI
    {
        private int speed = 3;

        public ShittyFollowAI(AMonster monster)
            : base(monster)
        {

        }

        public override Vector2 computeMovementVector(double timeMillis)
        {
            Level currentLevel = myMonster.currentLevel;

            float distance = Vector2.Distance(new Vector2(myMonster.worldCenter.X, myMonster.worldCenter.Y), new Vector2(currentLevel.mainChar.worldCenter.X, currentLevel.mainChar.worldCenter.Y));
            Vector2 direction = new Vector2();

            if (distance < 550 && distance > 40)
            {
                if (myMonster.worldCenter.X < currentLevel.mainChar.worldCenter.X)
                {
                    direction.X = speed;
                }
                else if (myMonster.worldCenter.X > currentLevel.mainChar.worldCenter.X)
                {
                    direction.X = -1 * speed;
                }
                else
                {
                    direction.X = 0;
                }

                if (myMonster.worldCenter.Y < currentLevel.mainChar.worldCenter.Y)
                {
                    direction.Y = speed;
                }
                else if (myMonster.worldCenter.Y > currentLevel.mainChar.worldCenter.Y)
                {
                    direction.Y = -1 * speed;
                }
                else
                {
                    direction.Y = 0;
                }
            }
            else
            {
                direction.X = 0;
                direction.Y = 0;
            }

            return MathHelperHelper.Vector2Normalize(direction);
        }
    }
}
