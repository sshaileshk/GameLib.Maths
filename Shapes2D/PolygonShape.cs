/*
* Farseer Physics Engine based on Box2D.XNA port:
* Copyright (c) 2010 Ian Qvist
* 
* Box2D.XNA port of Box2D:
* Copyright (c) 2009 Brandon Furtwangler, Nathan Furtwangler
*
* Original source Box2D:
* Copyright (c) 2006-2009 Erin Catto http://www.gphysics.com 
* 
* This software is provided 'as-is', without any express or implied 
* warranty.  In no event will the authors be held liable for any damages 
* arising from the use of this software. 
* Permission is granted to anyone to use this software for any purpose, 
* including commercial applications, and to alter it and redistribute it 
* freely, subject to the following restrictions: 
* 1. The origin of this software must not be misrepresented; you must not 
* claim that you wrote the original software. If you use this software 
* in a product, an acknowledgment in the product documentation would be 
* appreciated but is not required. 
* 2. Altered source versions must be plainly marked as such, and must not be 
* misrepresented as being the original software. 
* 3. This notice may not be removed or altered from any source distribution. 
*/

using System.Diagnostics;

namespace GameLib.Maths.Shapes2D
{
    /// <summary>
    /// Represents a simple non-selfintersecting convex polygon.
    /// If you want to have concave polygons, you will have to use the <see cref="BayazitDecomposer"/> or the <see cref="EarclipDecomposer"/>
    /// to decompose the concave polygon into 2 or more convex polygons.
    /// </summary>
    public class PolygonShape : Shape
    {
        public Vertices Normals;
        public Vertices Vertices;

        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonShape"/> class.
        /// </summary>
        /// <param name="vertices">The vertices.</param>
        /// <param name="density">The density.</param>
        public PolygonShape(Vertices vertices, float density)
            : base(density)
        {
            _radius = (2.0f * 0.005f);

            Set(vertices);
        }

        public PolygonShape(float density)
            : base(density)
        {
            _radius = (2.0f * 0.005f);
            Normals = new Vertices();
            Vertices = new Vertices();
        }

        internal PolygonShape()
            : base(0)
        {
            _radius = (2.0f * 0.005f);
            Normals = new Vertices();
            Vertices = new Vertices();
        }

        
        /// <summary>
        /// Copy vertices. This assumes the vertices define a convex polygon.
        /// It is assumed that the exterior is the the right of each edge.
        /// </summary>
        /// <param name="vertices">The vertices.</param>
        public void Set(Vertices vertices)
        {
            
            // Copy vertices.
            Vertices = new Vertices(vertices);

            Normals = new Vertices(vertices.Count);

            // Compute normals. Ensure the edges have non-zero length.
            for (int i = 0; i < vertices.Count; ++i)
            {
                int i1 = i;
                int i2 = i + 1 < vertices.Count ? i + 1 : 0;
                Vector2 edge = Vertices[i2] - Vertices[i1];
                Debug.Assert(edge.LengthSquared() > 1.192092896e-07f * 1.192092896e-07f);

                Vector2 temp = new Vector2(edge.Y, -edge.X);
                temp.Normalize();
                Normals.Add(temp);
            }

#if DEBUG
            // Ensure the polygon is convex and the interior
            // is to the left of each edge.
            for (int i = 0; i < Vertices.Count; ++i)
            {
                int i1 = i;
                int i2 = i + 1 < Vertices.Count ? i + 1 : 0;
                Vector2 edge = Vertices[i2] - Vertices[i1];

                for (int j = 0; j < vertices.Count; ++j)
                {
                    // Don't check vertices on the current edge.
                    if (j == i1 || j == i2)
                    {
                        continue;
                    }

                    Vector2 r = Vertices[j] - Vertices[i1];

                    // Your polygon is non-convex (it has an indentation) or
                    // has colinear edges.
                    float s = edge.X * r.Y - edge.Y * r.X;

                    Debug.Assert(s > 0.0f);
                }
            }
#endif

        }

        
        /// <summary>
        /// Build vertices to represent an axis-aligned box.
        /// </summary>
        /// <param name="halfWidth">The half-width.</param>
        /// <param name="halfHeight">The half-height.</param>
        public void SetAsBox(float halfWidth, float halfHeight)
        {
            Set(PolygonTools.CreateRectangle(halfWidth, halfHeight));
        }

