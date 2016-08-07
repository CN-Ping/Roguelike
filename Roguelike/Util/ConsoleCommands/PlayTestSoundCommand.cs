using Roguelike.Sound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNAGameConsole;

namespace Roguelike.Util.ConsoleCommands
{
    class PlayTestSoundCommand : IConsoleCommand
    {
        private Model.Model gameModel;

        public string Name
        {
            get { return "playSound"; }
        }

        // This is what shows up in the console window help
        public string Description
        {
            get { return "plays a test sound"; }
        }

        // This constructor can take any arguments. Mine just take the stats insance. but they can be anything
        public PlayTestSoundCommand(Model.Model model)
        {
            gameModel = model;
        }

        // Visitor pattern execution
        public string Execute(string[] arguments)
        {
            TestSound test = new TestSound(gameModel.Game);
            test.LoadContent();
            test.PlaySound();
            return "Playing the test sound";
        }
    }
}
