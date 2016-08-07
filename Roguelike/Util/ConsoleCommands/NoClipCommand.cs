using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XNAGameConsole;
using Roguelike.Model;

namespace Roguelike.Util.ConsoleCommands
{
    public class NoClipCommand : IConsoleCommand
    {
        private Model.Model gameModel;

        public string Name
        {
            get { return "noclip"; }
        }

        public string Description
        {
            get { return "toggles noclip mode"; }
        }

        public NoClipCommand(Model.Model model)
        {
            this.gameModel = model;
        }

        public string Execute(string[] arguments)
        {
            gameModel.currentLevel.mainChar.stats.noClip = !gameModel.currentLevel.mainChar.stats.noClip;

            return "noclip mode: " + gameModel.currentLevel.mainChar.stats.noClip;
        }
    }
}
