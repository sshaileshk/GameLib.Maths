using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GameLib.Maths
{
    [DataContract]
    public struct Matrix3x3 : IEquatable<Matrix3x3>
    {
        private static Matrix3x3 identity = new Matrix3x3(1f, 0f, 0f,
                                                    0f, 1f, 0f,
                                                    0f, 0f, 1f);
        
        
        [DataMember]
        public float M11;

        [DataMember]
        public float M12;

        [DataMember]
        public float M13;

        [DataMember]
        public float M21;

        [DataMember]
        public float M22;

        [DataMember]
        public float M23;

        [DataMember]
        public float M31;

        [DataMember]
        public float M32;

        [DataMember]
        public float M33;

        
        
        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return M11;
                    case 1: return M12;
                    case 2: return M13;
                    case 3: return M21;
                    case 4: return M22;
                    case 5: return M23;
                    case 6: return M31;
                    case 7: return M32;
                    case 8: return M33;
                }
                throw new ArgumentOutOfRangeException();
            }

            set
            {
                switch (index)
                {
                    case 0: M11 = value; break;
                    case 1: M12 = value; break;
                    case 2: M13 = value; break;
                    case 3: M21 = value; break;
                    case 4: M22 = value; break;
                    case 5: M23 = value; break;
                    case 6: M31 = value; break;
                    case 7: M32 = value; break;
                    case 8: M33 = value; break;
                    default: throw new ArgumentOutOfRangeException();
                }
            }
        }

        public float this[int row, int column]
        {
            get
            {
                return this[(row * 3) + column];
            }

            set
            {
                this[(row * 3) + column] = value;
            }
        }


        public Vector3 Backward
        {
            get
            {
                return new Vector3(this.M31, this.M32, this.M33);
            }
            set
            {
                this.M31 = value.X;
                this.M32 = value.Y;
                this.M33 = value.Z;
            }
        }


        public Vector3 Down
        {
            get
            {
                return new Vector3(-this.M21, -this.M22, -this.M23);
            }
            set
            {
                this.M21 = -value.X;
                this.M22 = -value.Y;
                this.M23 = -value.Z;
            }
        }


        public Vector3 Forward
        {
            get
            {
                return new Vector3(-this.M31, -this.M32, -this.M33);
            }
            set
            {
                this.M31 = -value.X;
                this.M32 = -value.Y;
                this.M33 = -value.Z;
            }
        }


        public static Matrix3x3 Identity
        {
            get { return identity; }
        }


        

        public Vector3 Left
        {
            get
            {
                return new Vector3(-this.M11, -this.M12, -this.M13);
            }
            set
            {
                this.M11 = -value.X;
                this.M12 = -value.Y;
                this.M13 = -value.Z;
            }
        }


        public Vector3 Right
        {
            get
            {
                return new Vector3(this.M11, this.M12, this.M13);
            }
            set
            {
                this.M11 = value.X;
                this.M12 = value.Y;
                this.M13 = value.Z;
            }
        }


        public Vector3 Up
        {
            get
            {
                return new Vector3(this.M21, this.M22, this.M23);
            }
            set
            {
                this.M21 = value.X;
                this.M22 = value.Y;
                this.M23 = value.Z;
            }
        }
        


        public Matrix3x3(float m11, float m12, float m13, float m21, float m22, float m23, float m31,
                      float m32, float m33)
        {
            this.M11 = m11;
            this.M12 = m12;
            this.M13 = m13;
            this.M21 = m21;
            this.M22 = m22;
            this.M23 = m23;
            this.M31 = m31;
            this.M32 = m32;
            this.M33 = m33;
        }


        // required for OpenGL 2.0 projection matrix stuff
        // TODO: have this work correctly for 3x3 Matrices. Needs to return
        // a float[9] for a 3x3, and a float[16] for a 4x4
        public static float[] ToFloatArray(Matrix3x3 mat)
        {
            float[] matarray = {
                                    mat.M11, mat.M12, mat.M13,
                                    mat.M21, mat.M22, mat.M23,
                                    mat.M31, mat.M32, mat.M33,
                                };
            return matarray;
        }
        public static Matrix3x3 Add(Matrix3x3 matrix1, Matrix3x3 matrix2)
        {
            matrix1.M11 += matrix2.M11;
            matrix1.M12 += matrix2.M12;
            matrix1.M13 += matrix2.M13;
            matrix1.M21 += matrix2.M21;
            matrix1.M22 += matrix2.M22;
            matrix1.M23 += matrix2.M23;
            matrix1.M31 += matrix2.M31;
            matrix1.M32 += matrix2.M32;
            matrix1.M33 += matrix2.M33;
            return matrix1;
        }


        public static void Add(ref Matrix3x3 matrix1, ref Matrix3x3 matrix2, out Matrix3x3 result)
        {
            result.M11 = matrix1.M11 + matrix2.M11;
            result.M12 = matrix1.M12 + matrix2.M12;
            result.M13 = matrix1.M13 + matrix2.M13;
            result.M21 = matrix1.M21 + matrix2.M21;
            result.M22 = matrix1.M22 + matrix2.M22;
            result.M23 = matrix1.M23 + matrix2.M23;
            result.M31 = matrix1.M31 + matrix2.M31;
            result.M32 = matrix1.M32 + matrix2.M32;
            result.M33 = matrix1.M33 + matrix2.M33;

        }

        public static Matrix3x3 CreateFromAxisAngle(Vector3 axis, float angle)
        {
            Matrix3x3 result;
            CreateFromAxisAngle(ref axis, angle, out result);
            return result;
        }


        public static void CreateFromAxisAngle(ref Vector3 axis, float angle, out Matrix3x3 result)
        {
            float x = axis.X;
            float y = axis.Y;
            float z = axis.Z;
            float num2 = (float)Math.Sin((double)angle);
            float num = (float)Math.Cos((double)angle);
            float num11 = x * x;
            float num10 = y * y;
            float num9 = z * z;
            float num8 = x * y;
            float num7 = x * z;
            float num6 = y * z;
            result.M11 = num11 + (num * (1f - num11));
            result.M12 = (num8 - (num * num8)) + (num2 * z);
            result.M13 = (num7 - (num * num7)) - (num2 * y);
            result.M21 = (num8 - (num * num8)) - (num2 * z);
            result.M22 = num10 + (num * (1f - num10));
            result.M23 = (num6 - (num * num6)) + (num2 * x);
            result.M31 = (num7 - (num * num7)) + (num2 * y);
            result.M32 = (num6 - (num * num6)) - (num2 * x);
            result.M33 = num9 + (num * (1f - num9));
        }


        public static Matrix3x3 CreateFromQuaternion(Quaternion quaternion)
        {
            Matrix3x3 result;
            CreateFromQuaternion(ref quaternion, out result);
            return result;
        }


        public static void CreateFromQuaternion(ref Quaternion quaternion, out Matrix3x3 result)
        {
            float num9 = quaternion.X * quaternion.X;
            float num8 = quaternion.Y * quaternion.Y;
            float num7 = quaternion.Z * quaternion.Z;
            float num6 = quaternion.X * quaternion.Y;
            float num5 = quaternion.Z * quaternion.W;
            float num4 = quaternion.Z * quaternion.X;
            float num3 = quaternion.Y * quaternion.W;
            float num2 = quaternion.Y * quaternion.Z;
            float num = quaternion.X * quaternion.W;
            result.M11 = 1f - (2f * (num8 + num7));
            result.M12 = 2f * (num6 + num5);
            result.M13 = 2f * (num4 - num3);
            result.M21 = 2f * (num6 - num5);
            result.M22 = 1f - (2f * (num7 + num9));
            result.M23 = 2f * (num2 + num);
            result.M31 = 2f * (num4 + num3);
            result.M32 = 2f * (num2 - num);
            result.M33 = 1f - (2f * (num8 + num9));
        }

        public static Matrix3x3 CreateFromYawPitchRoll(float yaw, float pitch, float roll)
        {
            Matrix3x3 matrix;
            CreateFromYawPitchRoll(yaw, pitch, roll, out matrix);
            return matrix;
        }

        public static void CreateFromYawPitchRoll(float yaw, float pitch, float roll, out Matrix3x3 result)
        {
            Quaternion quaternion;
            Quaternion.CreateFromYawPitchRoll(yaw, pitch, roll, out quaternion);
            CreateFromQuaternion(ref quaternion, out result);
        }


        public static Matrix3x3 CreateRotationX(float radians)
        {
            Matrix3x3 result;
            CreateRotationX(radians, out result);
            return result;
        }


        public static void CreateRotationX(float radians, out Matrix3x3 result)
        {
            result = Matrix3x3.Identity;

            var val1 = (float)Math.Cos(radians);
            var val2 = (float)Math.Sin(radians);

            result.M22 = val1;
            result.M23 = val2;
            result.M32 = -val2;
            result.M33 = val1;
        }

        public static Matrix3x3 CreateRotationY(float radians)
        {
            Matrix3x3 result;
            CreateRotationY(radians, out result);
            return result;
        }


        public static void CreateRotationY(float radians, out Matrix3x3 result)
        {
            result = Matrix3x3.Identity;

            var val1 = (float)Math.Cos(radians);
            var val2 = (float)Math.Sin(radians);

            result.M11 = val1;
            result.M13 = -val2;
            result.M31 = val2;
            result.M33 = val1;
        }


        public static Matrix3x3 CreateRotationZ(float radians)
        {
            Matrix3x3 result;
            CreateRotationZ(radians, out result);
            return result;
        }


        public static void CreateRotationZ(float radians, out Matrix3x3 result)
        {
            result = Matrix3x3.Identity;

            var val1 = (float)Math.Cos(radians);
            var val2 = (float)Math.Sin(radians);

            result.M11 = val1;
            result.M12 = val2;
            result.M21 = -val2;
            result.M22 = val1;
        }


        




        public bool Decompose(out Vector3 scale, out Quaternion rotation)
        {
            scale.X = (float)Math.Sqrt(this.M11 * this.M11 + this.M12 * this.M12 + this.M13 * this.M13);
            scale.Y = (float)Math.Sqrt(this.M21 * this.M21 + this.M22 * this.M22 + this.M23 * this.M23);
            scale.Z = (float)Math.Sqrt(this.M31 * this.M31 + this.M32 * this.M32 + this.M33 * this.M33);

            if (scale.X == 0.0 || scale.Y == 0.0 || scale.Z == 0.0)
            {
                rotation = Quaternion.Identity;
                return false;
            }

            Matrix m1 = new Matrix(this.M11 / scale.X, M12 / scale.X, M13 / scale.X, 0,
                                   this.M21 / scale.Y, M22 / scale.Y, M23 / scale.Y, 0,
                                   this.M31 / scale.Z, M32 / scale.Z, M33 / scale.Z, 0,
                                   0, 0, 0, 1);

            rotation = Quaternion.CreateFromRotationMatrix(m1);
            return true;
        }

        public static Matrix3x3 Divide(Matrix3x3 matrix1, Matrix3x3 matrix2)
        {
            matrix1.M11 = matrix1.M11 / matrix2.M11;
            matrix1.M12 = matrix1.M12 / matrix2.M12;
            matrix1.M13 = matrix1.M13 / matrix2.M13;
            matrix1.M21 = matrix1.M21 / matrix2.M21;
            matrix1.M22 = matrix1.M22 / matrix2.M22;
            matrix1.M23 = matrix1.M23 / matrix2.M23;
            matrix1.M31 = matrix1.M31 / matrix2.M31;
            matrix1.M32 = matrix1.M32 / matrix2.M32;
            matrix1.M33 = matrix1.M33 / matrix2.M33;
            return matrix1;
        }


        public static void Divide(ref Matrix3x3 matrix1, ref Matrix3x3 matrix2, out Matrix3x3 result)
        {
            result.M11 = matrix1.M11 / matrix2.M11;
            result.M12 = matrix1.M12 / matrix2.M12;
            result.M13 = matrix1.M13 / matrix2.M13;
            result.M21 = matrix1.M21 / matrix2.M21;
            result.M22 = matrix1.M22 / matrix2.M22;
            result.M23 = matrix1.M23 / matrix2.M23;
            result.M31 = matrix1.M31 / matrix2.M31;
            result.M32 = matrix1.M32 / matrix2.M32;
            result.M33 = matrix1.M33 / matrix2.M33;
        }


        public static Matrix3x3 Divide(Matrix3x3 matrix1, float divider)
        {
            float num = 1f / divider;
            matrix1.M11 = matrix1.M11 * num;
            matrix1.M12 = matrix1.M12 * num;
            matrix1.M13 = matrix1.M13 * num;
            matrix1.M21 = matrix1.M21 * num;
            matrix1.M22 = matrix1.M22 * num;
            matrix1.M23 = matrix1.M23 * num;
            matrix1.M31 = matrix1.M31 * num;
            matrix1.M32 = matrix1.M32 * num;
            matrix1.M33 = matrix1.M33 * num;
            return matrix1;
        }


        public static void Divide(ref Matrix3x3 matrix1, float divider, out Matrix3x3 result)
        {
            float num = 1f / divider;
            result.M11 = matrix1.M11 * num;
            result.M12 = matrix1.M12 * num;
            result.M13 = matrix1.M13 * num;
            result.M21 = matrix1.M21 * num;
            result.M22 = matrix1.M22 * num;
            result.M23 = matrix1.M23 * num;
            result.M31 = matrix1.M31 * num;
            result.M32 = matrix1.M32 * num;
            result.M33 = matrix1.M33 * num;
        }


        public bool Equals(Matrix3x3 other)
        {
            return ((((((this.M11 == other.M11) && (this.M22 == other.M22)) && ((this.M33 == other.M33))) && (((this.M12 == other.M12) && (this.M13 == other.M13)) && ((this.M21 == other.M21)))) && ((((this.M23 == other.M23)) && ((this.M31 == other.M31) && (this.M32 == other.M32))))));
        }


        public override bool Equals(object obj)
        {
            bool flag = false;
            if (obj is Matrix3x3)
            {
                flag = this.Equals((Matrix3x3)obj);
            }
            return flag;
        }


        public override int GetHashCode()
        {
            return (((((((((((((((this.M11.GetHashCode() + this.M12.GetHashCode()) + this.M13.GetHashCode())) + this.M21.GetHashCode()) + this.M22.GetHashCode()) + this.M23.GetHashCode())) + this.M31.GetHashCode()) + this.M32.GetHashCode()) + this.M33.GetHashCode()))))));
        }

        public static void Invert(ref Matrix3x3 matrix, out Matrix3x3 result)
        {
            float num1 = matrix.M11;
            float num2 = matrix.M12;
            float num3 = matrix.M13;
            float num4 = 0;
            float num5 = matrix.M21;
            float num6 = matrix.M22;
            float num7 = matrix.M23;
            float num8 = 0;
            float num9 = matrix.M31;
            float num10 = matrix.M32;
            float num11 = matrix.M33;
            float num12 = 0;
            float num13 = 0;
            float num14 = 0;
            float num15 = 0;
            float num16 = 1;
            float num17 = (float)((double)num11 * (double)num16 - (double)num12 * (double)num15);
            float num18 = (float)((double)num10 * (double)num16 - (double)num12 * (double)num14);
            float num19 = (float)((double)num10 * (double)num15 - (double)num11 * (double)num14);
            float num20 = (float)((double)num9 * (double)num16 - (double)num12 * (double)num13);
            float num21 = (float)((double)num9 * (double)num15 - (double)num11 * (double)num13);
            float num22 = (float)((double)num9 * (double)num14 - (double)num10 * (double)num13);
            float num23 = (float)((double)num6 * (double)num17 - (double)num7 * (double)num18 + (double)num8 * (double)num19);
            float num24 = (float)-((double)num5 * (double)num17 - (double)num7 * (double)num20 + (double)num8 * (double)num21);
            float num25 = (float)((double)num5 * (double)num18 - (double)num6 * (double)num20 + (double)num8 * (double)num22);
            float num26 = (float)-((double)num5 * (double)num19 - (double)num6 * (double)num21 + (double)num7 * (double)num22);
            float num27 = (float)(1.0 / ((double)num1 * (double)num23 + (double)num2 * (double)num24 + (double)num3 * (double)num25 + (double)num4 * (double)num26));

            result.M11 = num23 * num27;
            result.M21 = num24 * num27;
            result.M31 = num25 * num27;
            result.M12 = (float)-((double)num2 * (double)num17 - (double)num3 * (double)num18 + (double)num4 * (double)num19) * num27;
            result.M22 = (float)((double)num1 * (double)num17 - (double)num3 * (double)num20 + (double)num4 * (double)num21) * num27;
            result.M32 = (float)-((double)num1 * (double)num18 - (double)num2 * (double)num20 + (double)num4 * (double)num22) * num27;
            float num28 = (float)((double)num7 * (double)num16 - (double)num8 * (double)num15);
            float num29 = (float)((double)num6 * (double)num16 - (double)num8 * (double)num14);
            float num30 = (float)((double)num6 * (double)num15 - (double)num7 * (double)num14);
            float num31 = (float)((double)num5 * (double)num16 - (double)num8 * (double)num13);
            float num32 = (float)((double)num5 * (double)num15 - (double)num7 * (double)num13);
            float num33 = (float)((double)num5 * (double)num14 - (double)num6 * (double)num13);
            result.M13 = (float)((double)num2 * (double)num28 - (double)num3 * (double)num29 + (double)num4 * (double)num30) * num27;
            result.M23 = (float)-((double)num1 * (double)num28 - (double)num3 * (double)num31 + (double)num4 * (double)num32) * num27;
            result.M33 = (float)((double)num1 * (double)num29 - (double)num2 * (double)num31 + (double)num4 * (double)num33) * num27;
        }
        

        public static Matrix3x3 Multiply(Matrix3x3 matrix1, Matrix3x3 matrix2)
        {
            var m11 = (((matrix1.M11 * matrix2.M11) + (matrix1.M12 * matrix2.M21)) + (matrix1.M13 * matrix2.M31));
            var m12 = (((matrix1.M11 * matrix2.M12) + (matrix1.M12 * matrix2.M22)) + (matrix1.M13 * matrix2.M32));
            var m13 = (((matrix1.M11 * matrix2.M13) + (matrix1.M12 * matrix2.M23)) + (matrix1.M13 * matrix2.M33));
            var m21 = (((matrix1.M21 * matrix2.M11) + (matrix1.M22 * matrix2.M21)) + (matrix1.M23 * matrix2.M31));
            var m22 = (((matrix1.M21 * matrix2.M12) + (matrix1.M22 * matrix2.M22)) + (matrix1.M23 * matrix2.M32));
            var m23 = (((matrix1.M21 * matrix2.M13) + (matrix1.M22 * matrix2.M23)) + (matrix1.M23 * matrix2.M33));
            var m31 = (((matrix1.M31 * matrix2.M11) + (matrix1.M32 * matrix2.M21)) + (matrix1.M33 * matrix2.M31));
            var m32 = (((matrix1.M31 * matrix2.M12) + (matrix1.M32 * matrix2.M22)) + (matrix1.M33 * matrix2.M32));
            var m33 = (((matrix1.M31 * matrix2.M13) + (matrix1.M32 * matrix2.M23)) + (matrix1.M33 * matrix2.M33));
            matrix1.M11 = m11;
            matrix1.M12 = m12;
            matrix1.M13 = m13;
            matrix1.M21 = m21;
            matrix1.M22 = m22;
            matrix1.M23 = m23;
            matrix1.M31 = m31;
            matrix1.M32 = m32;
            matrix1.M33 = m33;
            return matrix1;
        }


        public static void Multiply(ref Matrix3x3 matrix1, ref Matrix3x3 matrix2, out Matrix3x3 result)
        {
            var m11 = (((matrix1.M11 * matrix2.M11) + (matrix1.M12 * matrix2.M21)) + (matrix1.M13 * matrix2.M31));
            var m12 = (((matrix1.M11 * matrix2.M12) + (matrix1.M12 * matrix2.M22)) + (matrix1.M13 * matrix2.M32));
            var m13 = (((matrix1.M11 * matrix2.M13) + (matrix1.M12 * matrix2.M23)) + (matrix1.M13 * matrix2.M33));
            var m21 = (((matrix1.M21 * matrix2.M11) + (matrix1.M22 * matrix2.M21)) + (matrix1.M23 * matrix2.M31));
            var m22 = (((matrix1.M21 * matrix2.M12) + (matrix1.M22 * matrix2.M22)) + (matrix1.M23 * matrix2.M32));
            var m23 = (((matrix1.M21 * matrix2.M13) + (matrix1.M22 * matrix2.M23)) + (matrix1.M23 * matrix2.M33));
            var m31 = (((matrix1.M31 * matrix2.M11) + (matrix1.M32 * matrix2.M21)) + (matrix1.M33 * matrix2.M31));
            var m32 = (((matrix1.M31 * matrix2.M12) + (matrix1.M32 * matrix2.M22)) + (matrix1.M33 * matrix2.M32));
            var m33 = (((matrix1.M31 * matrix2.M13) + (matrix1.M32 * matrix2.M23)) + (matrix1.M33 * matrix2.M33));
            result.M11 = m11;
            result.M12 = m12;
            result.M13 = m13;
            result.M21 = m21;
            result.M22 = m22;
            result.M23 = m23;
            result.M31 = m31;
            result.M32 = m32;
            result.M33 = m33;
        }

        public static Matrix3x3 Multiply(Matrix3x3 matrix1, float factor)
        {
            matrix1.M11 *= factor;
            matrix1.M12 *= factor;
            matrix1.M13 *= factor;
            matrix1.M21 *= factor;
            matrix1.M22 *= factor;
            matrix1.M23 *= factor;
            matrix1.M31 *= factor;
            matrix1.M32 *= factor;
            matrix1.M33 *= factor;
            return matrix1;
        }


        public static void Multiply(ref Matrix3x3 matrix1, float factor, out Matrix3x3 result)
        {
            result.M11 = matrix1.M11 * factor;
            result.M12 = matrix1.M12 * factor;
            result.M13 = matrix1.M13 * factor;
            result.M21 = matrix1.M21 * factor;
            result.M22 = matrix1.M22 * factor;
            result.M23 = matrix1.M23 * factor;
            result.M31 = matrix1.M31 * factor;
            result.M32 = matrix1.M32 * factor;
            result.M33 = matrix1.M33 * factor;
        }


        public static Matrix3x3 Negate(Matrix3x3 matrix)
        {
            matrix.M11 = -matrix.M11;
            matrix.M12 = -matrix.M12;
            matrix.M13 = -matrix.M13;
            matrix.M21 = -matrix.M21;
            matrix.M22 = -matrix.M22;
            matrix.M23 = -matrix.M23;
            matrix.M31 = -matrix.M31;
            matrix.M32 = -matrix.M32;
            matrix.M33 = -matrix.M33;
            return matrix;
        }


        public static void Negate(ref Matrix3x3 matrix, out Matrix3x3 result)
        {
            result.M11 = -matrix.M11;
            result.M12 = -matrix.M12;
            result.M13 = -matrix.M13;
            result.M21 = -matrix.M21;
            result.M22 = -matrix.M22;
            result.M23 = -matrix.M23;
            result.M31 = -matrix.M31;
            result.M32 = -matrix.M32;
            result.M33 = -matrix.M33;
        }


        public static Matrix3x3 operator +(Matrix3x3 matrix1, Matrix3x3 matrix2)
        {
            Matrix3x3.Add(ref matrix1, ref matrix2, out matrix1);
            return matrix1;
        }


        public static Matrix3x3 operator /(Matrix3x3 matrix1, Matrix3x3 matrix2)
        {
            matrix1.M11 = matrix1.M11 / matrix2.M11;
            matrix1.M12 = matrix1.M12 / matrix2.M12;
            matrix1.M13 = matrix1.M13 / matrix2.M13;
            matrix1.M21 = matrix1.M21 / matrix2.M21;
            matrix1.M22 = matrix1.M22 / matrix2.M22;
            matrix1.M23 = matrix1.M23 / matrix2.M23;
            matrix1.M31 = matrix1.M31 / matrix2.M31;
            matrix1.M32 = matrix1.M32 / matrix2.M32;
            matrix1.M33 = matrix1.M33 / matrix2.M33;
            return matrix1;
        }


        public static Matrix3x3 operator /(Matrix3x3 matrix, float divider)
        {
            float num = 1f / divider;
            matrix.M11 = matrix.M11 * num;
            matrix.M12 = matrix.M12 * num;
            matrix.M13 = matrix.M13 * num;
            matrix.M21 = matrix.M21 * num;
            matrix.M22 = matrix.M22 * num;
            matrix.M23 = matrix.M23 * num;
            matrix.M31 = matrix.M31 * num;
            matrix.M32 = matrix.M32 * num;
            matrix.M33 = matrix.M33 * num;
            return matrix;
        }


        public static bool operator ==(Matrix3x3 matrix1, Matrix3x3 matrix2)
        {
            return (
                matrix1.M11 == matrix2.M11 &&
                matrix1.M12 == matrix2.M12 &&
                matrix1.M13 == matrix2.M13 &&
                matrix1.M21 == matrix2.M21 &&
                matrix1.M22 == matrix2.M22 &&
                matrix1.M23 == matrix2.M23 &&
                matrix1.M31 == matrix2.M31 &&
                matrix1.M32 == matrix2.M32 &&
                matrix1.M33 == matrix2.M33
                );
        }


        public static bool operator !=(Matrix3x3 matrix1, Matrix3x3 matrix2)
        {
            return (
                matrix1.M11 != matrix2.M11 ||
                matrix1.M12 != matrix2.M12 ||
                matrix1.M13 != matrix2.M13 ||
                matrix1.M21 != matrix2.M21 ||
                matrix1.M22 != matrix2.M22 ||
                matrix1.M23 != matrix2.M23 ||
                matrix1.M31 != matrix2.M31 ||
                matrix1.M32 != matrix2.M32 ||
                matrix1.M33 != matrix2.M33
                );
        }


        public static Matrix3x3 operator *(Matrix3x3 matrix1, Matrix3x3 matrix2)
        {
            Matrix3x3 result;
            Matrix3x3.Multiply(ref matrix1, ref matrix2, out result);
            return result;
        }


        public static Matrix3x3 operator *(Matrix3x3 matrix, float scaleFactor)
        {
            matrix.M11 = matrix.M11 * scaleFactor;
            matrix.M12 = matrix.M12 * scaleFactor;
            matrix.M13 = matrix.M13 * scaleFactor;
            matrix.M21 = matrix.M21 * scaleFactor;
            matrix.M22 = matrix.M22 * scaleFactor;
            matrix.M23 = matrix.M23 * scaleFactor;
            matrix.M31 = matrix.M31 * scaleFactor;
            matrix.M32 = matrix.M32 * scaleFactor;
            matrix.M33 = matrix.M33 * scaleFactor;
            return matrix;
        }


        public static Matrix3x3 operator -(Matrix3x3 matrix1, Matrix3x3 matrix2)
        {
            matrix1.M11 = matrix1.M11 - matrix2.M11;
            matrix1.M12 = matrix1.M12 - matrix2.M12;
            matrix1.M13 = matrix1.M13 - matrix2.M13;
            matrix1.M21 = matrix1.M21 - matrix2.M21;
            matrix1.M22 = matrix1.M22 - matrix2.M22;
            matrix1.M23 = matrix1.M23 - matrix2.M23;
            matrix1.M31 = matrix1.M31 - matrix2.M31;
            matrix1.M32 = matrix1.M32 - matrix2.M32;
            matrix1.M33 = matrix1.M33 - matrix2.M33;
            return matrix1;
        }


        public static Matrix3x3 operator -(Matrix3x3 matrix)
        {
            matrix.M11 = -matrix.M11;
            matrix.M12 = -matrix.M12;
            matrix.M13 = -matrix.M13;
            matrix.M21 = -matrix.M21;
            matrix.M22 = -matrix.M22;
            matrix.M23 = -matrix.M23;
            matrix.M31 = -matrix.M31;
            matrix.M32 = -matrix.M32;
            matrix.M33 = -matrix.M33;
            return matrix;
        }


        public static Matrix3x3 Subtract(Matrix3x3 matrix1, Matrix3x3 matrix2)
        {
            matrix1.M11 = matrix1.M11 - matrix2.M11;
            matrix1.M12 = matrix1.M12 - matrix2.M12;
            matrix1.M13 = matrix1.M13 - matrix2.M13;
            matrix1.M21 = matrix1.M21 - matrix2.M21;
            matrix1.M22 = matrix1.M22 - matrix2.M22;
            matrix1.M23 = matrix1.M23 - matrix2.M23;
            matrix1.M31 = matrix1.M31 - matrix2.M31;
            matrix1.M32 = matrix1.M32 - matrix2.M32;
            matrix1.M33 = matrix1.M33 - matrix2.M33;
            return matrix1;
        }


        public static void Subtract(ref Matrix3x3 matrix1, ref Matrix3x3 matrix2, out Matrix3x3 result)
        {
            result.M11 = matrix1.M11 - matrix2.M11;
            result.M12 = matrix1.M12 - matrix2.M12;
            result.M13 = matrix1.M13 - matrix2.M13;
            result.M21 = matrix1.M21 - matrix2.M21;
            result.M22 = matrix1.M22 - matrix2.M22;
            result.M23 = matrix1.M23 - matrix2.M23;
            result.M31 = matrix1.M31 - matrix2.M31;
            result.M32 = matrix1.M32 - matrix2.M32;
            result.M33 = matrix1.M33 - matrix2.M33;
        }


        public override string ToString()
        {
            return "{" + String.Format("M11:{0} M12:{1} M13:{2}", M11, M12, M13) + "}"
                + " {" + String.Format("M21:{0} M22:{1} M23:{2}", M21, M22, M23) + "}"
                + " {" + String.Format("M31:{0} M32:{1} M33:{2}", M31, M32, M33) + "}";
        }


        public static Matrix3x3 Transpose(Matrix3x3 matrix)
        {
            Matrix3x3 ret;
            Transpose(ref matrix, out ret);
            return ret;
        }


        public static void Transpose(ref Matrix3x3 matrix, out Matrix3x3 result)
        {
            Matrix3x3 ret;

            ret.M11 = matrix.M11;
            ret.M12 = matrix.M21;
            ret.M13 = matrix.M31;

            ret.M21 = matrix.M12;
            ret.M22 = matrix.M22;
            ret.M23 = matrix.M32;

            ret.M31 = matrix.M13;
            ret.M32 = matrix.M23;
            ret.M33 = matrix.M33;

            result = ret;
        }

        public void Absolute(out Matrix3x3 result)
        {
            result.M11 = Math.Abs(M11);
            result.M21 = Math.Abs(M21);
            result.M31 = Math.Abs(M31);
            result.M12 = Math.Abs(M12);
            result.M22 = Math.Abs(M22);
            result.M32 = Math.Abs(M32);
            result.M13 = Math.Abs(M13);
            result.M23 = Math.Abs(M23);
            result.M33 = Math.Abs(M33);
        }
    }
}
