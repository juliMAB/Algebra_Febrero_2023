using System;
using System.Security.Principal;
using EjerciciosVec3;
using UnityEngine;

namespace EjerciciosQuaternion
{
    [Serializable]
    public struct MyQuaternion : IEquatable<MyQuaternion>
    {
        #region Variables

        public float x;
        public float y;
        public float z;
        public float w;

        public MyQuaternion normalized => Normalize(this);
        public Vec3 eulerAngles
        {
            get => ToEulerRad(this);
            set => this = Euler(value);
        }
        public float LengthSquared
        {
            get
            {
                return x * x + y * y + z * z + w * w;
            }
        }
        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return x;
                    case 1:
                        return y;
                    case 2:
                        return z;
                    case 3:
                        return w;
                    default:
                        throw new IndexOutOfRangeException("Invalid Quaternion index!");
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        x = value;
                        break;
                    case 1:
                        y = value;
                        break;
                    case 2:
                        z = value;
                        break;
                    case 3:
                        w = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Invalid Quaternion index!");
                }
            }
        }

        #endregion

        #region Constants

        public const float kEpsilon = 1e-05f;

        #endregion

        #region Constructors

        public MyQuaternion(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public MyQuaternion(MyQuaternion q)
        {
            x = q.x;
            y = q.y;
            z = q.z;
            w = q.w;
        }

        #endregion

        #region Default Values

        public static MyQuaternion Identity { get; } = new MyQuaternion(0f, 0f, 0f, 1f);

        #endregion

        #region Operators

        public static MyQuaternion operator *(MyQuaternion lhs, MyQuaternion rhs)
        {
            return new MyQuaternion(
                lhs.w * rhs.x + lhs.x * rhs.w + lhs.y * rhs.z - lhs.z * rhs.y,
                lhs.w * rhs.y + lhs.y * rhs.w + lhs.z * rhs.x - lhs.x * rhs.z,
                lhs.w * rhs.z + lhs.z * rhs.w + lhs.x * rhs.y - lhs.y * rhs.x,
                lhs.w * rhs.w - lhs.x * rhs.x - lhs.y * rhs.y - lhs.z * rhs.z);
        }

        public static Vec3 operator *(MyQuaternion rotation, Vec3 point)
        {
            float x = rotation.x * 2f;
            float y = rotation.y * 2f;
            float z = rotation.z * 2f;
            float xx = rotation.x * x;
            float yy = rotation.y * y;
            float zz = rotation.z * z;
            float xy = rotation.x * y;
            float xz = rotation.x * z;
            float yz = rotation.y * z;
            float wx = rotation.w * x;
            float wy = rotation.w * y;
            float wz = rotation.w * z;

            Vec3 res;
            res.x = (1f - (yy + zz)) * point.x + (xy - wz) * point.y + (xz + wy) * point.z;
            res.y = (xy + wz) * point.x + (1f - (xx + zz)) * point.y + (yz - wx) * point.z;
            res.z = (xz - wy) * point.x + (yz + wx) * point.y + (1f - (xx + yy)) * point.z;
            return res;
        }

        public static bool operator ==(MyQuaternion lhs, MyQuaternion rhs)
        {
            return IsEqualUsingDot(Dot(lhs, rhs));
        }

        public static bool operator !=(MyQuaternion lhs, MyQuaternion rhs)
        {
            return !(lhs == rhs);
        }

        public static implicit operator MyQuaternion(Quaternion q) => new MyQuaternion(q.x, q.y, q.z, q.w);

        public static implicit operator Quaternion(MyQuaternion q) => new Quaternion(q.x, q.y, q.z, q.w);

        #endregion

        #region Functions

        public void Set(float newX, float newY, float newZ, float newW)
        {
            x = newX;
            y = newY;
            z = newZ;
            w = newW;
        }

        public void Set(Quaternion q)
        {
            x = q.x;
            y = q.y;
            z = q.z;
            w = q.w;
        }

        private static bool IsEqualUsingDot(float dot)
        {
            return dot > 1.0f - kEpsilon;
        }

        public static float Dot(MyQuaternion a, MyQuaternion b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
        }

        public static MyQuaternion Inverse(MyQuaternion rotation)
        {
            return new MyQuaternion(-rotation.x, -rotation.y, -rotation.z, rotation.w);
        }

        public static float Angle(MyQuaternion a, MyQuaternion b)
        {
            float dot = Dot(a, b);
            return IsEqualUsingDot(dot) ? 0.0f : Mathf.Acos(Mathf.Min(Mathf.Abs(dot), 1.0F)) * 2.0F * Mathf.Rad2Deg;
        }

        public static MyQuaternion AngleAxis(float angle, Vec3 axis)
        {
            if (axis.sqrMagnitude == 0.0f)
                return Identity;

            MyQuaternion result = Identity;
            float radians = angle * Mathf.Deg2Rad;
            radians *= 0.5f;
            axis.Normalize();
            axis = axis * Mathf.Sin(radians);
            result.x = axis.x;
            result.y = axis.y;
            result.z = axis.z;
            result.w = Mathf.Cos(radians);

            return Normalize(result);
        }

        public void ToAngleAxis(out float angle, out Vec3 axis)
        {
            angle = 0.0f;
            axis = Vec3.Zero;

            float radians = Mathf.Acos(w);

            axis.x = x;
            axis.y = y;
            axis.z = z;
            axis = axis / Mathf.Sin(radians);
            axis.Normalize();

            radians /= 0.5f;
            angle = radians * Mathf.Rad2Deg;
        }

        public static MyQuaternion Lerp(MyQuaternion a, MyQuaternion b, float t)
        {
            t = Mathf.Clamp(t, 0, 1);
            return LerpUnclamped(a, b, t);
        }

        public static MyQuaternion LerpUnclamped(MyQuaternion a, MyQuaternion b, float t)
        {

            // if either input is zero, return the other.
            if (a.LengthSquared == 0.0f)
            {
                if (b.LengthSquared == 0.0f)
                {
                    return Identity;
                }
                return b;
            }
            else if (b.LengthSquared == 0.0f)
            {
                return a;
            }


            float cosHalfAngle = a.w * b.w + Vec3.Dot(new Vec3 (a.x,a.y,a.z), new Vec3(b.x, b.y, b.z));

            if (cosHalfAngle >= 1.0f || cosHalfAngle <= -1.0f)
            {
                // angle = 0.0f, so just return one input.
                return a;
            }
            else if (cosHalfAngle < 0.0f)
            {
                b = MyQuaternion.Inverse(b);
                b.w = -b.w;
                cosHalfAngle = -cosHalfAngle;
            }

            float blendA;
            float blendB;
            if (cosHalfAngle < 0.99f)
            {
                // do proper slerp for big angles
                float halfAngle = Mathf.Acos(cosHalfAngle);
                float sinHalfAngle = Mathf.Sin(halfAngle);
                float oneOverSinHalfAngle = 1.0f / sinHalfAngle;
                blendA = Mathf.Sin(halfAngle * (1.0f - t)) * oneOverSinHalfAngle;
                blendB = Mathf.Sin(halfAngle * t) * oneOverSinHalfAngle;
            }
            else
            {
                // do lerp if angle is really small.
                blendA = 1.0f - t;
                blendB = t;
            }
            Vec3 res = blendA * new Vec3(a.x, a.y, a.z) + blendB * new Vec3(b.x, b.y, b.z);
            MyQuaternion result = new MyQuaternion(res.x,res.y,res.z, blendA * a.w + blendB * b.w);
            if (result.LengthSquared > 0.0f)
                return Normalize(result);
            else
                return Identity;
            //MyQuaternion q = Identity;
            //if (Dot(a, b) < 0)//find short path.
            //{
            //    q.x = a.x + t * (-b.x - a.x);
            //    q.y = a.y + t * (-b.y - a.y);
            //    q.z = a.z + t * (-b.z - a.z);
            //    q.w = a.w + t * (-b.w - b.w);
            //}
            //else
            //{
            //    q.x = a.x + t * (b.x - a.x);
            //    q.y = a.y + t * (b.y - a.y);
            //    q.z = a.z + t * (b.z - a.z);
            //    q.w = a.w + t * (b.w - b.w);
            //}
            //return q.normalized;
        }

        public static MyQuaternion Slerp(MyQuaternion a, MyQuaternion b, float t)
        {
            t = Mathf.Clamp(t, 0, 1);
            return SlerpUnclamped(a, b, t);
        }

        public static MyQuaternion SlerpUnclamped(MyQuaternion a, MyQuaternion b, float t)
        {
            float cosAngle = Dot(a, b);
            if (cosAngle < 0)
            {
                cosAngle = -cosAngle;
                b = new MyQuaternion(-b.x, -b.y, -b.z, -b.w);
            }
            float t1, t2;
            if (cosAngle < 0.95f)
            {
                float angle = Mathf.Acos(cosAngle);
                float sinAgle = Mathf.Sin(angle);
                float invSinAngle = 1 / sinAgle;
                t1 = Mathf.Sin((1 - t) * angle) * invSinAngle;
                t2 = Mathf.Sin(t * angle) * invSinAngle;
                return new MyQuaternion(a.x * t1 + b.x * t2, a.y * t1 + b.y * t2, a.z * t1 + a.z * t2, a.w * t1 + b.w * t2);
            }
            else
            {
                return Lerp(a, b, t);
            }
        }
        /// <summary>
        ///   te va a devolver la rotacion que forme el producto cruz de los vectores por el angulo que formen.
        /// </summary>
        public static MyQuaternion FromToRotation(Vec3 fromDirection, Vec3 toDirection)
        {
            //va a girar sobre axis la cantidad de angle.
            //axis va a ser el vector perpendicular a ambos vectores.
            Vector3 axis = Vector3.Cross(fromDirection, toDirection);
            float angle = Vector3.Angle(fromDirection, toDirection);
            return AngleAxis(angle, axis.normalized);
        }
        public void SetFromToRotation(Vec3 fromDirection, Vec3 toDirection)
        {
            this = FromToRotation(fromDirection, toDirection);
        }
        /// <summary>
        ///   te va a devolver la rotacion que se requiere para pasar de de from a to
        /// </summary>
        public static MyQuaternion RotateTowards(MyQuaternion from, MyQuaternion to, float maxDegreesDelta)
        {
            float t = Mathf.Min(1f, maxDegreesDelta / Angle(from, to));
            return SlerpUnclamped(from, to, t);
        }

        public void SetLookRotation(Vec3 view)
        {
            Vec3 up = Vec3.Up;
            SetLookRotation(view, up);
        }

        public void SetLookRotation(Vec3 view, Vec3 up)
        {
            this = LookRotation(view, up);
        }

        public static MyQuaternion LookRotation(Vec3 forward)
        {
            Vec3 up = Vec3.Up;
            return LookRotation(forward, up);
        }

        public static MyQuaternion LookRotation(Vec3 forward, Vec3 upwards)
        {
            forward = Vec3.Normalize(forward);
            Vec3 right = Vec3.Normalize(Vec3.Cross(upwards, forward));
            upwards = Vec3.Cross(forward, right);
            float m00 = right.x;
            float m01 = right.y;
            float m02 = right.z;
            float m10 = upwards.x;
            float m11 = upwards.y;
            float m12 = upwards.z;
            float m20 = forward.x;
            float m21 = forward.y;
            float m22 = forward.z;

            float num8 = (m00 + m11) + m22;
            MyQuaternion quaternion = new MyQuaternion();
            if (num8 > 0f)
            {
                float num = Mathf.Sqrt(num8 + 1f);
                quaternion.w = num * 0.5f;
                num = 0.5f / num;
                quaternion.x = (m12 - m21) * num;
                quaternion.y = (m20 - m02) * num;
                quaternion.z = (m01 - m10) * num;
                return quaternion;
            }
            if ((m00 >= m11) && (m00 >= m22))
            {
                float num7 = Mathf.Sqrt(((1f + m00) - m11) - m22);
                float num4 = 0.5f / num7;
                quaternion.x = 0.5f * num7;
                quaternion.y = (m01 + m10) * num4;
                quaternion.z = (m02 + m20) * num4;
                quaternion.w = (m12 - m21) * num4;
                return quaternion;
            }
            if (m11 > m22)
            {
                float num6 = Mathf.Sqrt(((1f + m11) - m00) - m22);
                float num3 = 0.5f / num6;
                quaternion.x = (m10 + m01) * num3;
                quaternion.y = 0.5f * num6;
                quaternion.z = (m21 + m12) * num3;
                quaternion.w = (m20 - m02) * num3;
                return quaternion;
            }
            float num5 = Mathf.Sqrt(((1f + m22) - m00) - m11);
            float num2 = 0.5f / num5;
            quaternion.x = (m20 + m02) * num2;
            quaternion.y = (m21 + m12) * num2;
            quaternion.z = 0.5f * num5;
            quaternion.w = (m01 - m10) * num2;
            return quaternion;
        }

        public static MyQuaternion Euler(float x, float y, float z)
        {
            MyQuaternion qx = Identity;
            MyQuaternion qy = Identity;
            MyQuaternion qz = Identity;

            float sinAngle = 0.0f;
            float cosAngle = 0.0f;
            //se calcula el seno del angulo en la componente imaginalia.
            //y el coseno del angulo en la componente real.
            sinAngle = Mathf.Sin(Mathf.Deg2Rad * y * 0.5f);
            cosAngle = Mathf.Cos(Mathf.Deg2Rad * y * 0.5f);
            qy.Set(0, sinAngle, 0, cosAngle);

            sinAngle = Mathf.Sin(Mathf.Deg2Rad * x * 0.5f);
            cosAngle = Mathf.Cos(Mathf.Deg2Rad * x * 0.5f);
            qx.Set(sinAngle, 0, 0, cosAngle);

            sinAngle = Mathf.Sin(Mathf.Deg2Rad * z * 0.5f);
            cosAngle = Mathf.Cos(Mathf.Deg2Rad * z * 0.5f);
            qz.Set(0, 0, sinAngle, cosAngle);
            //y se aplican las 3 rotaciones en este orden especifico.
            return qy * qx * qz;
        }

        public static MyQuaternion Euler(Vec3 euler)
        {
            return Euler(euler.x, euler.y, euler.z);
        }

        public static Vec3 ToEulerRad(MyQuaternion rotation)
        {
            //Q/?a2+b2+c2+d2. //normalizar el quaternion.
            float sqw = rotation.w * rotation.w;
            float sqx = rotation.x * rotation.x;
            float sqy = rotation.y * rotation.y;
            float sqz = rotation.z * rotation.z;
            float unit = sqx + sqy + sqz + sqw;
            float test = rotation.x * rotation.w - rotation.y * rotation.z;
            Vec3 v;
            // singularity north
            if (test > 0.4995f * unit)
            {
                v.y = 2f * Mathf.Atan2(rotation.y, rotation.x);
                v.x = Mathf.PI / 2;
                v.z = 0;
                return NormalizeAngles(v * Mathf.Rad2Deg);
            }
            // singularity south
            if (test < -0.4995f * unit)
            {
                v.y = -2f * Mathf.Atan2(rotation.y, rotation.x);
                v.x = -Mathf.PI / 2;
                v.z = 0;
                return NormalizeAngles(v * Mathf.Rad2Deg);
            }
            //this is the formula.
            //https://en.wikipedia.org/wiki/Conversion_between_quaternions_and_Euler_angles#Euler_angles_to_quaternion_conversion
            MyQuaternion q = new MyQuaternion(rotation.w, rotation.z, rotation.x, rotation.y);
            v.y = Mathf.Atan2(2f * q.x * q.w + 2f * q.y * q.z, 1 - 2f * (q.z * q.z + q.w * q.w));
            v.x = Mathf.Asin(2f * (q.x * q.z - q.w * q.y));
            v.z = Mathf.Atan2(2f * q.x * q.y + 2f * q.z * q.w, 1 - 2f * (q.y * q.y + q.z * q.z));
            return NormalizeAngles(v * Mathf.Rad2Deg);
        }
        /// <summary>
        ///set the values ??between 360 and 0
        /// </summary>
        private static Vec3 NormalizeAngles(Vec3 angles)
        {
            angles.x = NormalizeAngle(angles.x);
            angles.y = NormalizeAngle(angles.y);
            angles.z = NormalizeAngle(angles.z);
            return angles;
        }
        private static float NormalizeAngle(float angle)
        {
            while (angle > 360)
                angle -= 360;
            while (angle < 0)
                angle += 360;
            return angle;
        }

        /// <summary>
        /// Converts this quaternion to one with the same orientation but with a magnitude
        /// of 1.
        /// mag = Mathf.Sqrt(Dot(q, q));
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        public static MyQuaternion Normalize(MyQuaternion q)
        {
            float mag = Mathf.Sqrt(Dot(q, q));

            if (mag < kEpsilon)
                return Identity;

            return new MyQuaternion(q.x / mag, q.y / mag, q.z / mag, q.w / mag);
        }

        public void Normalize()
        {
            this = Normalize(this);
        }

        #endregion

        #region Internals

        public bool Equals(MyQuaternion other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return x.Equals(other.x) && y.Equals(other.y) && z.Equals(other.z) && w.Equals(other.w);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MyQuaternion)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = x.GetHashCode();
                hashCode = (hashCode * 397) ^ y.GetHashCode();
                hashCode = (hashCode * 397) ^ z.GetHashCode();
                hashCode = (hashCode * 397) ^ w.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"({x}, {y}, {z}, {w})";
        }

        public string ToString(string format)
        {
            return string.Format("({0}, {1}, {2}, {3})", x.ToString(format), y.ToString(format), z.ToString(format), w.ToString(format));
        }
        #endregion
    }
}

