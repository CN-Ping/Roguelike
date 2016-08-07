using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roguelike.Model.Infrastructure;

namespace Roguelike.Model.LevelGeneration.GuidedGeneration
{
    class BetterBSP
    {
        Random rng = new Random();
        int playerStartI = 0;
        int playerStartJ = 0;

        public BetterBSP(Level levelIn)
        {
            
        }

        public int[,] createMap(ATile[,] representation, int recursionDepth, bool levelOne)
        {
            int iSize = representation.GetLength(0);
            int jSize = representation.GetLength(1);

            return createMap(iSize, jSize, recursionDepth, levelOne);
        }

        public int[,] createMap(int iSize, int jSize, int recursionDepth, bool levelOne)
        {
            int[,] map = initialize(iSize, jSize);

            HashSet<BSPTreeNode> properRooms = new HashSet<BSPTreeNode>();
            HashSet<BSPTreeNode> allRooms = new HashSet<BSPTreeNode>();

            BSPTreeNode tree = placeRooms(map, recursionDepth, properRooms, allRooms);

            if (levelOne)
            {
                findStartL1(map);
            }

            else
            {
                findStartElse(map, tree);
            }

            findExit(tree, map);

            placeTreasureAndBoss(map, tree, allRooms, properRooms);

            //printMap(map);

            return map;
        }

        public BSPTreeNode placeRooms(int[,] maze, int recursionDepth, HashSet<BSPTreeNode> allRooms, HashSet<BSPTreeNode> properRooms)
        {
            int iSize = maze.GetLength(0);
            int jSize = maze.GetLength(1);

            BSPTreeNode tree = new BSPTreeNode(recursionDepth, iSize, jSize, maze);
            tree.splitSpace();
            tree.connectChildren();

            //printMap(maze);

            tree.placeDoors(properRooms, allRooms);
            //System.Console.WriteLine(tree.toString());
            return tree;
        }

        private void findStartL1(int[,] maze)
        {
            bool found = false;
            //int rooms = 0;
            int j = 0;

            while (!found)
            {
                int roomHeight = 0;
                bool go = true;

                for (int i = 0; ((i < maze.GetLength(0)) && (go)); i++)
                {
                    int val = maze[i,j];

                    switch (val)
                    {
                        case 1:
                            if (roomHeight > 3)
                            {
                                playerStartI = (i - roomHeight) + (roomHeight / 2);
                                playerStartJ = j;
                                Console.WriteLine("BetterBSP: Player starting position computed to be: i = " + playerStartI + ", j = " + playerStartJ);

                                maze[playerStartI, j - 1] = 5;
                                maze[playerStartI + 1, j - 1] = 5;
                                go = false;
                                found = true;
                            }

                            else if (roomHeight > 0)
                            {
                                roomHeight = 0;
                            }
                            break;

                        case 3:
                            roomHeight++;
                            break;
                    }
                }

                j++;
            }
        }

        private void findStartElse(int[,] maze, BSPTreeNode tree)
        {
            Tuple<int, int> t = tree.randomRecurse();

            playerStartI = t.Item1;
            playerStartJ = t.Item2;

            maze[playerStartI, playerStartJ] = 9;
        }

        private void findExit(BSPTreeNode tree, int[,] map)
        {
            // search tree for child node containing room.
            Tuple<int, int> exit = tree.computeExitRelativeTo(playerStartI, playerStartJ);
            map[exit.Item1, exit.Item2] = 6;
        }

        public int[] getPlayerStartingPosition() {
            return new int[]{playerStartI, playerStartJ};
        }

        private void placeTreasureAndBoss(int[,] map, BSPTreeNode tree, HashSet<BSPTreeNode> properRooms, HashSet<BSPTreeNode> allRooms)
        {
            if (properRooms.Count > 0)
            {
                // do the boss first because he has the most stringent requirements
                BSPTreeNode[] asArray = properRooms.ToArray();
                BSPTreeNode bossRoom = asArray[rng.Next(asArray.Length)];

                allRooms.Remove(bossRoom);

                //transform this boss room
                map[bossRoom.centerI, bossRoom.centerJ] = 8;
            }


            BSPTreeNode[] asArray2 = allRooms.ToArray();
            BSPTreeNode treasureRoom = asArray2[rng.Next(asArray2.Length)];

            map[treasureRoom.centerI, treasureRoom.centerJ] = 7;
        }

        private int[,] initialize(int isize_, int jsize_)
        {
            int[,] maze = new int[isize_, jsize_];
            Random rnd = new Random();

            // Random initialization
            for (int i = 0; i < isize_; i++)
            {
                for (int j = 0; j < jsize_; j++)
                {
                    maze[i, j] = 1;
                }
            }

            return maze;
        }

        /// <summary>
        /// 0: Error / unset
        /// 1: Wall
        /// 2: Corridor
        /// 3: Room
        /// 4: Door
        /// 5: Starting entrance
        /// 6: exit teleporter
        /// 7: treasure room
        /// 8: boss room
        /// </summary>
        /// <param name="maze"></param>
        private void printMap(int[,] maze)
        {
            int numRows = maze.GetLength(0);
            int numCols = maze.GetLength(1);

            for (int i = 0; i < numRows; i++)
            {
                String s = "";
                for (int j = 0; j < numCols; j++)
                {
                    char c = '@';

                    switch (maze[i, j])
                    {
                        case 0:
                            c = '$';
                            break;

                        case 1:
                            c = '#';
                            break;

                        case 2:
                            c = ' ';
                            break;

                        case 3:
                            c = '.';
                            break;

                        case 4:
                            c = 'X';
                            break;

                        case 5:
                            c = '@';
                            break;
                            
                        case 6:
                            c = 'O';
                            break;

                        case 7:
                            c = 'T';
                            break;

                        case 8:
                            c = 'B';
                            break;
                    }

                    s += c + " ";
                }
               Console.WriteLine(s);
            }
            //Console.WriteLine("");
        }
    }
}
