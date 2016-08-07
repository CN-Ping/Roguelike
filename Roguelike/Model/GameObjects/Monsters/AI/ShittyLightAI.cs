using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Roguelike.Model.Infrastructure;
using Roguelike.Util;

namespace Roguelike.Model.GameObjects.Monsters.AI
{
    public class ShittyLightAI : AMonsterAI
    {
        private int speed = 3;
        private bool lightBehavior = false;
        private bool AttractLight;
        private bool AvoidLight;
        private ShittyFollowAI followAI;

        /// <summary>
        /// Define light behavior of monster
        /// </summary>
        /// <param name="monster"></param>
        /// <param name="behavior">0 for avoid light, 1 for attract light </param>
        public ShittyLightAI(AMonster monster, int behavior)
            : base(monster)
        {
            setLightBehavior(behavior);
            followAI = new ShittyFollowAI(monster);
        }

        public void setLightBehavior(int behavior)
        {
            if (behavior == 0)
            {
                AvoidLight = true;
                AttractLight = false;
                lightBehavior = true;
            }
            else if (behavior == 1)
            {
                AvoidLight = false;
                AttractLight = true;
                lightBehavior = true;
            }
        }

        public override Vector2 computeMovementVector(double timeMillis)
        {
            Level currentLevel = myMonster.currentLevel;
            Vector2 direction = new Vector2();

            float distance = Vector2.Distance(new Vector2(myMonster.worldCenter.X, myMonster.worldCenter.Y), new Vector2(currentLevel.mainChar.worldCenter.X, currentLevel.mainChar.worldCenter.Y));
            if (lightBehavior == true)
            {
                int lightDir = getClosestLightDirection();
                if (AttractLight)
                {
                    if (lightDir == 0)
                    {
                        direction.X = 0;
                        direction.Y = 0;
                        
                    }
                    else if (lightDir == 1)
                    {
                        //up
                        direction.X = 0;
                        direction.Y = -1 * speed;
                    }
                    else if (lightDir == 2)
                    {
                        //left
                        direction.X = -1 * speed;
                        direction.Y = 0;
                    }
                    else if (lightDir == 3)
                    {
                        //right
                        direction.X = speed;
                        direction.Y = 0;
                    }
                    else if (lightDir == 4)
                    {
                        //down
                        direction.X = 0;
                        direction.Y = speed;
                    }
                }
                else if(AvoidLight)
                {
                    if (distance < 180)
                    {
                        return followAI.computeMovementVector(timeMillis);
                    }
                    if (lightDir == 0)
                    {
                       return followAI.computeMovementVector(timeMillis);  
                    }
                    else if (lightDir == 1)
                    {
                        //up
                        direction.X = 0;
                        direction.Y = 1 * speed;
                    }
                    else if (lightDir == 2)
                    {
                        //left
                        direction.X = 1 * speed;
                        direction.Y = 0;
                    }
                    else if (lightDir == 3)
                    {
                        //right
                        direction.X = -1 * speed;
                        direction.Y = 0;
                    }
                    else if (lightDir == 4)
                    {
                        //down
                        direction.X = 0;
                        direction.Y = -1 * speed;
                    }
                }
            }
            else
            {
                direction.X = 0;
                direction.Y = 0;
            }

            return MathHelperHelper.Vector2Normalize(direction); ;
        }

        private int getClosestLightDirection()
        {
            if (myMonster.shadowLevel.R < 3)
            {
                return 0;
            }
            //else if (myMonster.shadowLevel.R==255){
            //    return 0;
            //}
            else
            {
                Color up = myMonster.getShadowLevelAtRelLoc(new Vector2(0, -20));
                Color left = myMonster.getShadowLevelAtRelLoc(new Vector2(-20, 0));
                Color right = myMonster.getShadowLevelAtRelLoc(new Vector2(20, 0));
                Color down = myMonster.getShadowLevelAtRelLoc(new Vector2(0, 20));
                int levelUp = up.R;
                int levelLeft = left.R;
                int levelRight = right.R;
                int levelDown = down.R;
                int max = Math.Max(levelUp, Math.Max(levelLeft, Math.Max(levelRight, levelDown)));
                if (levelUp == max)
                {
                    return 1;
                }
                else if (levelLeft == max)
                {
                    return 2;
                }
                else if (levelRight == max)
                {
                    return 3;
                }
                else
                {
                    return 4;
                }
            }
        }
    }
}
