#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Model.LevelGeneration;
using Roguelike.Model.LevelGeneration.GuidedGeneration;
using Roguelike.View;
using Shadows2D;
using Roguelike.Model.Infrastructure;
using Roguelike.Model.GameObjects.Monsters;
#endregion

namespace Roguelike.Model
{
    public class World
    {
        ATile[,] representation;
        int[,] intRepresentation;

        Level level;

        public int tileHeight = 100;
        public int tileWidth = 100;

        public int gridHeight;
        public int gridWidth;
        public int recursionDepth;

        Random rng = new Random();
        List<MonsterEntry> monsters;

        /* Keeping these separate so that they draw before objects and you don't do collision detecting with them. */
        public List<TextureSplat> splats;

        /* tuples are j, i (or x, y, if you prefer) */
        public List<Tuple<MonsterEntry, int, int>> squidSquad = new List<Tuple<MonsterEntry, int, int>>();

        /// <summary>
        /// The grid representation of the world
        /// </summary>
        /// <param name="maxWidth">Maximum number of tiles the world can be wide.</param>
        /// <param name="maxHeight">Maximum number of tiles the world can be high.</param>
        public World(int maxWidth, int maxHeight, int recursionDepth_, Level levelIn)
        {
            gridHeight = maxHeight;
            gridWidth = maxWidth;
            recursionDepth = recursionDepth_;

            level = levelIn;
            monsters = new List<MonsterEntry>();

            //populate monster pools (levels are 0-indexed)
            switch (level.LevelNumber)
            {
                case 0:
                    monsters.Add(new MonsterEntry<SquidMonster>(5.0f, 14.0f, 0.06f));
                    break;

                case 1:
                    monsters.Add(new MonsterEntry<SquidMonster>(5.0f, 10.0f, 0.08f));
                    monsters.Add(new MonsterEntry<SpiderMonster>(5.0f, 10.0f, 0.005f, 9));
                    break;

                case 2:
                    monsters.Add(new MonsterEntry<SquidMonster>(5.0f, 10.0f, 0.08f));
                    monsters.Add(new MonsterEntry<SpiderMonster>(5.0f, 10.0f, 0.05f, 9));
                    break;

                case 3:
                    monsters.Add(new MonsterEntry<SquidMonster>(5.0f, 10.0f, 0.08f));
                    monsters.Add(new MonsterEntry<SpiderMonster>(5.0f, 10.0f, 0.06f, 9));
                    break;

                default:
                    monsters.Add(new MonsterEntry<SquidMonster>(5.0f, 10.0f, 0.08f));
                    monsters.Add(new MonsterEntry<SpiderMonster>(5.0f, 10.0f, 0.05f, 9));
                    monsters.Add(new MonsterEntry<GhostMonster>(5.0f, 14.0f, 0.06f));
                    break;
            }

            representation = new ATile[maxWidth, maxHeight];

            splats = new List<TextureSplat>();

            /* Loop through to fill the world with null */
            for (int i = 0; i < representation.GetLength(0); i++)
            {
                for (int j = 0; i < representation.GetLength(1); i++)
                {
                    representation[i, j] = null;
                }
            }
        }

        public void FirstLevelInitialize()
        {
            GenerateLevel(recursionDepth, true);
        }

        internal void SubsequentLevelInitialize()
        {
            GenerateLevel(recursionDepth, false);
        }

        public void Draw(SpriteBatchWrapper spriteBatch)
        {
            /* Loop through each tile in the world and have it draw itself */
            for (int i = 0; i < representation.GetLength(0); i++ )
            {
                for (int j = 0; j < representation.GetLength(1); j++ )
                {
                    if (representation[i, j] != null)
                    {
                        /*Each tile is 100 by 100*/
                        representation[i, j].Draw(spriteBatch);
                    }
                }
            }

            /* Draw texture splats */
            for (int i = 0; i < splats.Count; i++ )
            {
                splats[i].Draw(spriteBatch);
            }
        }

        public void DrawWalls(SpriteBatchWrapper spriteBatch)
        {
            /* Loop through each tile in the world and have it draw itself */
            for (int i = 0; i < representation.GetLength(0); i++)
            {
                for (int j = 0; j < representation.GetLength(1); j++)
                {
                    if (representation[i, j] != null)
                    {
                        /*Each tile is 100 by 100*/
                        if (representation[i, j].tileType == TileType.Wall) //|| representation[i, j].tileType == TileType.Door)
                        {
                            representation[i, j].Draw(spriteBatch);
                        }
                    }
                }
            }

            ///* Draw texture splats */
            //for (int i = 0; i < splats.Count; i++)
            //{
            //    splats[i].Draw(spriteBatch);
            //}
        }

