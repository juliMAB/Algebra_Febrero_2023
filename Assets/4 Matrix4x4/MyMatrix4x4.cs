using System;
using UnityEngine;
using Vector4 = UnityEngine.Vector4;
using EjerciciosQuaternion;
using EjerciciosVec3;

namespace EjerciciosMatrix
{
    public struct MyMatrix4x4 : IEquatable<MyMatrix4x4>
    {
        #region Variables

        public float m00;
        public float m10;
        public float m20;
        public float m30;

        public float m01;
        public float m11;
        public float m21;
        public float m31;

        public float m02;
        public float m12;
        public float m22;
        public float m32;

        public float m03;
        public float m13;
        public float m23;
        public float m33;

        public MyQuaternion rotation => GetRotation(this);
        public Vec3 lossyScale => GetScale(this);
        public MyMatrix4x4 transpose => Transpose(this);

        public float this[int row, int column]
        {
            get => this[row + column * 4];
            set => this[row + column * 4] = value;
        }

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return m00;
                    case 1:
                        return m10;
                    case 2:
                        return m20;
                    case 3:
                        return m30;
                    case 4:
                        return m01;
                    case 5:
                        return m11;
                    case 6:
                        return m21;
                    case 7:
                        return m31;
                    case 8:
                        return m02;
                    case 9:
                        return m12;
                    case 10:
                        return m22;
                    case 11:
                        return m32;
                    case 12:
                        return m03;
                    case 13:
                        return m13;
                    case 14:
                        return m23;
                    case 15:
                        return m33;
                    default:
                        throw new IndexOutOfRangeException("Invalid matrix index!");
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        m00 = value;
                        break;
                    case 1:
                        m10 = value;
                        break;
                    case 2:
                        m20 = value;
                        break;
                    case 3:
                        m30 = value;
                        break;
                    case 4:
                        m01 = value;
                        break;
                    case 5:
                        m11 = value;
                        break;
                    case 6:
                        m21 = value;
                        break;
                    case 7:
                        m31 = value;
                        break;
                    case 8:
                        m02 = value;
                        break;
                    case 9:
                        m12 = value;
                        break;
                    case 10:
                        m22 = value;
                        break;
                    case 11:
                        m32 = value;
                        break;
                    case 12:
                        m03 = value;
                        break;
                    case 13:
                        m13 = value;
                        break;
                    case 14:
                        m23 = value;
                        break;
                    case 15:
                        m33 = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Invalid matrix index!");
                }
            }
        }

        #endregion

        #region Constructors

        public MyMatrix4x4(Vector4 column0, Vector4 column1, Vector4 column2, Vector4 column3)
        {
            m00 = column0.x;
            m01 = column1.x;
            m02 = column2.x;
            m03 = column3.x;
            m10 = column0.y;
            m11 = column1.y;
            m12 = column2.y;
            m13 = column3.y;
            m20 = column0.z;
            m21 = column1.z;
            m22 = column2.z;
            m23 = column3.z;
            m30 = column0.w;
            m31 = column1.w;
            m32 = column2.w;
            m33 = column3.w;
        }

        #endregion

        #region Operators

        public static MyMatrix4x4 operator *(MyMatrix4x4 lhs, MyMatrix4x4 rhs)
        {
            MyMatrix4x4 matrix4x4;
            matrix4x4.m00 = (lhs.m00 * rhs.m00 + lhs.m01 * rhs.m10 + lhs.m02 * rhs.m20 + lhs.m03 * rhs.m30);
            matrix4x4.m01 = (lhs.m00 * rhs.m01 + lhs.m01 * rhs.m11 + lhs.m02 * rhs.m21 + lhs.m03 * rhs.m31);
            matrix4x4.m02 = (lhs.m00 * rhs.m02 + lhs.m01 * rhs.m12 + lhs.m02 * rhs.m22 + lhs.m03 * rhs.m32);
            matrix4x4.m03 = (lhs.m00 * rhs.m03 + lhs.m01 * rhs.m13 + lhs.m02 * rhs.m23 + lhs.m03 * rhs.m33);
            matrix4x4.m10 = (lhs.m10 * rhs.m00 + lhs.m11 * rhs.m10 + lhs.m12 * rhs.m20 + lhs.m13 * rhs.m30);
            matrix4x4.m11 = (lhs.m10 * rhs.m01 + lhs.m11 * rhs.m11 + lhs.m12 * rhs.m21 + lhs.m13 * rhs.m31);
            matrix4x4.m12 = (lhs.m10 * rhs.m02 + lhs.m11 * rhs.m12 + lhs.m12 * rhs.m22 + lhs.m13 * rhs.m32);
            matrix4x4.m13 = (lhs.m10 * rhs.m03 + lhs.m11 * rhs.m13 + lhs.m12 * rhs.m23 + lhs.m13 * rhs.m33);
            matrix4x4.m20 = (lhs.m20 * rhs.m00 + lhs.m21 * rhs.m10 + lhs.m22 * rhs.m20 + lhs.m23 * rhs.m30);
            matrix4x4.m21 = (lhs.m20 * rhs.m01 + lhs.m21 * rhs.m11 + lhs.m22 * rhs.m21 + lhs.m23 * rhs.m31);
            matrix4x4.m22 = (lhs.m20 * rhs.m02 + lhs.m21 * rhs.m12 + lhs.m22 * rhs.m22 + lhs.m23 * rhs.m32);
            matrix4x4.m23 = (lhs.m20 * rhs.m03 + lhs.m21 * rhs.m13 + lhs.m22 * rhs.m23 + lhs.m23 * rhs.m33);
            matrix4x4.m30 = (lhs.m30 * rhs.m00 + lhs.m31 * rhs.m10 + lhs.m32 * rhs.m20 + lhs.m33 * rhs.m30);
            matrix4x4.m31 = (lhs.m30 * rhs.m01 + lhs.m31 * rhs.m11 + lhs.m32 * rhs.m21 + lhs.m33 * rhs.m31);
            matrix4x4.m32 = (lhs.m30 * rhs.m02 + lhs.m31 * rhs.m12 + lhs.m32 * rhs.m22 + lhs.m33 * rhs.m32);
            matrix4x4.m33 = (lhs.m30 * rhs.m03 + lhs.m31 * rhs.m13 + lhs.m32 * rhs.m23 + lhs.m33 * rhs.m33);
            return matrix4x4;
        }

        public static Vector4 operator *(MyMatrix4x4 lhs, Vector4 vector)
        {
            Vector4 vector4;
            vector4.x = (lhs.m00 * vector.x + lhs.m01 * vector.y + lhs.m02 * vector.z + lhs.m03 * vector.w);
            vector4.y = (lhs.m10 * vector.x + lhs.m11 * vector.y + lhs.m12 * vector.z + lhs.m13 * vector.w);
            vector4.z = (lhs.m20 * vector.x + lhs.m21 * vector.y + lhs.m22 * vector.z + lhs.m23 * vector.w);
            vector4.w = (lhs.m30 * vector.x + lhs.m31 * vector.y + lhs.m32 * vector.z + lhs.m33 * vector.w);
            return vector4;
        }

        public static bool operator ==(MyMatrix4x4 lhs, MyMatrix4x4 rhs) => lhs.GetColumn(0) == rhs.GetColumn(0) && lhs.GetColumn(1) == rhs.GetColumn(1) && lhs.GetColumn(2) == rhs.GetColumn(2) && lhs.GetColumn(3) == rhs.GetColumn(3);

        public static bool operator !=(MyMatrix4x4 lhs, MyMatrix4x4 rhs) => !(lhs == rhs);

        #endregion

        #region Default Values

        public static MyMatrix4x4 zero { get; } = new MyMatrix4x4(new Vector4(0.0f, 0.0f, 0.0f, 0.0f), new Vector4(0.0f, 0.0f, 0.0f, 0.0f), new Vector4(0.0f, 0.0f, 0.0f, 0.0f), new Vector4(0.0f, 0.0f, 0.0f, 0.0f));
        public static MyMatrix4x4 identity { get; } = new MyMatrix4x4(new Vector4(1f, 0.0f, 0.0f, 0.0f), new Vector4(0.0f, 1f, 0.0f, 0.0f), new Vector4(0.0f, 0.0f, 1f, 0.0f), new Vector4(0.0f, 0.0f, 0.0f, 1f));

        #endregion

        #region Functions

        public Vector4 GetColumn(int index)
        {
            switch (index)
            {
                case 0:
                    return new Vector4(m00, m10, m20, m30);
                case 1:
                    return new Vector4(m01, m11, m21, m31);
                case 2:
                    return new Vector4(m02, m12, m22, m32);
                case 3:
                    return new Vector4(m03, m13, m23, m33);
                default:
                    throw new IndexOutOfRangeException("Invalid column index!");
            }
        }

        public Vector4 GetRow(int index)
        {
            switch (index)
            {
                case 0:
                    return new Vector4(m00, m01, m02, m03);
                case 1:
                    return new Vector4(m10, m11, m12, m13);
                case 2:
                    return new Vector4(m20, m21, m22, m23);
                case 3:
                    return new Vector4(m30, m31, m32, m33);
                default:
                    throw new IndexOutOfRangeException("Invalid row index!");
            }
        }

        public void SetColumn(int index, Vector4 column)
        {
            this[0, index] = column.x;
            this[1, index] = column.y;
            this[2, index] = column.z;
            this[3, index] = column.w;
        }

        public void SetRow(int index, Vector4 row)
        {
            this[index, 0] = row.x;
            this[index, 1] = row.y;
            this[index, 2] = row.z;
            this[index, 3] = row.w;
        }

        public static MyQuaternion GetRotation(MyMatrix4x4 m)
        {
            Vec3 s = GetScale(m);

            float m00 = m[0, 0] / s.x;
            float m01 = m[0, 1] / s.y;
            float m02 = m[0, 2] / s.z;
            float m10 = m[1, 0] / s.x;
            float m11 = m[1, 1] / s.y;
            float m12 = m[1, 2] / s.z;
            float m20 = m[2, 0] / s.x;
            float m21 = m[2, 1] / s.y;
            float m22 = m[2, 2] / s.z;

            MyQuaternion q = MyQuaternion.Identity;
            q.w = Mathf.Sqrt(Mathf.Max(0, 1 + m00 + m11 + m22)) / 2;
            q.x = Mathf.Sqrt(Mathf.Max(0, 1 + m00 - m11 - m22)) / 2;
            q.y = Mathf.Sqrt(Mathf.Max(0, 1 - m00 + m11 - m22)) / 2;
            q.z = Mathf.Sqrt(Mathf.Max(0, 1 - m00 - m11 + m22)) / 2;
            q.x *= Mathf.Sign(q.x * (m21 - m12));
            q.y *= Mathf.Sign(q.y * (m02 - m20));
            q.z *= Mathf.Sign(q.z * (m10 - m01));

            float qMagnitude = Mathf.Sqrt(q.w * q.w + q.x * q.x + q.y * q.y + q.z * q.z);
            q.w /= qMagnitude;
            q.x /= qMagnitude;
            q.y /= qMagnitude;
            q.z /= qMagnitude;

            return q;
        }

        public static Vec3 GetScale(MyMatrix4x4 m)
        {
            return new Vec3(m.GetColumn(0).magnitude, m.GetColumn(1).magnitude, m.GetColumn(2).magnitude);
        }

        public static MyMatrix4x4 Translate(Vec3 vector)
        {
            MyMatrix4x4 matrix4x4;
            matrix4x4.m00 = 1f;
            matrix4x4.m01 = 0.0f;
            matrix4x4.m02 = 0.0f;
            matrix4x4.m03 = vector.x;
            matrix4x4.m10 = 0.0f;
            matrix4x4.m11 = 1f;
            matrix4x4.m12 = 0.0f;
            matrix4x4.m13 = vector.y;
            matrix4x4.m20 = 0.0f;
            matrix4x4.m21 = 0.0f;
            matrix4x4.m22 = 1f;
            matrix4x4.m23 = vector.z;
            matrix4x4.m30 = 0.0f;
            matrix4x4.m31 = 0.0f;
            matrix4x4.m32 = 0.0f;
            matrix4x4.m33 = 1f;
            return matrix4x4;
        }

        public static MyMatrix4x4 Scale(Vec3 vector)
        {
            MyMatrix4x4 matrix4x4;
            matrix4x4.m00 = vector.x;
            matrix4x4.m01 = 0.0f;
            matrix4x4.m02 = 0.0f;
            matrix4x4.m03 = 0.0f;
            matrix4x4.m10 = 0.0f;
            matrix4x4.m11 = vector.y;
            matrix4x4.m12 = 0.0f;
            matrix4x4.m13 = 0.0f;
            matrix4x4.m20 = 0.0f;
            matrix4x4.m21 = 0.0f;
            matrix4x4.m22 = vector.z;
            matrix4x4.m23 = 0.0f;
            matrix4x4.m30 = 0.0f;
            matrix4x4.m31 = 0.0f;
            matrix4x4.m32 = 0.0f;
            matrix4x4.m33 = 1f;
            return matrix4x4;
        }

        public static MyMatrix4x4 Rotate(MyQuaternion q)
        {
            float num1 = q.x * 2f;
            float num2 = q.y * 2f;
            float num3 = q.z * 2f;
            float num4 = q.x * num1;
            float num5 = q.y * num2;
            float num6 = q.z * num3;
            float num7 = q.x * num2;
            float num8 = q.x * num3;
            float num9 = q.y * num3;
            float num10 = q.w * num1;
            float num11 = q.w * num2;
            float num12 = q.w * num3;

            MyMatrix4x4 matrix4x4;
            matrix4x4.m00 = (1.0f - (num5 + num6));
            matrix4x4.m10 = num7 + num12;
            matrix4x4.m20 = num8 - num11;
            matrix4x4.m30 = 0.0f;
            matrix4x4.m01 = num7 - num12;
            matrix4x4.m11 = (1.0f - (num4 + num6));
            matrix4x4.m21 = num9 + num10;
            matrix4x4.m31 = 0.0f;
            matrix4x4.m02 = num8 + num11;
            matrix4x4.m12 = num9 - num10;
            matrix4x4.m22 = (1.0f - (num4 + num5));
            matrix4x4.m32 = 0.0f;
            matrix4x4.m03 = 0.0f;
            matrix4x4.m13 = 0.0f;
            matrix4x4.m23 = 0.0f;
            matrix4x4.m33 = 1f;
            return matrix4x4;
        }

        public static MyMatrix4x4 Transpose(MyMatrix4x4 matrix)
        {
            MyMatrix4x4 result;

            result.m00 = matrix.m00;
            result.m01 = matrix.m10;
            result.m02 = matrix.m20;
            result.m03 = matrix.m30;
            result.m10 = matrix.m01;
            result.m11 = matrix.m11;
            result.m12 = matrix.m21;
            result.m13 = matrix.m31;
            result.m20 = matrix.m02;
            result.m21 = matrix.m12;
            result.m22 = matrix.m22;
            result.m23 = matrix.m32;
            result.m30 = matrix.m03;
            result.m31 = matrix.m13;
            result.m32 = matrix.m23;
            result.m33 = matrix.m33;

            return result;
        }

        public static MyMatrix4x4 Inverse(MyMatrix4x4 matrix)
        {
            MyMatrix4x4 result = zero;

            float det = GetDeterminant(matrix);

            if (Mathf.Abs(det) < Mathf.Epsilon)
                return result;

            MyMatrix4x4 adj = identity;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    adj[i, j] = GetDet3X3(matrix, i, j) * Mathf.Pow(-1f, (i + j));
                }
            }

            MyMatrix4x4 t = Transpose(adj);
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    result[i, j] = t[i, j] / det;
                }
            }

            return result;
        }

        public static float GetDet3X3(MyMatrix4x4 matrix, int row, int column)
        {
            float det = 0f;
            float[,] mat3x3 = new float[3, 3];
            int contI = 0;
            int contJ = 0;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (i != row && j != column)
                    {
                        if (contJ == 3)
                        {
                            contJ = 0;
                            contI++;
                        }

                        mat3x3[contI, contJ] = matrix[i, j];
                        contJ++;
                    }
                }
            }

            for (int i = 0; i < 3; i++)
                det += (mat3x3[0, i] * (mat3x3[1, (i + 1) % 3] * mat3x3[2, (i + 2) % 3] - mat3x3[1, (i + 2) % 3] * mat3x3[2, (i + 1) % 3]));

            return det;
        }

        public static MyMatrix4x4 TRS(Vec3 pos, MyQuaternion qua, Vec3 sca)
        {
            MyMatrix4x4 t = Translate(pos);
            MyMatrix4x4 r = Rotate(qua);
            MyMatrix4x4 s = Scale(sca);
            MyMatrix4x4 trs = t * r * s;

            return trs;
        }

        public void SetTRS(Vec3 pos, MyQuaternion qua, Vec3 sca)
        {
            this = TRS(pos, qua, sca);
        }

        public static float GetDeterminant(MyMatrix4x4 matrix)
        {
            return ((matrix.m00 * matrix.m11 * matrix.m22 * matrix.m33) +
                     (matrix.m01 * matrix.m12 * matrix.m23 * matrix.m30) +
                     (matrix.m02 * matrix.m13 * matrix.m20 * matrix.m31) +
                     (matrix.m03 * matrix.m10 * matrix.m21 * matrix.m32)) -
                    ((matrix.m03 * matrix.m12 * matrix.m21 * matrix.m30) +
                     (matrix.m13 * matrix.m22 * matrix.m31 * matrix.m00) +
                     (matrix.m23 * matrix.m32 * matrix.m01 * matrix.m10) +
                     (matrix.m33 * matrix.m02 * matrix.m11 * matrix.m20));
        }

        public Vec3 MultiplyPoint(Vec3 point)
        {
            Vec3 v3;
            v3.x = (m00 * point.x + m01 * point.y + m02 * point.z) + m03;
            v3.y = (m10 * point.x + m11 * point.y + m12 * point.z) + m13;
            v3.z = (m20 * point.x + m21 * point.y + m22 * point.z) + m23;
            float num = 1f / ((m30 * point.x + m31 * point.y + m32 * point.z) + m33);
            v3.x *= num;
            v3.y *= num;
            v3.z *= num;
            return v3;
        }

        public Vec3 MultiplyPoint3x4(Vec3 point)
        {
            Vec3 v3;
            v3.x = (m00 * point.x + m01 * point.y + m02 * point.z) + m03;
            v3.y = (m10 * point.x + m11 * point.y + m12 * point.z) + m13;
            v3.z = (m20 * point.x + m21 * point.y + m22 * point.z) + m23;
            return v3;
        }

        public Vec3 MultiplyVector(Vec3 vector)
        {
            Vec3 v3;
            v3.x = (m00 * vector.x + m01 * vector.y + m02 * vector.z);
            v3.y = (m10 * vector.x + m11 * vector.y + m12 * vector.z);
            v3.z = (m20 * vector.x + m21 * vector.y + m22 * vector.z);
            return v3;
        }

        #endregion

        #region Internals

        public bool Equals(MyMatrix4x4 other)
        {
            return m00.Equals(other.m00) && m10.Equals(other.m10) && m20.Equals(other.m20) && m30.Equals(other.m30) &&
                   m01.Equals(other.m01) && m11.Equals(other.m11) && m21.Equals(other.m21) && m31.Equals(other.m31) &&
                   m02.Equals(other.m02) && m12.Equals(other.m12) && m22.Equals(other.m22) && m32.Equals(other.m32) &&
                   m03.Equals(other.m03) && m13.Equals(other.m13) && m23.Equals(other.m23) && m33.Equals(other.m33);
        }

        public override bool Equals(object obj)
        {
            return obj is MyMatrix4x4 other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = m00.GetHashCode();
                hashCode = (hashCode * 397) ^ m10.GetHashCode();
                hashCode = (hashCode * 397) ^ m20.GetHashCode();
                hashCode = (hashCode * 397) ^ m30.GetHashCode();
                hashCode = (hashCode * 397) ^ m01.GetHashCode();
                hashCode = (hashCode * 397) ^ m11.GetHashCode();
                hashCode = (hashCode * 397) ^ m21.GetHashCode();
                hashCode = (hashCode * 397) ^ m31.GetHashCode();
                hashCode = (hashCode * 397) ^ m02.GetHashCode();
                hashCode = (hashCode * 397) ^ m12.GetHashCode();
                hashCode = (hashCode * 397) ^ m22.GetHashCode();
                hashCode = (hashCode * 397) ^ m32.GetHashCode();
                hashCode = (hashCode * 397) ^ m03.GetHashCode();
                hashCode = (hashCode * 397) ^ m13.GetHashCode();
                hashCode = (hashCode * 397) ^ m23.GetHashCode();
                hashCode = (hashCode * 397) ^ m33.GetHashCode();
                return hashCode;
            }
        }

        #endregion
    }
}