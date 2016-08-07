using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XNAGameConsole;
using Roguelike.Model;
using Roguelike.Model.GameObjects.Loot;
using Roguelike.Model.GameObjects.Pickups;

namespace Roguelike.Util.ConsoleCommands
{
    /// <summary>
    /// This is the documented example for how to make and register commands.
    /// 
    /// Make you register the command in GameMain.cs: initializeConsole();
    /// </summary>
    public class SpawnPickupByIdCommand : IConsoleCommand
    {
        private Model.Model gameModel;
        private PickupGenerator g;

        // this is the actual command you will type
        public string Name
        {
            get { return "spawnPickupById"; }
        }

        // This is what shows up in the console window help
        public string Description
        {
            get { return "[pickup, x, y] Spawns the desired pickup at (x,y) relative to the player"; }
        }

        // This constructor can take any arguments. Mine just take the stats insance. but they can be anything
        public SpawnPickupByIdCommand(Model.Model modelIn)
        {
            this.gameModel = modelIn;
            g = new PickupGenerator();
        }

        // Visitor pattern execution
        public string Execute(string[] arguments)
        {
            if (arguments.Length < 3)
            {
                return "Not enough arguments.";
            }

            int itemId = int.Parse(arguments[0]);
            int xPos = int.Parse(arguments[1]) + (int)gameModel.currentLevel.mainChar.worldCenter.X;
            int yPos = int.Parse(arguments[2]) + (int)gameModel.currentLevel.mainChar.worldCenter.Y;

            gameModel.currentLevel.addGameObject(g.getPickupById(itemId, gameModel.currentLevel, xPos, yPos));

            return "spawned " + itemId + " at (" + xPos + ", " + yPos + ")";
        }
    }
}
