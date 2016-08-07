using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Roguelike.Util
{
    public class MathHelperHelper
    {
        public static Vector2 Vector2Normalize(Vector2 normalizeMe)
        {
            Vector2 result = Vector2.Normalize(normalizeMe);

            if (Double.IsNaN(result.X))
            {
                //Console.Error.WriteLine("Detected NaN: X");
                result.X = 0;
            }

            if (Double.IsNaN(result.Y))
            {
                //Console.Error.WriteLine("Detected NaN: Y");
                result.Y = 0;
            }

            return result;
        }

        public static void Vector2Normalize(ref Vector2 normalizeMe)
        {
            Vector2 result = Vector2.Normalize(normalizeMe);

            if (Double.IsNaN(result.X))
            {
                //Console.Error.WriteLine("Detected NaN: X");
                result.X = 0;
            }

            if (Double.IsNaN(result.Y))
            {
                //Console.Error.WriteLine("Detected NaN: Y");
                result.Y = 0;
            }

            normalizeMe = result;
        }

        public static Vector2 Vector2Int(Vector2 integerizeMe)
        {
            double x = Math.Round(integerizeMe.X, MidpointRounding.AwayFromZero);
            double y = Math.Round(integerizeMe.Y, MidpointRounding.AwayFromZero);

            return new Vector2((float)x, (float)y);
        }

        public static void Vector2Int(ref Vector2 integerizeMe)
        {
            integerizeMe.X = (float)Math.Round(integerizeMe.X, MidpointRounding.AwayFromZero);
            integerizeMe.Y = (float)Math.Round(integerizeMe.Y, MidpointRounding.AwayFromZero);
        }

        public static float CrossProduct(Vector2 v1, Vector2 v2) 
        {
            return (v1.X*v2.Y) - (v1.Y*v2.X);
        }
    }
}
