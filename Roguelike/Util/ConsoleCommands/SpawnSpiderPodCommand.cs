using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XNAGameConsole;
using Roguelike.Model;
using Roguelike.Model.GameObjects.Monsters;
using Roguelike.Model.Infrastructure;

namespace Roguelike.Util.ConsoleCommands
{
    /// <summary>
    /// This is the documented example for how to make and register commands.
    /// 
    /// Make you register the command in GameMain.cs: initializeConsole();
    /// </summary>
    public class SpawnSpiderPodCommand : IConsoleCommand
    {
        private Model.Model gameModel;

        // this is the actual command you will type
        public string Name
        {
            get { return "spiders"; }
        }

        // This is what shows up in the console window help
        public string Description
        {
            get { return "spawn 9 spiders centered around the provided [x, y] point"; }
        }

        // This constructor can take any arguments. Mine just take the stats insance. but they can be anything
        public SpawnSpiderPodCommand(Model.Model modelIn)
        {
            this.gameModel = modelIn;
        }

        // Visitor pattern execution
        public string Execute(string[] arguments)
        {
            int xPos = int.Parse(arguments[0]) + (int)gameModel.currentLevel.mainChar.worldCenter.X;
            int yPos = int.Parse(arguments[1]) + (int)gameModel.currentLevel.mainChar.worldCenter.Y;

            int[] positions = new int[] {-20, 0, 20};

            foreach (int pos_x in positions)
            {
                foreach (int pos_y in positions)
                {
                    AMonster m = new SpiderMonster(gameModel.currentLevel, xPos + pos_x, yPos + pos_y);
                    gameModel.currentLevel.addGameObject(m);
                }
            }

            return "spawned a bunch of spiders at (" + xPos + ", " + yPos + ")";
        }
    }
}

