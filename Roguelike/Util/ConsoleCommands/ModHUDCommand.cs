using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XNAGameConsole;
using Roguelike.Model;

namespace Roguelike.Util.ConsoleCommands
{
    public class ModHUDCommand : IConsoleCommand
    {
        private Model.Model gameModel;

        public string Name
        {
            get { return "hud"; }
        }

        public string Description
        {
            get { return "Changes hud on/off"; }
        }

        public ModHUDCommand(Model.Model model)
        {
            this.gameModel = model;
        }

        public string Execute(string[] arguments)
        {
            if (arguments.GetLength(0) == 0)
            {
                gameModel.currentLevel.hud.active = !gameModel.currentLevel.hud.active;

                return "Toggled hud to: " + gameModel.currentLevel.hud.active;
            }

            else
            {
                switch (arguments[0]) {
                    case "on":
                        gameModel.currentLevel.hud.active = true;
                        return "Set hud to: " + gameModel.currentLevel.hud.active;

                    case "off":
                        gameModel.currentLevel.hud.active = false;
                        return "Set hud to: " + gameModel.currentLevel.hud.active;

                    default :
                        return "Invalid hud setting. Try 'on' or 'off'";
                }
            }
        }
    }
}
