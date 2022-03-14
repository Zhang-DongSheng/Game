using UnityEngine;

namespace Game
{
    public static partial class Utility
    {
        public static class Vector
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

            public static Vector3 AngleToPosition(float angle, float distance)
            {
                return new Vector3()
                {
                    x = distance * Mathf.Cos(angle * Mathf.Deg2Rad),
                    y = 0,
                    z = distance * Mathf.Sin(angle * Mathf.Deg2Rad)
                };
            }

            public static Quaternion AngleToRotation(float horizontal, float vertical)
            {
                Vector3 vector = new Vector3(horizontal, 0, vertical);

                if (vector != Vector3.zero)
                {
                    return Quaternion.LookRotation(vector, Vector3.up);
                }
                return Quaternion.identity;
            }

            public static float PositionToAngle(Vector3 from, Vector3 to)
            {
                Vector3 vector = to - from;

                vector.y = vector.z;

                return Vector2.SignedAngle(vector, Vector2.up);
            }
        }
    }
}