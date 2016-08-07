using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Roguelike.Model.Infrastructure;
using Roguelike.Util;

namespace Roguelike.Model.GameObjects.Monsters.AI
{
    public class FlockingAI : AMonsterAI
    {
        private int speed = 3;
        
        private static float A_STAR_RANGE = 600f;
        private static float FLOCKING_RANGE = 800f;

        private static float ALIGNMENT_0_TO_1 = 0.0f;
        private static float COHESION_0_TO_1 = 0.25f;
        private static float SEPARATION_0_TO_1 = 0.2f;
        private static float A_STAR_0_TO_1 = 0.10f;
        private static float RANDOM_0_TO_1 = 0.15f;

        private static double lastUpdate = 0;
        private static double timer = 300;

        private AStarFollowAI aStarAI;

        protected static HashSet<AMonster> flockingMonsters = new HashSet<AMonster>();
        protected HashSet<AMonster> flockingMonstersInRange = new HashSet<AMonster>();

        Vector2 velocity = new Vector2();

        static Random rng = new Random();

        public FlockingAI(AMonster monster) : base(monster)
        {
            flockingMonsters.Add(monster);

            aStarAI = new AStarFollowAI(monster);
        }

        public override Vector2 computeMovementVector(double timeMillis)
        {
            
            if (timeMillis - lastUpdate > timer)
            {
                float backup = SEPARATION_0_TO_1;

                SEPARATION_0_TO_1 = COHESION_0_TO_1;
                COHESION_0_TO_1 = backup;

                lastUpdate = timeMillis;
            }
            

            Level currentLevel = myMonster.currentLevel;

            velocity = UpdateNeighborhood(timeMillis, currentLevel);

            return velocity;
        }

        /// <summary>
        /// Updates the flockingMonstersInRange field from the world
        /// </summary>
        private Vector2 UpdateNeighborhood(double timeMillis, Level level)
        {
            int distanceToPlayer = (int)Vector2.Distance(new Vector2(myMonster.worldCenter.X, myMonster.worldCenter.Y), new Vector2(level.mainChar.worldCenter.X, level.mainChar.worldCenter.Y));

            Vector2 aStarVector = new Vector2();
            
            if (distanceToPlayer < A_STAR_RANGE) {
                aStarVector = aStarAI.computeMovementVector(timeMillis);
            }

            flockingMonstersInRange.Clear();
            Vector2 v = new Vector2();
            Vector2 alignment = new Vector2();

            Vector2 p = new Vector2();
            Vector2 cohesion = new Vector2();

            Vector2 s = new Vector2();
            Vector2 separation = new Vector2();

            Vector2 randomDirection = new Vector2((float)(rng.NextDouble() - 0.5), (float)(rng.NextDouble() - 0.5));

            // this overhead may get ridiculous. Can optimize with hash sets and some simple checking later
            foreach (AMonster monster in flockingMonsters) {
                if (monster != myMonster)
                {
                    if (Vector2.Distance(myMonster.worldCenter, monster.worldCenter) < FLOCKING_RANGE)
                    {
                        flockingMonstersInRange.Add(monster);

                        v += monster.velocity;

                        p.X += monster.worldCenter.X;
                        p.Y += monster.worldCenter.Y;

                        s.X += (monster.worldCenter.X - myMonster.worldCenter.X);
                        s.Y += (monster.worldCenter.Y - myMonster.worldCenter.Y);
                    }
                }
            }

            // if there are no suitable neighbors, then return, since this will divide by 0.
            //if (flockingMonstersInRange.Count == 0)
            //{
            //    return Vector2.Zero;
            //}
            if (flockingMonstersInRange.Count > 0)
            {
                v /= Math.Max(flockingMonstersInRange.Count, 1);
                p /= Math.Max(flockingMonstersInRange.Count, 1);
                s /= Math.Max(flockingMonstersInRange.Count, 1);

                p -= myMonster.worldCenter;
                s *= -1;

                alignment = MathHelperHelper.Vector2Normalize(v);
                cohesion = MathHelperHelper.Vector2Normalize(p);
                separation = MathHelperHelper.Vector2Normalize(s);
            }

            Vector2 heading = (alignment * ALIGNMENT_0_TO_1) + (cohesion * COHESION_0_TO_1) + (separation * SEPARATION_0_TO_1) + (aStarVector * A_STAR_0_TO_1) + (randomDirection * RANDOM_0_TO_1);
            
            return MathHelperHelper.Vector2Normalize(heading) * speed;
        }

        public override void MonsterDied(AMonster ded)
        {
            flockingMonsters.Remove(ded);
        }
    }
}
