using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Operation
{
    public static class Geometry
    {
        private static Vector3 velocity;

        public static Vector3 Move(Vector3 position, Vector3 direction)
        {
            return position + direction * Time.deltaTime;
        }

        public static Vector3 Move(Vector3 position, Vector3 direction, float speed)
        {
            return position + direction * Time.deltaTime * speed;
        }

        public static Vector3 MoveTowards(Vector3 from, Vector3 to, float delta)
        {
            return Vector3.MoveTowards(from, to, delta);
        }

        public static Vector3 Lerp(Vector3 from, Vector3 to, float step)
        {
            return Vector3.Lerp(from, to, step);
        }

        public static Vector3 LerpUnclamped(Vector3 from, Vector3 to, float step)
        {
            return Vector3.LerpUnclamped(from, to, step);
        }

        public static Vector3 Smooth(Vector3 from, Vector3 to, float speed)
        {
            return Vector3.SmoothDamp(from, to, ref velocity, Time.deltaTime, speed);
        }

        public static Vector3 Rotate(Vector3 from, Vector3 to, float step)
        {
            return Vector3.Slerp(from, to, step);
        }

        public static Quaternion Rotate(Quaternion from, Quaternion to, float step)
        {
            return Quaternion.SlerpUnclamped(from, to, step);
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