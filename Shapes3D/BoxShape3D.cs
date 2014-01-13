using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib.Maths.Shapes3D
{
    public class BoxShape3D : Shape3D
    {
        Vector3 halfVector;
        Matrix3x3 rotation = Matrix3x3.Identity;
        Vector3 translation = Vector3.Zero;

        BoundingBox aabb;

        public BoxShape3D(Vector3 halfVector)
            : this(halfVector, Matrix3x3.Identity, Vector3.Zero)
        {

        }

        public BoxShape3D(Vector3 halfVector, Matrix3x3 rotation, Vector3 translation)
        {
            this.halfVector = halfVector;
            this.rotation = rotation;
            this.translation = translation;
            Update();
        }

        public float XHalfWidth { get { return halfVector.X; } set { halfVector.X = value; Update(); } }
        public float YHalfWidth { get { return halfVector.Y; } set { halfVector.Y = value; Update(); } }
        public float ZHalfWidth { get { return halfVector.Z; } set { halfVector.Z = value; Update(); } }

        public override BoundingBox AABB
        {
            get { return aabb; }
        }
        public override Vector3 Center
        {
            get { return translation; }
        }
        private void Update()
        {
            Matrix3x3 absRotate;
            rotation.Absolute(out absRotate);//halfVectorが正の時、絶対値を取ることで計算量を減らすことが出来る(bulletより)
            Vector3 rotatedHalfVector;
            Vector3.Transform(ref halfVector, ref absRotate, out rotatedHalfVector);
            Vector3.Add(ref translation, ref rotatedHalfVector, out aabb.Max);
            Vector3.Subtract(ref translation, ref rotatedHalfVector, out aabb.Min);
        }


        protected override bool TestRayDetail(ref Ray ray)
        {
            Matrix3x3 rotInvert;
            Vector3 pos1;
            Ray convertedRay;
            Vector3.Subtract(ref ray.Position, ref translation, out pos1);
            Matrix3x3.Invert(ref rotation, out rotInvert);
            Vector3.Transform(ref pos1, ref rotInvert, out convertedRay.Position);
            Vector3.Transform(ref ray.Direction, ref rotInvert, out convertedRay.Direction);
            BoundingBox box = new BoundingBox
            {
                Max = halfVector,
                Min = -halfVector
            };
            float? result;
            convertedRay.Intersects(ref box, out result);
            return result.HasValue;
        }
    }
}
