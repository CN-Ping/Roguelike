using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XNAGameConsole;
using Roguelike.Model;

namespace Roguelike.Util.ConsoleCommands
{
    public class SetSpeedCommand : IConsoleCommand
    {
        private StatsInstance playerStats;

        public string Name
        {
            get { return "player.setSpeed"; }
        }

        public string Description
        {
            get { return "Sets the player speed"; }
        }

        public SetSpeedCommand(StatsInstance player)
        {
            this.playerStats = player;
        }

        public string Execute(string[] arguments)
        {
            double newSpeed = double.Parse(arguments[0]);
            playerStats.xVel = (int)((playerStats.xVel / playerStats.speed) * newSpeed);
            playerStats.yVel = (int)((playerStats.yVel / playerStats.speed) * newSpeed);
            playerStats.speed = newSpeed;
            return "Set player speed to " + newSpeed;
        }
    }
}
