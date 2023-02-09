using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace juli

{
    public struct Vec3 : IEquatable<Vec3>
    {
        #region Variables
        public float x;
        public float y;
        public float z;

        public float sqrMagnitude { get { return Vec3.SqrMagnitude(new Vec3(x, y, z)); } }
        public Vec3 normalized { get { return Vec3.Normalize(this); } }
        public float magnitude { get { return Magnitude(new Vec3(x, y, z)); } }
        #endregion

        #region constants
        public const float epsilon = 1e-05f;
        #endregion

        #region Default Values
        public static Vec3 Zero { get { return new Vec3(0.0f, 0.0f, 0.0f); } }
        public static Vec3 One { get { return new Vec3(1.0f, 1.0f, 1.0f); } }
        public static Vec3 Forward { get { return new Vec3(0.0f, 0.0f, 1.0f); } }
        public static Vec3 Back { get { return new Vec3(0.0f, 0.0f, -1.0f); } }
        public static Vec3 Right { get { return new Vec3(1.0f, 0.0f, 0.0f); } }
        public static Vec3 Left { get { return new Vec3(-1.0f, 0.0f, 0.0f); } }
        public static Vec3 Up { get { return new Vec3(0.0f, 1.0f, 0.0f); } }
        public static Vec3 Down { get { return new Vec3(0.0f, -1.0f, 0.0f); } }
        public static Vec3 PositiveInfinity { get { return new Vec3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity); } }
        public static Vec3 NegativeInfinity { get { return new Vec3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity); } }
        #endregion                                                                                                                                                                               

        #region Constructors
        public Vec3(float x, float y)
        {
            this.x = x;
            this.y = y;
            this.z = 0.0f;
        }

        public Vec3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        
        public Vec3(Vec3 v3)
        {
            this.x = v3.x;
            this.y = v3.y;
            this.z = v3.z;
        }

        public Vec3(Vector3 v3)
        {
            this.x = v3.x;
            this.y = v3.y;
            this.z = v3.z;
        }

        public Vec3(Vector2 v2)
        {
            this.x = v2.x;
            this.y = v2.y;
            this.z = 0.0f;
        }
        #endregion

        #region Operators
        public static bool operator ==(Vec3 left, Vec3 right)
        {
            float diff_x = left.x - right.x;
            float diff_y = left.y - right.y;
            float diff_z = left.z - right.z;
            float sqrmag = diff_x * diff_x + diff_y * diff_y + diff_z * diff_z;
            return sqrmag < epsilon * epsilon;
        }
        public static bool operator !=(Vec3 left, Vec3 right)
        {
            return !(left == right);
        }

        public static Vec3 operator +(Vec3 leftV3, Vec3 rightV3)
        {
            return new Vec3(leftV3.x + rightV3.x, leftV3.y + rightV3.y, leftV3.z + rightV3.z);
        }

        public static Vec3 operator -(Vec3 leftV3, Vec3 rightV3)
        {
            return new Vec3(leftV3.x - rightV3.x, leftV3.y - rightV3.y, leftV3.z - rightV3.z);
        }

        public static Vec3 operator -(Vec3 v3)
        {
            return new Vec3(v3.x * -1, v3.y * -1, v3.z * -1);
        }

        public static Vec3 operator *(Vec3 v3, float scalar)
        {
            return new Vec3(v3.x * scalar, v3.y * scalar, v3.z * scalar);
        }
        public static Vec3 operator *(float scalar, Vec3 v3)
        {
            return new Vec3(v3.x * scalar, v3.y * scalar, v3.z * scalar);
        }
        public static Vec3 operator /(Vec3 v3, float scalar)
        {
            return new Vec3(v3.x / scalar, v3.y / scalar, v3.z / scalar);
        }

        public static implicit operator Vector3(Vec3 v3)
        {
            return new Vector3(v3.x, v3.y, v3.z);
        }

        public static implicit operator Vector2(Vec3 v2)
        {
            return new Vector2(v2.x, v2.y);
        }
        #endregion

        #region Functions

        public override string ToString()
        {
            return "X = " + x.ToString() + "   Y = " + y.ToString() + "   Z = " + z.ToString();
        }
        public static float Angle(Vec3 from, Vec3 to)
        {
            return Mathf.Rad2Deg * (float)(Mathf.Acos(Dot(from, to) / (Magnitude(from) * Magnitude(to))));
        }

        public static explicit operator Vec3(Vector3 v)
        {
            return new Vec3(v.x, v.y, v.z);
        }

        public static Vec3 ClampMagnitude(Vec3 vector, float maxLength)
        {
            if (Magnitude(vector) > maxLength)
            {
                float relation = maxLength / Magnitude(vector);
                vector.x = vector.x * relation;
                vector.y = vector.y * relation;
                vector.z = vector.z * relation;
                if (float.IsNaN(vector.x))
                    vector.x = 0;
                if (float.IsNaN(vector.y))
                    vector.y = 0;
                if (float.IsNaN(vector.z))
                    vector.z = 0;
            }
            return vector;
        }
        public static float Magnitude(Vec3 vector)
        {
            return (float)Math.Sqrt(SqrMagnitude(vector));
            //return (float)Math.Sqrt(Math.Pow(vector.x, 2) + Math.Pow(vector.y, 2) + Math.Pow(vector.z, 2));
        }
        public static Vec3 Cross(Vec3 a, Vec3 b)
        {
            return new Vec3(((a.y * b.z) - (a.z * b.y)), ((a.z * b.x) - (a.x * b.z)), ((a.x * b.y) - (a.y * b.x)));
        }
        public static float Distance(Vec3 a, Vec3 b)
        {
            return (float)Mathf.Sqrt(Mathf.Pow(a.x - b.x, 2) + Mathf.Pow(a.y - b.y, 2) + Mathf.Pow(a.z - b.z, 2));
        }
        public static float Dot(Vec3 a, Vec3 b)
        {
            return (a.x * b.x) + (a.y * b.y) + (a.z * b.z);
        }
        public static Vec3 Lerp(Vec3 a, Vec3 b, float t)
        {
            Vec3 c = new Vec3(t * (b.x - a.x) + a.x, t * (b.y - a.y) + a.y, t * (b.z - a.z) + a.z);
            if (t > 1.0f)
            {
                return b;
            }
            else if (t < 0)
            {
                return a;
            }
            else
            {
                return c;
            }
        }
        public static Vec3 LerpUnclamped(Vec3 a, Vec3 b, float t)
        {
            Vec3 c = new Vec3(t * (b.x - a.x) + a.x, t * (b.y - a.y) + a.y, t * (b.z - a.z) + a.z);
            return c;
        }
        public static Vec3 Max(Vec3 a, Vec3 b)
        {
            if (a.x < b.x)
                a.x = b.x;
            if (a.y < b.y)
                a.y = b.y;
            if (a.z < b.z)
                a.z = b.z;
            return a;
        }
        public static Vec3 Min(Vec3 a, Vec3 b)
        {
            if (a.x > b.x)
                a.x = b.x;
            if (a.y > b.y)
                a.y = b.y;
            if (a.z > b.z)
                a.z = b.z;
            return a;
        }
        public static float SqrMagnitude(Vec3 vector)
        {
            float x = vector.x;
            float y = vector.y;
            float z = vector.z;

            return (float)(Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2));

        }
        public static Vec3 Project(Vec3 vector, Vec3 onNormal)
        {
            if (float.IsNaN(vector.x))
                vector.x = 0;
            if (float.IsNaN(vector.y))
                vector.y = 0;
            if (float.IsNaN(vector.z))
                vector.z = 0;
            return (Dot(vector, onNormal) / (Magnitude(onNormal) * Magnitude(onNormal))) * onNormal;
        }
        public static Vec3 Reflect(Vec3 inDirection, Vec3 inNormal)
        {
            return -2F * Dot(inNormal, inDirection) * inNormal + inDirection;
        }
        public void Set(float newX, float newY, float newZ)
        {
            x = newX;
            y = newY;
            z = newZ;
        }
        public void Scale(Vec3 scale)
        {
            x = x * scale.x;
            y = y * scale.y;
            z = z * scale.z;
        }
        public static Vec3 Normalize(Vec3 inNormal)
        {
            return inNormal / Vec3.Magnitude(inNormal);
        }
        public float Max(float x, float y, float z)
        {
            float max = x;
            if (Math.Abs(max) < Math.Abs(y))
            {
                max = y;
                if (Math.Abs(max) < Math.Abs(z))
                {
                    max = z;
                }
            }
            else if (Math.Abs(max) < Math.Abs(z))
            {
                max = z;
            }
            return Math.Abs(max);
        }
        #endregion

        #region Internals
        public override bool Equals(object other)
        {
            if (!(other is Vec3)) return false;
            return Equals((Vec3)other);
        }

        public bool Equals(Vec3 other)
        {
            return x == other.x && y == other.y && z == other.z;
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ (y.GetHashCode() << 2) ^ (z.GetHashCode() >> 2);
        }
        #endregion
    }

}