using Roguelike.Model.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNAGameConsole;

namespace Roguelike.Util.ConsoleCommands
{
    class KeymapCommand : IConsoleCommand
    {
        Model.Model gameModel;

        // this is the actual command you will type
        public string Name
        {
            get { return "keymap"; }
        }

        // This is what shows up in the console window help
        public string Description
        {
            get { return "toggles (or sets) keyboard layout"; }
        }

        // This constructor can take any arguments. Mine just take the stats insance. but they can be anything
        public KeymapCommand(Model.Model inModel)
        {
            gameModel = inModel;
        }

        // Visitor pattern execution
        public string Execute(string[] arguments)
        {
            if (arguments.Length == 0)
            {
                gameModel.dvorak = !gameModel.dvorak;

                if (gameModel.dvorak)
                    return "Toggled keymap to dvorak.";

                else
                    return "Toggled keymap to qwerty.";
            }

            if (arguments[0].Equals("dvorak"))
            {
                gameModel.dvorak = true;
                return "Set keymap to dvorak.";
            }
            else if (arguments[1].Equals("qwerty"))
            {
                gameModel.dvorak = false;
                return "Set kepmap to qwerty.";
            }

            else
            {
                return "Unrecognized keymap.";
            }
            
        }
    
    }
}
