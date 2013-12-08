using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib.Maths.Shapes2D
{
    static class LineTools
    {
        // From Eric Jordan's convex decomposition library
        /// <summary>
        ///Check if the lines a0->a1 and b0->b1 cross.
        ///If they do, intersectionPoint will be filled
        ///with the point of crossing.
        ///
        ///Grazing lines should not return true.
        /// 
        /// </summary>
        /// <param name="a0"></param>
        /// <param name="a1"></param>
        /// <param name="b0"></param>
        /// <param name="b1"></param>
        /// <param name="intersectionPoint"></param>
        /// <returns></returns>
        public static bool LineIntersect2(Vector2 a0, Vector2 a1, Vector2 b0, Vector2 b1, out Vector2 intersectionPoint)
        {
            intersectionPoint = Vector2.Zero;

            if (a0 == b0 || a0 == b1 || a1 == b0 || a1 == b1)
                return false;

            float x1 = a0.X;
            float y1 = a0.Y;
            float x2 = a1.X;
            float y2 = a1.Y;
            float x3 = b0.X;
            float y3 = b0.Y;
            float x4 = b1.X;
            float y4 = b1.Y;

            //AABB early exit
            if (Math.Max(x1, x2) < Math.Min(x3, x4) || Math.Max(x3, x4) < Math.Min(x1, x2))
                return false;

            if (Math.Max(y1, y2) < Math.Min(y3, y4) || Math.Max(y3, y4) < Math.Min(y1, y2))
                return false;

            float ua = ((x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3));
            float ub = ((x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3));
            float denom = (y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1);
            if (Math.Abs(denom) < 1.192092896e-07f)
            {
                //Lines are too close to parallel to call
                return false;
            }
            ua /= denom;
            ub /= denom;

            if ((0 < ua) && (ua < 1) && (0 < ub) && (ub < 1))
            {
                intersectionPoint.X = (x1 + ua * (x2 - x1));
                intersectionPoint.Y = (y1 + ua * (y2 - y1));
                return true;
            }

            return false;
        }

    }
}