        /// <summary>
        /// Build vertices to represent an oriented box.
        /// </summary>
        /// <param name="halfWidth">The half-width..</param>
        /// <param name="halfHeight">The half-height.</param>
        /// <param name="center">The center of the box in local coordinates.</param>
        /// <param name="angle">The rotation of the box in local coordinates.</param>
        public void SetAsBox(float halfWidth, float halfHeight, Vector2 center, float angle)
        {
            Set(PolygonTools.CreateRectangle(halfWidth, halfHeight, center, angle));
        }

        /// <summary>
        /// Test a point for containment in this shape. This only works for convex shapes.
        /// </summary>
        /// <param name="transform">The shape world transform.</param>
        /// <param name="point">a point in world coordinates.</param>
        /// <returns>True if the point is inside the shape</returns>
        public override bool TestPoint(ref Transform transform, ref Vector2 point)
        {
            Vector2 pLocal = MathUtils.MultiplyT(ref transform.R, point - transform.Position);

            for (int i = 0; i < Vertices.Count; ++i)
            {
                float dot = Vector2.Dot(Normals[i], pLocal - Vertices[i]);
                if (dot > 0.0f)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Cast a ray against a child shape.
        /// </summary>
        /// <param name="output">The ray-cast results.</param>
        /// <param name="input">The ray-cast input parameters.</param>
        /// <param name="transform">The transform to be applied to the shape.</param>
        /// <param name="childIndex">The child shape index.</param>
        /// <returns>True if the ray-cast hits the shape</returns>
        public override bool RayCast(out RayCastOutput output, ref RayCastInput input, ref Transform transform,
                                     int childIndex)
        {
            output = new RayCastOutput();

            // Put the ray into the polygon's frame of reference.
            Vector2 p1 = MathUtils.MultiplyT(ref transform.R, input.Point1 - transform.Position);
            Vector2 p2 = MathUtils.MultiplyT(ref transform.R, input.Point2 - transform.Position);
            Vector2 d = p2 - p1;

            float lower = 0.0f, upper = input.MaxFraction;

            int index = -1;

            for (int i = 0; i < Vertices.Count; ++i)
            {
                // p = p1 + a * d
                // dot(normal, p - v) = 0
                // dot(normal, p1 - v) + a * dot(normal, d) = 0
                float numerator = Vector2.Dot(Normals[i], Vertices[i] - p1);
                float denominator = Vector2.Dot(Normals[i], d);

                if (denominator == 0.0f)
                {
                    if (numerator < 0.0f)
                    {
                        return false;
                    }
                }
                else
                {
                    // Note: we want this predicate without division:
                    // lower < numerator / denominator, where denominator < 0
                    // Since denominator < 0, we have to flip the inequality:
                    // lower < numerator / denominator <==> denominator * lower > numerator.
                    if (denominator < 0.0f && numerator < lower * denominator)
                    {
                        // Increase lower.
                        // The segment enters this half-space.
                        lower = numerator / denominator;
                        index = i;
                    }
                    else if (denominator > 0.0f && numerator < upper * denominator)
                    {
                        // Decrease upper.
                        // The segment exits this half-space.
                        upper = numerator / denominator;
                    }
                }

                // The use of epsilon here causes the assert on lower to trip
                // in some cases. Apparently the use of epsilon was to make edge
                // shapes work, but now those are handled separately.
                //if (upper < lower - b2_epsilon)
                if (upper < lower)
                {
                    return false;
                }
            }

            Debug.Assert(0.0f <= lower && lower <= input.MaxFraction);

            if (index >= 0)
            {
                output.Fraction = lower;
                output.Normal = MathUtils.Multiply(ref transform.R, Normals[index]);
                return true;
            }

            return false;
        }

    }
}