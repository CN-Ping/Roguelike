using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XNAGameConsole;
using Roguelike.Model;
using System.Text.RegularExpressions;

namespace Roguelike.Util.ConsoleCommands
{
    public class SetCommand : IConsoleCommand
    {
        private Model.Model gameModel;

        public string Name
        {
            get { return "set"; }
        }

        public string Description
        {
            get { return "set object.property 3: Sets the 'property' of 'object' to the specified value. ex: [set pc.speed 4]"; }
        }

        public SetCommand(Model.Model modelIn)
        {
            this.gameModel = modelIn;
        }

        public string Execute(string[] arguments)
        {
            if (arguments.Length != 2)
            {
                return "Not enough arguments. See help or text Elliot for more information.";
            }

            string [] split = Regex.Split(arguments[0], "\\.");

            if (split.Length != 2)
            {
                return "Malformed first argument. Should be object.property. e.g.: pc.health";
            }

            string obj = split[0];
            string prop = split[1];

            switch (obj)
            {
                case "pc":
                    return applyToPC(prop, float.Parse(arguments[1]));

                case "world":
                    return applyToWorld(prop, double.Parse(arguments[1]));

                default :
                    return "Unknown object. Property modification aborted.";
            }
        }

        public string applyToPC(string prop, float value)
        {
            switch (prop)
            {
                case "damage":
                    gameModel.currentLevel.playerStatsInstance.damage = value;
                    return "Player damage set to " + value;

                case "fireRate":
                    gameModel.currentLevel.playerStatsInstance.rateOfFire = value;
                    return "Player fire rate set to " + value;

                case "health":
                    gameModel.currentLevel.playerStatsInstance.health = value;
                    return "Player health set to " + value;

                case "lightCone":
                    gameModel.currentLevel.playerStatsInstance.lightIntensity = value;
                    return "Player light width set to " + value;

                case "lightRange":
                    gameModel.currentLevel.playerStatsInstance.LightRange = value;
                    return "Player light range set to " + value;

                case "luck":
                    gameModel.currentLevel.playerStatsInstance.luck = value;
                    return "Player luck set to " + value;

                case "maxHealth":
                    gameModel.currentLevel.playerStatsInstance.maxHealth = value;
                    return "Player maximum health set to " + value;

                case "shotSize":
                    gameModel.currentLevel.playerStatsInstance.shotSize = value;
                    return "Player shot size set to " + value;

                case "shotSpeed":
                    gameModel.currentLevel.playerStatsInstance.shotSpeed = value;
                    return "Player shot sped set to " + value;

                case "speed":
                    gameModel.currentLevel.playerStatsInstance.speed = value;
                    return "Player speed set to " + value;

                case "torchCount":
                    gameModel.currentLevel.playerStatsInstance.torchCount = (int)value;
                    return "Player torch count set to " + value;

                case "torchRadius":
                    gameModel.currentLevel.playerStatsInstance.torchRadius = (int)value;
                    return "Player torch radius set to " + value;

                default :
                    return "Unknown property. Property modification aborted.";
            }
        }

        public string applyToWorld(string prop, double value)
        {
            return "We don't actually have any world values implemented yet, but it's nice of you to show interest.";
        }
    }
}
