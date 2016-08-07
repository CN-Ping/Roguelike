using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XNAGameConsole;
using Roguelike.Model;

namespace Roguelike.Util.ConsoleCommands
{
    public class SetHealthCommand : IConsoleCommand
    {
        private StatsInstance playerStats;

        public string Name
        {
            get { return "player.setHealth"; }
        }

        public string Description
        {
            get { return "Sets the player health"; }
        }

        public SetHealthCommand(StatsInstance player)
        {
            this.playerStats = player;
        }

        public string Execute(string[] arguments)
        {
            double newValue = double.Parse(arguments[0]);
            playerStats.health = newValue;
            return "Set player health to " + newValue;
        }
    }
}
