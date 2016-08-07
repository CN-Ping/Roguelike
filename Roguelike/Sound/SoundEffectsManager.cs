using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using System.IO;

namespace Roguelike.Sound
{
    public class SoundEffectsManager
    {
        static int MIN_DELAY = 10000;
        static int MAX_DELAY = 35000;

        int duration = 0;
        int timer = 0;

        static Random rng = new Random();
        
        Model.Model gameModel;
        Dictionary<String, SoundEffect> sounds;

        public SoundEffectsManager(Model.Model gameModel)
        {
            this.gameModel = gameModel;

            sounds = LoadContent<SoundEffect>(gameModel.Game.Content, "Sound/ambience");

            duration = rng.Next(MIN_DELAY, MAX_DELAY);
            gameModel.ConsoleWriteLine("SoundEffectsManager: First sound effect playing in " + duration + " ms.");
        }

        /// <summary>
        /// Load all content within a certain folder. The function
        /// returns a dictionary where the file name, without type
        /// extension, is the key and the texture object is the value.
        ///
        /// The contentFolder parameter has to be relative to the
        /// game.Content.RootDirectory folder.
        /// </summary>
        /// <typeparam name="T">The content type.</typeparam>
        /// <param name="contentManager">The content manager for which content is to be loaded.</param>
        /// <param name="contentFolder">The game project root folder relative folder path.</param>
        /// <returns>A list of loaded content objects.</returns>
        public static Dictionary<String, T> LoadContent<T>(ContentManager contentManager, string contentFolder)
        {
            //Load directory info, abort if none
            DirectoryInfo dir = new DirectoryInfo(contentManager.RootDirectory + "\\" + contentFolder);
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException();
            }

            //Init the resulting list
            Dictionary<String, T> result = new Dictionary<String, T>();

            //Load all files that matches the file filter
            FileInfo[] files = dir.GetFiles("*.*");
            foreach (FileInfo file in files)
            {
                string key = Path.GetFileNameWithoutExtension(file.Name);

                result[key] = contentManager.Load<T>(contentFolder + "/" + key);
            }

            //Return the result
            return result;
        }


        public void Update(int t)
        {

            if (t - timer > duration) {
                KeyValuePair<String, SoundEffect> kvp = sounds.ElementAt(rng.Next(sounds.Count));
                
                kvp.Value.Play(0.1f, 1f, 0f);

                duration = rng.Next(MIN_DELAY, MAX_DELAY);
                gameModel.ConsoleWriteLine("SoundEffectsManager: Playing " + kvp.Key + ". Next sound effect in " + duration + " ms.");

                timer = t;
            }
        }
    }
}


