using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Roguelike.util
{
    public class LineIntersection
    {

        public LineIntersection()
        {

        }

        public bool IsIntersection(Vector2 aStart, Vector2 aEnd, Vector2 bStart, Vector2 bEnd)
        {
            Nullable<Vector2> check = FindIntersection(aStart, aEnd, bStart, bEnd);
            if (check == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //Simple ray-casting
        public bool PointInsideConvexPolygon(Vector2 point, List<Vector2> polygonPoints)
        {

            float maxX = 0;
            float maxY = 0;

            /* Find the max x and y of the shape */
            for (int i = 0; i < polygonPoints.Count; i++ )
            {
                if (polygonPoints[i].X > maxX)
                {
                    maxX = polygonPoints[i].X;
                }
                if(polygonPoints[i].Y > maxY)
                {
                    maxY = polygonPoints[i].Y;
                }

            }

            /* Select a point we know is outside the polygon */
            Vector2 outsidePoint = new Vector2(maxX + 20, maxY + 10);

            /* Check how many times the line point to outsidePoint intersects the polygon */
            int intersectionCount = 0;
            HashSet<Nullable<Vector2>> intersections = new HashSet<Nullable<Vector2>>();
            for (int i = 0; i < polygonPoints.Count; i++ )
            {
                int secondI = i + 1;
                if (i == polygonPoints.Count - 1)
                {
                    secondI = 0;
                }

                Nullable<Vector2> intersects = FindIntersection(outsidePoint, point, polygonPoints[i], polygonPoints[secondI]);

                if (intersects != null)
                {
                    if (intersections.Contains(intersects) == false)
                    {
                        intersections.Add(intersects);
                        intersectionCount += 1;
                    }
                }
            }

            bool inside = false;
            if (intersectionCount % 2 == 0)
            {
                // If even # of intersections, the point is outside
                inside = false;
            }
            else
            {
                //if odd # of intersections, the point is inside
                inside = true;
            }

            return inside;
        }

        private Nullable<Vector2> FindIntersection(Vector2 aStart, Vector2 aEnd, Vector2 bStart, Vector2 bEnd)
        {
            //float A1 = aStart.Y-aEnd.Y;
            float A1 = aEnd.Y - aStart.Y;
            float B1 = aStart.X - aEnd.X;
            float C1 = A1 * aStart.X + B1 * aStart.Y;

            //float A2 = bStart.Y-bEnd.Y;
            float A2 = bEnd.Y - bStart.Y;
            float B2 = bStart.X - bEnd.X;
            float C2 = A2 * bStart.X + B2 * bStart.Y;

            /* Check if it is parallel */
            float delta = A1 * B2 - A2 * B1;
            if (delta == 0)
            {
                return null;
            }

            /* Get the intersection */
            Vector2 intersection = new Vector2((B2 * C1 - B1 * C2) / delta, (A1 * C2 - A2 * C1) / delta);

            /* See if the intersection is on one of the lines */
            if ((intersection.X <= aStart.X && intersection.X >= aEnd.X) || (intersection.X >= aStart.X && intersection.X <= aEnd.X))
            {
                if ((intersection.Y <= aStart.Y && intersection.Y >= aEnd.Y) || (intersection.Y >= aStart.Y && intersection.Y <= aEnd.Y))
                {
                    if ((intersection.X <= bStart.X && intersection.X >= bEnd.X) || (intersection.X >= bStart.X && intersection.X <= bEnd.X))
                    {
                        if ((intersection.Y <= bStart.Y && intersection.Y >= bEnd.Y) || (intersection.Y >= bStart.Y && intersection.Y <= bEnd.Y))
                        {
                            return intersection;
                        }
                    }
                }
            }

            return null;
        }

    }
}
