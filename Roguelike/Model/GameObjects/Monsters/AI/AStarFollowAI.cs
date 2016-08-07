using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Roguelike.Model.Infrastructure;
using Roguelike.Util;
using Roguelike.Model.GameObjects.Monsters.AI.Pathfinding.AStar;

namespace Roguelike.Model.GameObjects.Monsters.AI
{
    class AStarFollowAI : AMonsterAI
    {
        static SpatialAStar<ATile, Object> aStarSolver;
        static Dictionary<Tuple<ATile, ATile>, Tuple<List<ATile>, double>> pathDictionary = new Dictionary<Tuple<ATile, ATile>, Tuple<List<ATile>, double>>();
        
        Level currentLevel;
        ShittyFollowAI followAI;

        System.Drawing.Point monsterTileCoords;
        System.Drawing.Point playerTileCoords;
        List<ATile> path;

        double lastUpdate = 0;
        double UpdateInterval = 1000;

        double SHITTY_FOLLOW_DISTANCE = 250;
        double A_STAR_RANGE = 800;

        public AStarFollowAI(AMonster monster)
            : base(monster)
        {
            // only do this once, as it will be expensive.
            if (aStarSolver == null || currentLevel != monster.currentLevel)
            {
                aStarSolver = new SpatialAStar<ATile, Object>(monster.currentLevel.theWorld.getATileMatrix());
            }

            currentLevel = monster.currentLevel;
            myMonster = monster;
            followAI = new ShittyFollowAI(monster);
        }
        
        public AStarFollowAI(AMonster monster, double update_interval, double follow_distance, double a_star_range) : this(monster) {
            UpdateInterval = update_interval;
            SHITTY_FOLLOW_DISTANCE = follow_distance;
            A_STAR_RANGE = a_star_range;
        }

        public override Vector2 computeMovementVector(double timeMillis)
        {
            if (timeMillis - UpdateInterval > lastUpdate)
            {
                monsterTileCoords = Level.GetMatrixCoordAt(myMonster.worldCenter);
                playerTileCoords = Level.GetMatrixCoordAt(currentLevel.mainChar.worldCenter);
                //path = astar.AStar(monsterLocation, playerLocation);

                ATile one = currentLevel.GetATileAt(myMonster.worldCenter);
                ATile two = currentLevel.GetATileAt(currentLevel.mainChar.worldCenter);

                if (one != null && two != null)
                {
                    Tuple<ATile, ATile> key = new Tuple<ATile, ATile>(one, two);

                    if (pathDictionary.ContainsKey(key) && (timeMillis - pathDictionary[key].Item2 < 30000))
                    {
                        path = pathDictionary[key].Item1;
                    }

                    else
                    {
                        path = aStarSolver.Search(monsterTileCoords, playerTileCoords, null);

                        pathDictionary[key] = new Tuple<List<ATile>, double>(path, timeMillis);

                        if (path != null)
                        {
                            // add all this crap in for efficiency i hope
                            for (int i = 0; i < path.Count - 1; i++)
                            {
                                for (int j = i + 1; j < path.Count; j++)
                                {
                                    pathDictionary[new Tuple<ATile, ATile>(path[i], path[j])] = new Tuple<List<ATile>, double>(path.GetRange(i, j - i), timeMillis);
                                }
                            }

                        }

                    }


                    lastUpdate = timeMillis;
                }
            }

            float distance = Vector2.Distance(myMonster.worldCenter, currentLevel.mainChar.worldCenter);

            if (distance < SHITTY_FOLLOW_DISTANCE)
            {
               return followAI.computeMovementVector(timeMillis);
            }
            else if (distance < A_STAR_RANGE)
            {
                if (path == null)
                {
                    return Vector2.Zero;
                }
                else
                {
                    return MathHelperHelper.Vector2Normalize(getPathDirection(myMonster.worldCenter, path));
                }
            }
            else
            {
                return Vector2.Zero;
            }
        }

        /// <summary>
        /// Returns a tuple representing signs of speed in the x and y axis
        /// </summary>
        /// <param name="monsterLocation"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private Vector2 getPathDirection(Vector2 monsterLocation, List<ATile> path)
        {

            //Vector2 direction = monsterLocation - path.First().worldCenter;
            if (path.Count() > 0)
            {
                // if we're close enough, delete this
                if ((path.Count() > 1) && (Vector2.Distance(monsterLocation, path.First().worldCenter) <= 20))
                {
                    path.RemoveAt(0);
                }

                return path.First().worldCenter - monsterLocation;
            }

            else if (Vector2.Distance(myMonster.currentLevel.mainChar.worldCenter, monsterLocation) < SHITTY_FOLLOW_DISTANCE)
            {
                return myMonster.currentLevel.mainChar.worldCenter - monsterLocation;
            }

            else
            {
                return Vector2.Zero;
            }
        }

    }
}
