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
    public class ToastCommand : IConsoleCommand
    {
        private Model.Model gameModel;

        // this is the actual command you will type
        public string Name
        {
            get { return "toast"; }
        }

        // This is what shows up in the console window help
        public string Description
        {
            get { return "toasts the provided text"; }
        }

        // This constructor can take any arguments. Mine just take the stats insance. but they can be anything
        public ToastCommand(Model.Model gameModelIn)
        {
            this.gameModel = gameModelIn;
        }

        // Visitor pattern execution
        public string Execute(string[] arguments)
        {
            String text = "";

            foreach (string s in arguments)
            {
                text += s + " ";
            }

            gameModel.gameView.Toast(text);

            return "Toasted: \"" + text + "\"";
        }
    }
}
