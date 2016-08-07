using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XNAGameConsole;
using Roguelike.Model;

namespace Roguelike.Util.ConsoleCommands
{
    /// <summary>
    /// This is the documented example for how to make and register commands.
    /// 
    /// Make you register the command in GameMain.cs: initializeConsole();
    /// </summary>
    public class SkipToLevelCommand : IConsoleCommand
    {
        private Model.Model gameModel;

        // this is the actual command you will type
        public string Name
        {
            get { return "warp"; }
        }

        // This is what shows up in the console window help
        public string Description
        {
            get { return "Warps to provided level"; }
        }

        // This constructor can take any arguments. Mine just take the stats insance. but they can be anything
        public SkipToLevelCommand(Model.Model gameModelIn)
        {
            this.gameModel = gameModelIn;
        }

        // Visitor pattern execution
        public string Execute(string[] arguments)
        {
            if (arguments.Length < 1)
            {
                return "Not enough arguments. Please provide a level";
            }

            int l_number = int.Parse(arguments[0]);

            gameModel.currentLevel.LevelNumber = l_number - 1;

            gameModel.currentLevel.Ending = true;

            return "Set level to: " + l_number;
        }
    }
}
