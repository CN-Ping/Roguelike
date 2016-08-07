using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Roguelike.View
{
    public class MinimapCell
    {
        public static int cellSize = 5;
        public static int bigCellSize = 10;

        static Color dontDrawColour = Color.Black;
        static Texture2D dontDrawTexture;
        static Texture2D dontDrawTextureBig;
        static Dictionary<String, Texture2D> textureDictionary = new Dictionary<string, Texture2D>();
        static Dictionary<String, Texture2D> bigTextureDictionary = new Dictionary<string, Texture2D>();

        static bool initialized = false;


        public bool explored = false;
        Texture2D squareTexture;

        Texture2D bigSquareTexture;
        
        public MinimapCell()
        {
            // to avoid runtime errors, MinimapCell makes its own blank texture, instead of relying on it being passed in through the texture dictionary
            squareTexture = textureDictionary["blank"];
            bigSquareTexture = bigTextureDictionary["blank"];
        }

        public MinimapCell(String colourText)
        {
            squareTexture = textureDictionary[colourText];
            bigSquareTexture = bigTextureDictionary[colourText];
        }

        public static void InitializeTextureDictionary(List<Tuple<String, Color>> colorList, GraphicsDevice g) 
        {
            if (!initialized)
            {
                foreach (Tuple<String, Color> t in colorList)
                {
                    Texture2D newTexture = new Texture2D(g, cellSize, cellSize);

                    Color[] data = new Color[cellSize * cellSize];
                    for (int i = 0; i < data.Length; ++i) data[i] = t.Item2;
                    newTexture.SetData(data);

                    textureDictionary.Add(t.Item1, newTexture);

                    Texture2D newTexture2 = new Texture2D(g, bigCellSize, bigCellSize);

                    Color[] data2 = new Color[bigCellSize * bigCellSize];
                    for (int i = 0; i < data2.Length; ++i) data2[i] = t.Item2;
                    newTexture2.SetData(data2);

                    bigTextureDictionary.Add(t.Item1, newTexture2);
                }

                dontDrawTexture = new Texture2D(g, cellSize, cellSize);
                Color[] data3 = new Color[cellSize * cellSize];
                for (int i = 0; i < data3.Length; ++i) data3[i] = dontDrawColour;
                dontDrawTexture.SetData(data3);


                Texture2D tex = new Texture2D(g, cellSize, cellSize);

                Color[] d = new Color[cellSize * cellSize];
                for (int i = 0; i < d.Length; ++i) d[i] = Color.Black;
                tex.SetData(d);

                textureDictionary.Add("blank", tex);




                dontDrawTextureBig = new Texture2D(g, bigCellSize, bigCellSize);
                Color[] data4 = new Color[bigCellSize * bigCellSize];
                for (int i = 0; i < data4.Length; ++i) data4[i] = dontDrawColour;
                dontDrawTextureBig.SetData(data4);


                Texture2D tex2 = new Texture2D(g, bigCellSize, bigCellSize);

                Color[] d2 = new Color[bigCellSize * bigCellSize];
                for (int i = 0; i < d2.Length; ++i) d2[i] = Color.Black;
                tex2.SetData(d2);

                bigTextureDictionary.Add("blank", tex2);

                initialized = true;
            }
        }

        public void SetColour(String color)
        {
            squareTexture = textureDictionary[color];
            bigSquareTexture = bigTextureDictionary[color];
        }

        public void Draw(SpriteBatchWrapper spriteBatch, Vector2 position)
        {
            if (explored)
            {
                spriteBatch.s.Draw(squareTexture, position, Color.White);
            }

            else
            {
                spriteBatch.s.Draw(dontDrawTexture, position, Color.White);
            }
        }

        public void DrawBig(SpriteBatchWrapper spriteBatch, Vector2 position)
        {
            if (explored)
            {
                spriteBatch.s.Draw(bigSquareTexture, position, Color.White);
            }

            else
            {
                spriteBatch.s.Draw(dontDrawTextureBig, position, Color.White);
            }
        }
    }
}