        public void CastWall(ShadowCasterMap shadowMap)
        {
            for (int i = 0; i < representation.GetLength(0); i++)
            {
                for (int j = 0; j < representation.GetLength(1); j++)
                {
                    if (representation[i, j] != null)
                    {
                        /*Each tile is 100 by 100*/
                        if (representation[i, j].tileType == TileType.Wall)
                        {
                            representation[i, j].DrawCaster(shadowMap);
                        }
                    }
                }
            }
        }
        public void DrawFloor(SpriteBatchWrapper spriteBatch)
        {
            /* Loop through each tile in the world and have it draw itself */
            for (int i = 0; i < representation.GetLength(0); i++)
            {
                for (int j = 0; j < representation.GetLength(1); j++)
                {
                    if (representation[i, j] != null)
                    {
                        /*Each tile is 100 by 100*/
                        if (representation[i, j].tileType == TileType.Floor)
                        {
                            representation[i, j].Draw(spriteBatch);
                        }
                    }
                }
            }

            ///* Draw texture splats */
            for (int i = 0; i < splats.Count; i++)
            {
                splats[i].Draw(spriteBatch);
            }
        }

        /// <summary>
        /// Adds a room to the world's representation
        /// </summary>
        /// <param name="xPos">The x location of the room relative to the world grid</param>
        /// <param name="yPos">The y location of the room relative to the world grid</param>
        /// <param name="toAdd">The ATile to add to the world </param>
        public void AddRoom(int xPos, int yPos, ATile toAdd)
        {
            representation[xPos, yPos] = toAdd;
        }

        /// <summary>
        /// Returns the ATile at the index
        /// </summary>
        /// <param name="x">x index of the ATile you want</param>
        /// <param name="y">y index of the ATile you want</param>
        /// <returns></returns>
        public ATile getATile(int x, int y)
        { 
            if (x >= 0 && y >= 0 && x < gridWidth && y < gridHeight)
            {
                return representation[x, y];
            }
            else
            {
                return null;
            }
        }

        public ATile[,] getATileMatrix()
        {
            return representation;
        }
        
        private void CreateStartingRoom()
        {
            RoomParser parser = new RoomParser(this, level, level.gameModel.g);
            parser.ParseRoom("Content/testRoom.txt");
            parser.AttachParsedRoomToWorld(0,0);
        }

        private void populateMonsters(int[,] maze, int[] startingPos)
        {
            for (int i = 0; i < maze.GetLength(0); i++)
            {
                for (int j = 0; j < maze.GetLength(1); j++)
                {
                    // only spawn in halls or rooms
                    if ((maze[j, i] == 2) || (maze[j, i] == 3))
                    {

                        double chance = rng.NextDouble();
                        double sum = 0.0;

                        foreach (MonsterEntry ME in monsters) {
                            
                            // try to spawn the monster
                            if (chance < ME.SPAWN_RATE + sum)
                            {
                                bool legal = true;

                                int manhatToPlayer = Math.Abs((int)(startingPos[0] - j)) + Math.Abs((int)(startingPos[1] - i));

                                if (manhatToPlayer < ME.PLAYER_BERTH)
                                {
                                    legal = false;
                                }

                                int shortestDistance = int.MaxValue;
                                // only bother checking this if that other condition is legal
                                if (legal)
                                {
                                    // I know this is inefficient. Don't hate. 
                                    foreach (Tuple<MonsterEntry, int, int> s in squidSquad)
                                    {
                                        int manhat = Math.Abs((int)(s.Item2 - j)) + Math.Abs((int)(s.Item3 - i));

                                        shortestDistance = Math.Min(shortestDistance, manhat);

                                        if (shortestDistance < ME.MONSTER_BERTH)
                                        {
                                            legal = false;
                                        }
                                    }
                                }


                                // actually place the squid
                                if (legal)
                                {
                                    squidSquad.Add(new Tuple<MonsterEntry, int, int>(ME, j, i));
                                    break;
                                }

                                else
                                {
                                    sum += ME.SPAWN_RATE;
                                }

                            }

                            else
                            {
                                sum += ME.SPAWN_RATE;
                            }
                        }
                    }
                }
            }
        }

        private void GenerateLevel(int recursionDepth, bool levelOne)
        {
            BetterBSP b = new BetterBSP(level);
            int[,] maze = b.createMap(representation, recursionDepth, levelOne);

            RoomParser parser = new RoomParser(this, level, level.gameModel.g);
            parser.ParseIntMaze(maze);
            parser.AttachParsedRoomToWorld(0, 0);


            Console.WriteLine("world: temporarily overriding player starting position.");
            int[] startingPos = b.getPlayerStartingPosition();
            level.gamePosY = startingPos[0] * 100 + level.mainChar.startingHeight;
            level.gamePosX = startingPos[1] * 100 + level.mainChar.startingWidth;
            
            // TODO make squids not spawn in boss room?
            populateMonsters(maze, startingPos);

            intRepresentation = maze;
        }

        public int[,] GetIntMaze()
        {
            return intRepresentation;
        }
    }
}
