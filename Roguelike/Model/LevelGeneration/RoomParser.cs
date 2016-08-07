#region Using Statements
using Roguelike.Model.GameObjects.Interactables;
using Roguelike.Model.GameObjects.Loot;
using Roguelike.Model.GameObjects.Monsters;
using Roguelike.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Roguelike.Model.Infrastructure;
#endregion

namespace Roguelike.Model
{
    public class RoomParser
    {

        World theWorld;
        Level level;
        ATile[,] room;
        Random r;

        LootGenerator g;

        public RoomParser(World world, Level levelIn,  LootGenerator g_in)
        {
            g = g_in;
            theWorld = world;
            level = levelIn;
            r = new Random();
        }

        public void ParseRoom(String path)
        {
            StreamReader reader = File.OpenText(path);

            int parsedWidth = 0;
            int parsedHeight = 0;

            string line;
            bool dimensions = false;
            while((line = reader.ReadLine()) != null)
            {
                Console.WriteLine(line);
                if (line.StartsWith("//"))
                {
                    break;
                }
                else if (line.StartsWith("width"))
                {
                    string[] parsed = line.Split('=');
                    parsedWidth = Int32.Parse(parsed[1]);
                }
                else if (line.StartsWith("height"))
                {
                    string[] parsed = line.Split('=');
                    parsedHeight = Int32.Parse(parsed[1]);
                }
                else if (Char.IsNumber(line[0]))
                {
                    if (parsedHeight == 0 || parsedWidth == 0)
                    {
                        throw new Exception("ERROR: Need to specify height and width first\n");
                    }
                    string[] parsedA = line.Split('=');
                    string[] parsedNumbers = parsedA[0].Split(',');
                    string typeString = parsedA[1];
                    int xPos = Int32.Parse(parsedNumbers[0]);
                    int yPos = Int32.Parse(parsedNumbers[1]);
                    TileType type;
                    switch(typeString)
                    {
                        case "floor":
                            type = TileType.Floor;
                            break;
                        case "wall":
                            type = TileType.Wall;
                            break;
                        case "door":
                            type = TileType.Door;
                            break;
                        default:
                            type = TileType.Floor;
                            break;
                    }
                    AssignTile(xPos, yPos, type);
                }
                else 
                {
                    throw new Exception("ERROR: Unhandled case in RoomParser\n");
                }

                /* If both height and width have been specified, make the matrix */
                if (parsedHeight != 0 && parsedWidth != 0 && dimensions == false)
                {
                    room = new ATile[parsedWidth, parsedHeight];
                    dimensions = true;
                }

            }

            /* Fill the rest of the room with floor tiles and wall tiles */
            for (int i = 0; i < room.GetLength(0); i++ )
            {
                for (int j = 0; j < room.GetLength(1); j++)
                {
                    if (room[i, j] == null)
                    {
                        /* Check if it is a corner */
                        if (( i == 0 && (j == 0 || j == parsedHeight-1) ) || (i == parsedWidth-1 && (j == 0 || j == parsedHeight-1)))
                        {
                            //TODO corner case properly so that you know which corner it is supposed to be
                            AssignTile(i, j, TileType.Corner);
                        }
                        else if (i == 0 || j == 0 || i == parsedWidth-1 || j == parsedHeight-1)
                        {
                            //TODO figure out which wall it is
                            AssignTile(i, j, TileType.Wall);
                        }
                        else
                        {
                            AssignTile(i, j, TileType.Floor);
                        }
                    }
                }

            }
        }

        /// <summary>
        /// Adds the parsed room to the world representation.
        /// </summary>
        /// <param name="topLeftX">The x position to start attaching the new room at</param>
        /// <param name="topLeftY">The y position to start attaching the new room at</param>
        public void AttachParsedRoomToWorld(int topLeftX, int topLeftY)
        {
            //TODO: make sure attaching the room is allowed
            for (int i = 0; i < room.GetLength(0); i++)
            {
                for (int j = 0; j < room.GetLength(1); j++)
                {
                    theWorld.AddRoom(topLeftX + i, topLeftY + j, room[i,j]);
                }

            }
        }

