using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib.Maths.Shapes3D
{
    /// <summary>
    /// 3Dオブジェクト型の基本
    /// </summary>
    public abstract class Shape3D
    {
        public abstract BoundingBox AABB { get; }

        public abstract Vector3 Center { get; }

        public bool TestRay(ref Ray ray)
        {
            float? result;
            AABB.Intersects(ref ray, out result);
            if (!result.HasValue)
                return false;
            return TestRayDetail(ref ray);
        }
        protected abstract bool TestRayDetail(ref Ray ray);
    }
}
