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
    public class RevealMapCommand : IConsoleCommand
    {
        private Model.Model gameModel;

        // this is the actual command you will type
        public string Name
        {
            get { return "marco"; }
        }

        // This is what shows up in the console window help
        public string Description
        {
            get { return "Reveals the map"; }
        }

        // This constructor can take any arguments. Mine just take the stats insance. but they can be anything
        public RevealMapCommand(Model.Model gameModelIn)
        {
            this.gameModel = gameModelIn;
        }

        // Visitor pattern execution
        public string Execute(string[] arguments)
        {
            gameModel.gameView.minimap.Reveal();

            return "Polo!";
        }
    }
}
