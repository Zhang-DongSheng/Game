using UnityEngine;

namespace Game
{
    public static partial class Utility
    {
        /// <summary>
        /// 向量
        /// </summary>
        public static class Vector
        {
            private const float Min = 45f;

            private const float Max = 135f;
            /// <summary>
            /// 横向
            /// </summary>
            public static bool Horizontal(Vector2 vector)
            {
                float angle = Vector2.Angle(vector, Vector2.up);

                return angle > Min && angle < Max;
            }
            /// <summary>
            /// 竖向
            /// </summary>
            public static bool Vertical(Vector2 vector)
            {
                float angle = Vector2.Angle(vector, Vector2.up);

                return angle < Min || angle > Max;
            }
            /// <summary>
            /// 三维转二维
            /// </summary>
            public static Vector2 Vector3To2(Vector3 vector)
            {
                return new Vector2(vector.x, vector.z);
            }
            /// <summary>
            /// 二维转三维
            /// </summary>
            public static Vector3 Vector2To3(Vector2 vector, float height = 0)
            {
                return new Vector3(vector.x, height, vector.y);
            }

            public static Quaternion LookRotation(float horizontal, float vertical)
            {
                Vector3 vector = new Vector3(horizontal, 0, vertical);

                if (vector != Vector3.zero)
                {
                    return Quaternion.LookRotation(vector, Vector3.up);
                }
                return Quaternion.identity;
            }
            /// <summary>
            /// 相对角度
            /// </summary>
            public static float RelativeAngle(Vector3 from, Vector3 to)
            {
                Vector3 vector = to - from;

                vector.y = vector.z;

                return Vector2.SignedAngle(vector, Vector2.up);
            }
            /// <summary>
            /// 相对位置
            /// </summary>
            public static Vector3 RelativePosition(float angle, float distance)
            {
                return new Vector3()
                {
                    x = distance * Mathf.Cos(angle * Mathf.Deg2Rad),
                    y = 0,
                    z = distance * Mathf.Sin(angle * Mathf.Deg2Rad)
                };
            }
            /// <summary>
            /// 顺时针旋转
            /// </summary>
            public static Vector3 RotateClockwise(Vector3 position, Vector3 direction, float angle)
            {
                angle *= Mathf.Deg2Rad;

                position.x += direction.x * Mathf.Cos(angle) + direction.z * Mathf.Sin(angle);

                position.z += -direction.x * Mathf.Sin(angle) + direction.z * Mathf.Cos(angle);

                return position;
            }
            /// <summary>
            /// 逆时针旋转
            /// </summary>
            public static Vector3 RotateCounterclockwise(Vector3 position, Vector3 direction, float angle)
            {
                angle *= Mathf.Deg2Rad;

                position.x += direction.x * Mathf.Cos(angle) - direction.z * Mathf.Sin(angle);

                position.z += direction.x * Mathf.Sin(angle) + direction.z * Mathf.Cos(angle);

                return position;
            }
            /// <summary>
            /// 环绕旋转
            /// </summary>
            public static Vector3 RotateAround(Vector3 position, Vector3 center, float angle)
            {
                angle *= -Mathf.Deg2Rad;

                float x = (position.x - center.x) * Mathf.Cos(angle) - (position.z - center.z) * Mathf.Sin(angle) + center.x;

                float z = (position.x - center.x) * Mathf.Sin(angle) + (position.z - center.z) * Mathf.Cos(angle) + center.z;

                position.x = x;

                position.z = z;

                return position;
            }
            /// <summary>
            /// 相乘
            /// </summary>
            public static Vector3 Multiply(Vector3 a, Vector3 b)
            {
                a.x *= b.x;

                a.y *= b.y;

                a.z *= b.z;

                return a;
            }
            /// <summary>
            /// 法线
            /// </summary>
            public static Vector3 Normal(Vector3 a, Vector3 b, Vector3 c)
            {
                var na = (b.y - a.y) * (c.z - a.z) - (b.z - a.z) * (c.y - a.y);

                var nb = (b.z - a.z) * (c.x - a.x) - (b.x - a.x) * (c.z - a.z);

                var nc = (b.x - a.x) * (c.y - a.y) - (b.y - a.y) * (c.x - a.x);

                return new Vector3(na, nb, nc);
            }
            /// <summary>
            /// 平均值
            /// </summary>
            public static Vector3 Center(params Vector3[] list)
            {
                var result = Vector3.zero;

                var count = list.Length;

                if (count == 0) return result;

                for (int i = 0; i < count; i++)
                {
                    result += list[i];
                }
                return result / count;
            }
        }
    }
}