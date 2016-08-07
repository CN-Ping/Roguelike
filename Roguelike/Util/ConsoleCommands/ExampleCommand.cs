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
    public class ExampleCommand : IConsoleCommand
    {
        private StatsInstance playerStats;

        // this is the actual command you will type
        public string Name
        {
            get { return "example"; }
        }

        // This is what shows up in the console window help
        public string Description
        {
            get { return "Sets the player damage"; }
        }

        // This constructor can take any arguments. Mine just take the stats insance. but they can be anything
        public ExampleCommand(StatsInstance player)
        {
            this.playerStats = player;
        }

        // Visitor pattern execution
        public string Execute(string[] arguments)
        {
            float newDamage = float.Parse(arguments[0]);
            playerStats.speed = newDamage;
            return "Set player damage to " + newDamage;
        }
    }
}
