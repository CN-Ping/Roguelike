using Roguelike.Model.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNAGameConsole;

namespace Roguelike.Util.ConsoleCommands
{
    class DebugModeCommand : IConsoleCommand
    {
        Model.Model gameModel;

        // this is the actual command you will type
        public string Name
        {
            get { return "god"; }
        }

        // This is what shows up in the console window help
        public string Description
        {
            get { return "toggles (or sets) keyboard layout"; }
        }

        // This constructor can take any arguments. Mine just take the stats insance. but they can be anything
        public DebugModeCommand(Model.Model inModel)
        {
            gameModel = inModel;
        }

        // Visitor pattern execution
        public string Execute(string[] arguments)
        {
            gameModel.currentLevel.playerStatsInstance.health = gameModel.currentLevel.playerStatsInstance.health * 11111;
            gameModel.currentLevel.playerStatsInstance.speed = gameModel.currentLevel.playerStatsInstance.speed * 3;
            gameModel.currentLevel.playerStatsInstance.LightRange = 800;
            gameModel.currentLevel.playerStatsInstance.damage = gameModel.currentLevel.playerStatsInstance.damage * 8;
            gameModel.currentLevel.playerStatsInstance.torchCount = 99999;
            gameModel.currentLevel.playerStatsInstance.shotSpeed = gameModel.currentLevel.playerStatsInstance.shotSpeed * 2;
            gameModel.currentLevel.playerStatsInstance.rateOfFire = gameModel.currentLevel.playerStatsInstance.rateOfFire / 3;
            return "Player is in god mode";

        }

    }
}
