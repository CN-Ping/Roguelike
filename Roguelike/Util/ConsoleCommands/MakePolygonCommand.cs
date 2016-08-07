using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XNAGameConsole;
using Roguelike.Model;
using Roguelike.Model.Lighting.Shape;
using Microsoft.Xna.Framework.Graphics;

namespace Roguelike.Util.ConsoleCommands
{
    public class MakePolygonCommand : IConsoleCommand
    {
        private Model.Model gameModel;

        public string Name
        {
            get { return "makePolygon"; }
        }

        public string Description
        {
            get { return "Make a polygon. Specify 'static' or 'dynamic' as first argument."; }
        }

        public MakePolygonCommand(Model.Model model)
        {
            this.gameModel = model;
        }

        public string Execute(string[] arguments)
        {
            if (arguments.GetLength(0) < 1)
            {
                return "please provide parameters";
            }

            bool isStatic = false;

            switch (arguments[0]) {
                case "static": 
                    isStatic = true;
                    break;

                case "dynamic":
                    isStatic = false;
                    break;

                default:
                    return "invalid location choice. Select 'static' or 'dynamic' as first argument";
            }

            if (arguments.GetLength(0) % 2 != 1)
            {
                return "invalid number of points";
            }

            List<Vector2> vertices = new List<Vector2>();
            
            for (int i = 1; i < arguments.GetLength(0); i += 2)
            {
                vertices.Add(new Vector2(int.Parse(arguments[i]), int.Parse(arguments[i + 1])));
            }

            gameModel.currentLevel.addGameObject(new PolygonShape(gameModel.currentLevel, gameModel.be, vertices, isStatic));
            return "Drew a polygon";
        }
    }
}
