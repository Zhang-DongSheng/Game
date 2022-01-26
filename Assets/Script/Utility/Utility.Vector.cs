using UnityEngine;

namespace Game
{
    public static partial class Utility
    {
        public static class Vector
        {
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