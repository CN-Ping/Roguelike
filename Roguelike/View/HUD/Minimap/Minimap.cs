using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roguelike.Model.GameObjects;
using Roguelike.Model.Infrastructure;
using Roguelike.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Roguelike.View
{
    public class Minimap
    {
        Level currentLevel;

        Color floorColour = Color.Gray;
        Color wallColour = Color.LightGray;
        Color portalColour = Color.CornflowerBlue;
        Color lootColour = Color.Gold;
        Color levelStartColour = Color.DarkGreen;
        Color doorColour = Color.DarkRed;
        Color playerColour = Color.White;
        Color nullColour = Color.Transparent;

        Color borderColor = Color.DarkSlateGray;

        MinimapCell[,] minimap;
        int[,] megamap;
        
        Tuple<int, int> centerTile;
        MinimapCell[,] drawTheseTiles;

        MinimapCell nullCell;
        MinimapCell playerCell;

        int minimapRadius = 15;
        public int sightRadius;

        int dimensions;
        Vector2 mapDrawLocation;
        Vector2 upperLeftHandCorner;

        int pixelOffset = 20;
        Texture2D frame;

        Texture2D bigframe;

        Vector2 bigDrawLocation;

        public bool IsRevealed { get; private set; }

        public Minimap(Level level)
        {
            IsRevealed = false;

            currentLevel = level;

            megamap = currentLevel.theWorld.GetIntMaze();
            drawTheseTiles = new MinimapCell[minimapRadius * 2 + 1, minimapRadius * 2 + 1];

            initializeMinimapColours();

            nullCell = new MinimapCell();
            nullCell.explored = true;

            playerCell = new MinimapCell("player");
            playerCell.explored = true;

            sightRadius = (int)Math.Ceiling(currentLevel.playerStatsInstance.LightRange / 100) + 1;

            for (int i = 0; i < drawTheseTiles.GetLength(0); i++)
            {
                for (int j = 0; j < drawTheseTiles.GetLength(1); j++)
                {
                    drawTheseTiles[i, j] = nullCell;
                }
            }

            dimensions = MinimapCell.cellSize * (2 * minimapRadius + 1);
            mapDrawLocation = new Vector2(currentLevel.gameModel.gameView.viewport.Width - dimensions - (pixelOffset / 2), pixelOffset / 2);
            upperLeftHandCorner = new Vector2(currentLevel.gameModel.gameView.viewport.Width - dimensions - (pixelOffset), 0);

            int width = dimensions + pixelOffset;
            frame = new Texture2D(currentLevel.gameModel.Game.GraphicsDevice, width, width);

            Color[] data = new Color[width * width];
            for (int i = 0; i < data.Length; ++i) data[i] = borderColor;
            frame.SetData(data);

            GenerateMap();
        }

        private void initializeMinimapColours()
        {
            List<Tuple<String, Color>> l = new List<Tuple<string, Color>> { 
            new Tuple<String, Color>("floor", floorColour),
            new Tuple<String, Color>("wall", wallColour),
            new Tuple<String, Color>("portal", portalColour),
            new Tuple<String, Color>("loot", lootColour),
            new Tuple<String, Color>("levelStart", levelStartColour),
            new Tuple<String, Color>("door", doorColour),
            new Tuple<String, Color>("player", playerColour),
            new Tuple<String, Color>("null", nullColour)
            };

            MinimapCell.InitializeTextureDictionary(l, currentLevel.gameModel.Game.GraphicsDevice);
        }

        private void GenerateMap()
        {
            minimap = new MinimapCell[megamap.GetLength(0), megamap.GetLength(1)];

            for (int i = 0; i < megamap.GetLength(0); i++)
            {
                for (int j = 0; j < megamap.GetLength(1); j++)
                {
                    minimap[i, j] = new MinimapCell();
                    ParseIntMaze(i, j);
                }
            }

            bigDrawLocation = currentLevel.gameModel.gameView.screenCenter - new Vector2((minimap.GetLength(0) / 2) * MinimapCell.bigCellSize, (minimap.GetLength(1) / 2) * MinimapCell.bigCellSize);

            int width = (minimap.GetLength(0) + 2) * MinimapCell.bigCellSize;

            bigframe = new Texture2D(currentLevel.gameModel.Game.GraphicsDevice, width, width);
            Color[] data = new Color[width * width];
            for (int i = 0; i < data.Length; ++i) data[i] = borderColor;
            bigframe.SetData(data);
        }

        /// 0: Error / unset
        /// 1: Wall
        /// 2: Corridor
        /// 3: Room
        /// 4: Door
        /// 5: docking bay
        /// 6: exit teleporter thing
        /// 7: treasure thing
        public void ParseIntMaze(int i, int j)
        {
            switch (megamap[i, j])
            {
                case 0: // error
                    break;
                case 1: // wall
                    if (shouldBeWall(i, j))
                    {
                        minimap[i, j].SetColour("wall");
                    }
                    else
                    {
                        minimap[i, j].SetColour("blank");
                    }
                    break;
                case 2: // corridor
                case 3: // room
                    minimap[i, j].SetColour("floor");
                    break;
                case 4: // door
                    minimap[i, j].SetColour("door");
                    break;
                case 5: // docking bay
                    // TODO make a docking bay
                    minimap[i, j].SetColour("levelStart");
                    break;
                case 6: // teleportermabobby
                    minimap[i, j].SetColour("portal");
                    break;
                case 7: // treasure
                    minimap[i, j].SetColour("loot");
                    break;
                case 8: // boss monster
                    break;
            }
        }

        public void Draw(SpriteBatchWrapper spriteBatch)
        {
            spriteBatch.s.Draw(frame, upperLeftHandCorner, Color.White);

            for (int i = 0; i < drawTheseTiles.GetLength(0); i++)
            {
                for (int j = 0; j < drawTheseTiles.GetLength(1); j++)
                {
                    drawTheseTiles[j, i].Draw(spriteBatch, new Vector2(mapDrawLocation.X + i * MinimapCell.cellSize, mapDrawLocation.Y + j * MinimapCell.cellSize));
                }
            }
        }

        public void DrawBig(SpriteBatchWrapper spriteBatch)
        {
            spriteBatch.s.Draw(bigframe, bigDrawLocation, Color.White);

            for (int i = 0; i < minimap.GetLength(0); i++)
            {
                for (int j = 0; j < minimap.GetLength(1); j++)
                {
                    minimap[j, i].DrawBig(spriteBatch, new Vector2(bigDrawLocation.X + ((1 + i) * MinimapCell.bigCellSize), bigDrawLocation.Y + ((j + 1 )* MinimapCell.bigCellSize)));
                }
            }
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Tuple<int, int> tileo = currentLevel.GetATileAt(currentLevel.mainChar.worldCenter).atileCoords;

            // keep down the updates, if possible
            if (tileo != centerTile) {
                centerTile = tileo;

                int ii = 0;

                for (int i = centerTile.Item1 - minimapRadius; i < centerTile.Item1 + minimapRadius + 1; i++)
                {
                    int jj = 0;
                    for (int j = centerTile.Item2 - minimapRadius; j < centerTile.Item2 + minimapRadius + 1; j++)
                    {
                        if (i >= 0 && j >= 0 && i < minimap.GetLength(0) && j < minimap.GetLength(1))
                        {
                            drawTheseTiles[ii, jj] = minimap[i, j];

                            if ((Math.Abs(centerTile.Item1 - i) < sightRadius) && (Math.Abs(centerTile.Item2 - j) < sightRadius))
                            {
                                drawTheseTiles[ii, jj].explored = true;
                            }
                        }

                        else {
                            drawTheseTiles[ii, jj] = nullCell;
                        }

                        jj++;
                    }
                    ii++;
                }

                drawTheseTiles[minimapRadius, minimapRadius] = playerCell;
            }
        }

        // Ripped straight out of RoomParser. Don't hate.
        private bool shouldBeWall(int i, int j)
        {
            int surrounding = 0;
            int walls = 0;

            int iLength = megamap.GetLength(0);
            int jLength = megamap.GetLength(1);

            for (int ii = -1; ii < 2; ii++)
            {
                for (int jj = -1; jj < 2; jj++)
                {
                    int iSum = i + ii;
                    int jSum = j + jj;
                    if ((iSum < iLength) && (jSum < jLength) && (iSum > -1) && (jSum > -1))
                    {
                        int v = megamap[i + ii, j + jj];
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

        internal void Reveal()
        {
            IsRevealed = true;

            foreach  (MinimapCell c in minimap) {
                c.explored = true;
            }
        }
    }
}