        private void AssignTile(int x, int y, TileType type)
        {
            int width = theWorld.tileWidth;
            int height = theWorld.tileHeight;
            int newX = x*width + width / 2;
            int newY = y*height + height / 2;

            switch (type)
            {
                case TileType.Floor:
                    room[x, y] = new FloorTile(level, newX, newY, y, x);
                    level.addGameObject(room[x, y]);
                    PossiblyAddFloorSplats(x, y);
                    break;
            
                case TileType.Wall:
                    room[x, y] = new WallTile(level, newX, newY, y, x);
                    level.addGameObject(room[x, y]);
                    break;

                case TileType.Door:
                    room[x, y] = new DoorTile(level, newX, newY, y, x);
                    level.addGameObject(room[x, y]);
                    addDoor(x,y);
                    /*bool successful = add1x2Splat(x, y, "Textures/doorOpen", TileType.Door, (float)Math.PI / 2);
                    if (successful == false)
                    {
                        add2x1Splat(x, y, "Textures/doorOpen", TileType.Door, 0f);
                    }*/
                    break;

                case TileType.Corner:
                    room[x, y] = new WallTile(level, newX, newY, y, x);
                    level.addGameObject(room[x, y]);
                    break;

                case TileType.Null:
                    room[x, y] = new NullTile(level, newX, newY, y, x);
                    level.addGameObject(room[x, y]);
                    break;
            }

        }

        public void PossiblyAddFloorSplats(int x, int y)
        {
            /* Get number in [1, 100]*/
            int num = r.Next(1, 51);
            if (num < 6)
            {
                int ber = r.Next(1,6);
                if (ber < 3)
                {
                    add1x1Splat(x, y, "Textures/Floor/OilSpill");
                }
                else if (ber < 5)
                {
                    add1x1Splat(x, y, "Textures/Floor/OilDrops");
                }
                else
                {
                    add2x2Splat(x, y, "Textures/Floor/drain", TileType.Floor);
                }
            }
        }

        private bool add1x1Splat(int x, int y, string name)
        {
            int width = theWorld.tileWidth;
            int height = theWorld.tileHeight;

            TextureSplat splat = new TextureSplat(level, (x * width) + width/2, (y * height) + height/2, name);
            int rotation = r.Next(4);
            splat.setRotation(rotation);
            level.addGameObject(splat);
            //theWorld.splats.Add(splat);

            return true;
        }

        /* 1 width x 2 height. Pick the one that corresponds to how you want it in game. Then add rotation so that the sprite matches 1x2*/
        private bool add1x2Splat(int x, int y, string name, TileType tile, int rotation)
        {
            int width = theWorld.tileWidth;
            int height = theWorld.tileHeight;
            bool addedSplat = false;

            ATile other = room[x, y-1];

            if (other != null)
            {
                if (other.tileType == tile)
                {
                    TextureSplat splat = new TextureSplat(level, (x * width) + width/2, (y * height), name);
                    splat.setRotation(rotation);
                    level.addGameObject(splat);
                    //theWorld.splats.Add(splat);
                    addedSplat = true;
                }
            }

            return addedSplat;
        }

        private bool add2x1Splat(int x, int y, string name, TileType tile, int rotation)
        {
            int width = theWorld.tileWidth;
            int height = theWorld.tileHeight;
            bool addedSplat = false;

            ATile other = room[x - 1, y];

            if (other != null)
            {
                if (other.tileType == tile)
                {
                    TextureSplat splat = new TextureSplat(level, (x * width) , (y * height) + height / 2, name);
                    splat.setRotation(rotation);
                    level.addGameObject(splat);
                    //theWorld.splats.Add(splat);
                    addedSplat = true;
                }
            }

            return addedSplat;
        }

        private bool add2x2Splat(int x, int y, string name, TileType tile)
        {
            int width = theWorld.tileWidth;
            int height = theWorld.tileHeight;

            bool addedSplat = false;

            if (x - 1 > 0 && y - 1 > 0)
            {
                /* Check that all relevant tiles are floor */
                ATile a = room[x,y-1];
                ATile b = room[x-1,y];
                ATile c = room[x-1,y-1];
                if (a != null && b != null && c != null)
                {
                    if (a.tileType == tile && b.tileType == tile && c.tileType == tile)
                    {
                        TextureSplat splat = new TextureSplat(level, x*100, y*100, name);
                        level.addGameObject(splat);
                        //theWorld.splats.Add(splat);
                        addedSplat = true;
                    }
                }
            }

            return addedSplat;
        }

