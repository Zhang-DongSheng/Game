using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class TransformUtils
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

        public static Vector3 MoveTowards1(Vector3 from, Vector3 to, float step)
        {
            return Vector3.Lerp(from, to, step);
        }

        public static Vector3 MoveTowards2(Vector3 from, Vector3 to, float step)
        {
            return Vector3.LerpUnclamped(from, to, step);
        }

        public static Vector3 MoveTowards3(Vector3 from, Vector3 to, float speed)
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
    }
}