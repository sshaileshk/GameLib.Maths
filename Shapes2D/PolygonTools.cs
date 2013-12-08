using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib.Maths.Shapes2D
{
    public static class PolygonTools
    {
        /// <summary>
        /// Build vertices to represent an axis-aligned box.
        /// </summary>
        /// <param name="hx">the half-width.</param>
        /// <param name="hy">the half-height.</param>
        public static Vertices CreateRectangle(float hx, float hy)
        {
            Vertices vertices = new Vertices(4);
            vertices.Add(new Vector2(-hx, -hy));
            vertices.Add(new Vector2(hx, -hy));
            vertices.Add(new Vector2(hx, hy));
            vertices.Add(new Vector2(-hx, hy));

            return vertices;
        }
        /// <summary>
        /// Build vertices to represent an oriented box.
        /// </summary>
        /// <param name="hx">the half-width.</param>
        /// <param name="hy">the half-height.</param>
        /// <param name="center">the center of the box in local coordinates.</param>
        /// <param name="angle">the rotation of the box in local coordinates.</param>
        public static Vertices CreateRectangle(float hx, float hy, Vector2 center, float angle)
        {
            Vertices vertices = CreateRectangle(hx, hy);

            Transform xf = new Transform();
            xf.Position = center;
            xf.R.Set(angle);

            // Transform vertices
            for (int i = 0; i < 4; ++i)
            {
                vertices[i] = MathUtils.Multiply(ref xf, vertices[i]);
            }

            return vertices;
        }

    }
}
