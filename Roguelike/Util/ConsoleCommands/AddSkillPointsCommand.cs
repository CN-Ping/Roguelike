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
    public class AddSkillPointsCommand : IConsoleCommand
    {
        private Model.Model gameModel;

        // this is the actual command you will type
        public string Name
        {
            get { return "addSkillPoints"; }
        }

        // This is what shows up in the console window help
        public string Description
        {
            get { return "Adds more skill points"; }
        }

        // This constructor can take any arguments. Mine just take the stats insance. but they can be anything
        public AddSkillPointsCommand(Model.Model model)
        {
            this.gameModel = model;
        }

        // Visitor pattern execution
        public string Execute(string[] arguments)
        {
            int newSkillPoints = int.Parse(arguments[0]);
            gameModel.skillTree.skillPointsLeft += newSkillPoints;
            gameModel.skillTree.playerLevel += newSkillPoints;
            return "Added " + newSkillPoints + " skill points.";
        }
    }
}