        private void addDoor(int x, int y)
        {
            /* Check 1x2 */
            int width = theWorld.tileWidth;
            int height = theWorld.tileHeight;
            bool addedDoor = false;

            ATile other = room[x, y - 1];
            DoorInteractable newDoor = null;

            if (other != null)
            {
                if (other.tileType == TileType.Door)
                {
                    /* Don't have to check that there isn't already a door, because a door
                     * will only be created on the 2nd tile placed for the door. */
                    newDoor = new DoorInteractable(level, (x * width) + width / 2, (y * height));
                    newDoor.setRotation(1);
                    level.addGameObject(newDoor);
                    addedDoor = true;
                }
            }

            //return addedSplat;

            /* Check 2x1 */
            if (addedDoor == false)
            {
                other = room[x - 1, y];

                if (other != null)
                {
                    if (other.tileType == TileType.Door)
                    {
                        newDoor = new DoorInteractable(level, (x * width), (y * height) + height / 2);
                        level.addGameObject(newDoor);
                        addedDoor = true;
                    }
                }
            }

            if (addedDoor)
            {
                DoorTile door1 = (DoorTile)other;
                DoorTile door2 = (DoorTile)room[x, y];
                door1.doorObject = newDoor;
                door2.doorObject = newDoor;
            }
        }

        /// 0: Error / unset
        /// 1: Wall
        /// 2: Corridor
        /// 3: Room
        /// 4: Door
        /// 5: docking bay
        /// 6: exit teleporter thing
        /// 7: treasure thing
        public void ParseIntMaze(int[,] maze)
        {
            room = new ATile[maze.GetLength(0), maze.GetLength(1)];
            
            for (int i = 0; i < maze.GetLength(0); i++ )
            {
                for (int j = 0; j < maze.GetLength(1); j++)
                {
                    switch (maze[i, j])
                    {
                        case 0: // error
                            AssignTile(j, i, TileType.Floor);
                            break;
                        case 1: // wall
                            if (shouldBeWall(maze, i, j)) {
                                AssignTile(j, i, TileType.Wall);
                            }
                            else
                            {
                                AssignTile(j, i, TileType.Null);
                            }
                            break;
                        case 2: // corridor
                            AssignTile(j, i, TileType.Floor);
                            break;
                        case 3: // room
                            AssignTile(j, i, TileType.Floor);
                            break;
                        case 4: // door
                            AssignTile(j, i, TileType.Door);
                            break;
                        case 5: // docking bay
                            // TODO make a docking bay
                            //AssignTile(j, i, TileType.Floor);
                            AssignTile(j, i, TileType.Wall);
                            break;
                        case 6: // teleportermabobby
                            AssignTile(j, i, TileType.Floor);
                            level.portal = new AdvanceLevelInteractable(level, j*100, i*100 ); // this was i, j
                            level.addGameObject(level.portal);
                            break;
                        case 7: // treasure
                            AssignTile(j, i, TileType.Floor);
                            //TODO implement item pool
                            ALoot f = g.getRandomLootFromPool(level, j * 100 + 50, i * 100 + 50);

                            level.addGameObject(f);
                            break;
                        case 8:
                            AssignTile(j, i, TileType.Floor);

                            ZombieMonster newMonster = new ZombieMonster(level, j * 100 + 100, i * 100 + 50);
                            //newMonster.SetData(level, j * 100 + 100, i * 100 + 50);
                            level.addGameObject(newMonster);

                            break;

                        case 9:
                            AssignTile(j, i, TileType.Floor);

                            // make the exit portal here
                            TextureSplat splat = new TextureSplat(level, j * 100 + 50, i * 100 + 50, "Objects/AdvanceLevelTop");
                            splat.layerType = LayerType.AbovePlayer;
                            level.addGameObject(splat);

                            // make the exit portal here
                            TextureSplat splat2 = new TextureSplat(level, j * 100 + 50, i * 100 + 50, "Objects/AdvanceLevelDestination");
                            splat2.layerType = LayerType.Stuff;
                            level.addGameObject(splat2);

                            break;
                    }
                }
            }
        }

        private bool shouldBeWall(int[,] maze, int i, int j)
        {
            int surrounding = 0;
            int walls = 0;

            int iLength = maze.GetLength(0);
            int jLength = maze.GetLength(1);

            for (int ii = -1; ii < 2; ii++)
            {
                for (int jj = -1; jj < 2; jj++)
                {
                    int iSum = i+ii;
                    int jSum = j+jj;
                    if ( (iSum < iLength) && (jSum < jLength) && (iSum > -1) && (jSum > -1))
                    {
                        int v = maze[i + ii, j + jj];
                        if (v == 1)
                        {
                            walls++;
                        }
                        surrounding++;
                    }
                }
            }

            return !(surrounding == walls);
        }

    }
}
