using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Roguelike.Model.Infrastructure;
using System.IO;

namespace Roguelike.Model.GameObjects.Monsters.Randomized
{
    public class RandomMonsterGenerator
    {
        List<Texture2D> heads = new List<Texture2D>();
        List<Texture2D> bodies = new List<Texture2D>();
        List<Texture2D> llegs = new List<Texture2D>();
        List<Texture2D> rlegs = new List<Texture2D>();
        List<Texture2D> rarms = new List<Texture2D>();
        List<Texture2D> larms = new List<Texture2D>();

        Tuple<String, List<Texture2D>>[] directories;

        Random rng = new Random();

        public RandomMonsterGenerator(Model modelIn)
        {
            directories = new Tuple<String,List<Texture2D>>[] {
                new Tuple<String, List<Texture2D>>("Head", heads),
                new Tuple<String, List<Texture2D>>("Body", bodies),
                new Tuple<String, List<Texture2D>>("LArm", larms),
                new Tuple<String, List<Texture2D>>("RArm", rarms),
                new Tuple<String, List<Texture2D>>("LLeg", llegs),
                new Tuple<String, List<Texture2D>>("RLeg", rlegs)
            };

            String contentFolder = "Sprites/RandomMonsters";

            for (int i = 0; i < directories.Length; i++)
            {
                DirectoryInfo dir = new DirectoryInfo(modelIn.Game.content.RootDirectory + "\\" + contentFolder + "\\" + directories[i].Item1);
                if (!dir.Exists)
                {
                    throw new DirectoryNotFoundException();
                }

                //Load all files that matches the file filter
                FileInfo[] files = dir.GetFiles("*.*");
                foreach (FileInfo file in files)
                {
                    string key = Path.GetFileNameWithoutExtension(file.Name);
                    directories[i].Item2.Add(modelIn.Game.content.Load<Texture2D>(contentFolder + "/" + directories[i].Item1 + "/" + key));
                }
            }
            


        }

        public RandomMonster generateMonster(Level currentLevel, int x, int y)
        {
            return new RandomMonster(currentLevel, x, y, heads[rng.Next(heads.Count)], bodies[rng.Next(bodies.Count)], larms[rng.Next(larms.Count)], rarms[rng.Next(rarms.Count)], llegs[rng.Next(llegs.Count)], rlegs[rng.Next(rlegs.Count)]);
        }
    }
}
