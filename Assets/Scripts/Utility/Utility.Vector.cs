using UnityEngine;

namespace Game
{
    public static partial class Utility
    {
        public static class _Vector
        {
            private const float Min = 45f;

            private const float Max = 135f;

            public static bool Horizontal(Vector2 vector)
            {
                float angle = Vector2.Angle(vector, Vector2.up);

                return angle > Min && angle < Max;
            }

            public static bool Vertical(Vector2 vector)
            {
                float angle = Vector2.Angle(vector, Vector2.up);

                return angle < Min || angle > Max;
            }

            public static Vector2 Vector3To2(Vector3 vector)
            {
                return new Vector2(vector.x, vector.z);
            }

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
        }
    }
}