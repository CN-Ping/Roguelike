using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roguelike.Model.LevelGeneration.GuidedGeneration
{
    class BSPTreeNode
    {
        private double W_RATIO = 0.45; /* The required ratio of room width to height for a room to be accepted*/
        private double H_RATIO = 0.45; /* The same thing, but for horizontal rooms. 0.45 is the stock number. Feel free to play around. */
        private double SPLIT_MIN = 0.4; /* The minimum percentage through the room that the split is allowed to occur. Default: 0.4 */
        private double SPLIT_MAX = 0.6; /* The maximum percentage through the room that the split is allowed to occur. Default: 0.6 */
        private double BUFFER_SCALE = 0.3; /* The maximum percentage of each cell that will remain wall when the room is placed. */

        private static Random rng = new Random();

        protected int i, j;
        protected int width, height;
        private int recursionDepth;
        private int maxRecursionDepth;

        private int room1i = -1, room1j = -1;
        private int room2i = -1, room2j = -1;
        private int roomHeight = -1;
        private int roomWidth = -1;

        public int centerI, centerJ;

        private int[,] map;

        private BSPTreeNode leftChild = null;
        private BSPTreeNode rightChild = null;

        private bool generatedCorridors = false;

        private List<List<int[]>> entrances = new List<List<int[]>>();

        private HashSet<BSPTreeNode> properRooms;
        private HashSet<BSPTreeNode> allRooms;

        public BSPTreeNode(int maxRecursionDepth_, int recursionDepth_, int i_, int j_, int width_, int height_, int[,] map_)
        {
            recursionDepth = recursionDepth_;
            maxRecursionDepth = maxRecursionDepth_;
            map = map_;

            i = i_;
            j = j_;
            width = width_;
            height = height_;

            centerI = i + (height / 2);
            centerJ = j + (width / 2);
        }

        public BSPTreeNode(int maxRecursionDepth_, int width_, int height_, int[,] map_)
        {
            maxRecursionDepth = maxRecursionDepth_;
            width = width_;
            height = height_;
            map = map_;

            recursionDepth = 0;
            i = 0;
            j = 0; 
        }

        public BSPTreeNode(int maxRecursionDepth_, int recursionDepth_, int i_, int j_, int width_, int height_, int[,] map_, double wRatio_, double hoRatio_, double splitMin_, double splitMax_, double buff_)
        {
            recursionDepth = recursionDepth_;
            maxRecursionDepth = maxRecursionDepth_;
            map = map_;

            i = i_;
            j = j_;
            width = width_;
            height = height_;

            centerI = i + (height / 2);
            centerJ = j + (width / 2);

            W_RATIO = wRatio_;
            H_RATIO = hoRatio_;
            SPLIT_MIN = splitMin_;
            SPLIT_MAX = splitMax_;
            BUFFER_SCALE = buff_;
        }

        public bool isLoaf()
        {
            return (leftChild == null && rightChild == null);
        }

        public void placeRoom()
        {
            if (!isLoaf())
            {
                throw new Exception("Room placement should only take place in leaves.");
            }

            // ...

            double i_buff_1 = rng.NextDouble() * BUFFER_SCALE;
            double i_buff_2 = rng.NextDouble() * BUFFER_SCALE;
            double j_buff_1 = rng.NextDouble() * BUFFER_SCALE;
            double j_buff_2 = rng.NextDouble() * BUFFER_SCALE;

            int i1 = i + (int)Math.Ceiling(height * i_buff_1);
            int j1 = j + (int)Math.Ceiling(width * j_buff_1);

            int i2 = i + height - (int)Math.Ceiling(height * i_buff_2);
            int j2 = j + width - (int)Math.Ceiling(width * j_buff_2);

            room1i = i1;
            room1j = j1;
            room2i = i2 - 1;
            room2j = j2 - 1;

            //System.Console.WriteLine("BSPTreeNode: A room is placed placed here at recursion depth: " + recursionDepth + ", position: (" + room1i + "," + room1j + "), width:" + (j2 - j1) + " and height:" + (i2 - i1));
            roomHeight = (i2 - i1);
            roomWidth = (j2 - j1);

            for (int ii = i1; ii < i2; ii++)
            {
                for (int jj = j1; jj < j2; jj++)
                {
                    if (ii >= map.GetLength(0))
                    {
                        System.Console.WriteLine("Omitting too far down. i = " + ii);
                    }
                    if (jj >= map.GetLength(1))
                    {
                        System.Console.WriteLine("Omitting too far over. j = " + jj);
                    }
                    else
                    {
                        map[ii, jj] = 3;
                    }
                    
                }
            }
        }
        
        public void splitSpace()
        {
            if (recursionDepth < maxRecursionDepth) {
                // keep splitting

                double r1_w_ratio, r2_w_ratio;
                double r1_h_ratio, r2_h_ratio;

                int r1_w, r1_h;
                int r2_w, r2_h;

                BSPTreeNode potentialLeft = null;
                BSPTreeNode potentialRight = null;

                // Get acceptable values for a width and height.
                do {
                    r1_w_ratio = 0.0;
                    r2_w_ratio = 0.0;
                    r1_h_ratio = 0.0;
                    r2_h_ratio = 0.0;

                    // decide to split vertically or horizontally
                    if (rng.Next(2) == 0) // vertical
                    {
                        r1_h = height;
                        r2_h = height;

                        r1_w = (int)(((rng.NextDouble() * (SPLIT_MAX - SPLIT_MIN)) + SPLIT_MIN) * (double)width);
                        r2_w = width - r1_w;

                        r1_w_ratio = (double)r1_w / (double)r1_h;
                        r2_w_ratio = (double)r2_w / (double)r2_h;

                        potentialLeft = new BSPTreeNode(maxRecursionDepth, recursionDepth + 1, i, j, r1_w, r1_h, map);
                        potentialRight = new BSPTreeNode(maxRecursionDepth, recursionDepth + 1, i, j + r1_w, r2_w, r2_h, map);
                    }
                    else // horizontal
                    {
                        r1_w = width;
                        r2_w = width;

                        r1_h = (int)(((rng.NextDouble() * (SPLIT_MAX - SPLIT_MIN)) + SPLIT_MIN) * (double)height);
                        r2_h = height - r1_h;

                        r1_h_ratio = (double)r1_h / (double)r1_w;
                        r2_h_ratio = (double)r2_h / (double)r2_w;

                        potentialLeft = new BSPTreeNode(maxRecursionDepth, recursionDepth + 1, i, j, r1_w, r1_h, map, W_RATIO, H_RATIO, SPLIT_MIN, SPLIT_MAX, BUFFER_SCALE);
                        potentialRight = new BSPTreeNode(maxRecursionDepth, recursionDepth + 1, i + r1_h, j, r2_w, r2_h, map, W_RATIO, H_RATIO, SPLIT_MIN, SPLIT_MAX, BUFFER_SCALE);
                    }

                } while ((r1_w_ratio < W_RATIO || r2_w_ratio < W_RATIO) && (r1_h_ratio < H_RATIO || r2_h_ratio < H_RATIO));

                leftChild = potentialLeft;
                rightChild = potentialRight;

                leftChild.splitSpace();
                rightChild.splitSpace();
            }
            else
            {
                placeRoom();
            }
        }

        public bool hasGeneratedCorridors()
        {
            return generatedCorridors;
        }

        public String toString()
        {
            if (isLoaf())
            {
                return "([" + i + "," + j + "]:" + height + "x" + width + ")";
            }
            else
            {
                return "{([" + i + "," + j + "]:" + height + "x" + width + "):" + leftChild.toString() + ";" + rightChild.toString() + "}";
            }
        }

        public void connectChildren()
        {
            //System.Console.WriteLine("Recursively placing corridors.");
            if (isLoaf())
            {
                //System.Console.WriteLine("Imma loaf");
                return;
            }

            else
            {
                leftChild.connectChildren();
                rightChild.connectChildren();

                for (int a = leftChild.centerI; a <= rightChild.centerI + 1; a++)
                {
                    for (int b = leftChild.centerJ; b <= rightChild.centerJ + 1; b++)
                    {
                        if (map[a, b] == 1)
                        {
                            map[a, b] = 2;
                        }
                    }
                }
            }
        }

        public void placeDoors(HashSet<BSPTreeNode> properRoomsIn, HashSet<BSPTreeNode> allRoomsIn)
        {
            this.properRooms = properRoomsIn;
            this.allRooms = allRoomsIn;

            if (!isLoaf())
            {
                leftChild.placeDoors(properRooms, allRooms);
                rightChild.placeDoors(properRooms, allRooms);
            }

            else
            {
                //System.Console.WriteLine("BSPTreeNode: Placing door for room: " + ", position: (" + room1i + "," + room1j + "), width:" + roomWidth + " and height:" + roomHeight);
                
                int numEntrances = 0;
                bool isLegal = true;

                // these are in order: (top, left, right, bottom)
                // and these span the outside of the rooms
                int[] i1s = new int[] {(room1i - 1), (room1i - 1), (room1i - 1), (room1i + roomHeight)};
                int[] i2s = new int[] {(room1i - 1), (room1i + roomHeight), (room1i + roomHeight), (room1i + roomHeight)};
                int[] j1s = new int[] {(room1j - 1), (room1j - 1), (room1j + roomWidth), (room1j - 1) };
                int[] j2s = new int[] {(room1j + roomWidth), (room1j - 1), (room1j + roomWidth), (room1j + roomWidth) };

                for (int iter = 0; iter < 4; iter++)
                {
                    numEntrances += handleDoor(i1s[iter], j1s[iter], i2s[iter], j2s[iter]);

                    if (numEntrances > 1)
                    {
                        isLegal = false;
                        break;
                    }
                }

                if (isLegal)
                {
                    // assert this truth
                    if (entrances.Count != 1)
                    {
                        Console.Error.WriteLine("Entrances size mismatch!");
                    }

                    map[entrances.ElementAt(0).ElementAt(0)[0], entrances.ElementAt(0).ElementAt(0)[1]] = 4;
                    map[entrances.ElementAt(0).ElementAt(1)[0], entrances.ElementAt(0).ElementAt(1)[1]] = 4;

                    properRooms.Add(this);
                }

                allRooms.Add(this);
            }
        }

        public int handleDoor(int startI, int startJ, int endI, int endJ)
        {
            //Console.WriteLine("BSPTreeNode:     Looping on: " + startI + " <= i <= " + endI + ", " + startJ + " <= j <= " + endJ);
            int currentLength = 0;
            int numEntrances = 0;

            List<int[]> currentEntrance = new List<int[]>();

            for (int eye = startI; eye <= endI; eye++)
            {
                for (int jay = startJ; jay <= endJ ; jay++)
                {
                    switch (map[eye, jay])
                    {
                        case 1: // wall
                            if (currentLength > 0)
                            {
                                entrances.Add(currentEntrance);
                                currentEntrance = new List<int[]>();

                                if (currentLength > 2)
                                {
                                    return 666; //illegal
                                }
                                currentLength = 0;
                            }
                            break;

                        case 2: // corridor
                            if (currentLength == 0)
                            {
                                numEntrances++;
                            }

                            currentLength++;
                            currentEntrance.Add(new int[]{eye, jay});
                            break;

                        case 3: // room (under current parameters, this should never happen)
                            Console.WriteLine("BSPTreeNode: You shouldn't have hit room...");
                            return 665;
                    }
                }
            }

            return numEntrances;
        }

        public Tuple<int, int> computeExitRelativeTo(int i, int j)
        {
            // pick the opposite child
            //BSPTreeNode oppositeChild = (((leftChild.i) <= i && (leftChild.i + leftChild.height >= i)) && ((leftChild.j < j) && (leftChild.j + leftChild.width >= j))) ? rightChild : leftChild;
            //return oppositeChild.randomRecurse();

            removeStartingRoom(i, j);

            // I made this one line for Sarah's horror
            return ((((leftChild.i) <= i && (leftChild.i + leftChild.height >= i)) && ((leftChild.j < j) && (leftChild.j + leftChild.width >= j))) ? rightChild : leftChild).randomRecurse();
        }

        public Tuple<int, int> randomRecurse()
        {
            //if (isLoaf())
            //{
                // place the exit
            //    return new Tuple<int, int>(centerI, centerJ);
            //}

            //remove for later use
            if (isLoaf())
            {
                properRooms.Remove(this);
                allRooms.Remove(this);
            }

            //recurse down a random child
            //return ((rng.NextDouble() < 0.5) ? leftChild : rightChild).randomRecurse();

            // made this a single line also for Sarah's horror
            return (isLoaf() ? (new Tuple<int, int>(centerI, centerJ)) : ((rng.NextDouble() < 0.5) ? leftChild : rightChild).randomRecurse());
        }

        private void removeStartingRoom(int i, int j)
        {
            if (isLoaf())
            {
                Console.WriteLine("Removing starting room, the one at: " + this.i + ", " + this.j);
                allRooms.Remove(this);
                properRooms.Remove(this);
            }

            else
            {
                ((((leftChild.i) <= i && (leftChild.i + leftChild.height >= i)) && ((leftChild.j < j) && (leftChild.j + leftChild.width >= j))) ? leftChild : rightChild).removeStartingRoom(i, j);
            }
        }
    }
}
