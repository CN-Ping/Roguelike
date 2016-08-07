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
    public class SpawnRandomMonsterCommand : IConsoleCommand
    {
        private Model.Model gameModel;

        // this is the actual command you will type
        public string Name
        {
            get { return "spawnRandomMonster"; }
        }

        // This is what shows up in the console window help
        public string Description
        {
            get { return "[x, y] Spawns a random monster at (x,y) relative to the player"; }
        }

        // This constructor can take any arguments. Mine just take the stats insance. but they can be anything
        public SpawnRandomMonsterCommand(Model.Model modelIn)
        {
            this.gameModel = modelIn;
        }

        // Visitor pattern execution
        public string Execute(string[] arguments)
        {
            if (arguments.Length > 1)
            {
                int xPos = int.Parse(arguments[0]) + (int)gameModel.currentLevel.mainChar.worldCenter.X;
                int yPos = int.Parse(arguments[1]) + (int)gameModel.currentLevel.mainChar.worldCenter.Y;

                gameModel.currentLevel.spawnRandomMonster(xPos, yPos);

                return "spawned random Monster at (" + xPos + ", " + yPos + ")";
            }

            return "invalid number of arguments";
        }
    }
}

