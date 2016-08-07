using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XNAGameConsole;
using Roguelike.Model;

namespace Roguelike.Util.ConsoleCommands
{
    public class ModLightCommand : IConsoleCommand
    {
        private Model.Model gameModel;

        public string Name
        {
            get { return "light"; }
        }

        public string Description
        {
            get { return "Changes lighting on/off"; }
        }

        public ModLightCommand(Model.Model model)
        {
            this.gameModel = model;
        }

        public string Execute(string[] arguments)
        {
            if (arguments.GetLength(0) == 0)
            {
                gameModel.lightingEnabled = !gameModel.lightingEnabled;

                return "Toggled lighting to: " + gameModel.lightingEnabled;
            }

            else
            {
                switch (arguments[0]) {
                    case "on":
                        gameModel.lightingEnabled = true;
                        return "Set lighting to: " + gameModel.lightingEnabled;

                    case "off":
                        gameModel.lightingEnabled = false;
                        return "Set lighting to: " + gameModel.lightingEnabled;

                    default :
                        return "Invalid light setting. Try 'on' or 'off'";
                }
            }
        }
    }
}
