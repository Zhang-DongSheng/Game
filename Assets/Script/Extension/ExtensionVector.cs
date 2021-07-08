using System;
using UnityEngine;

namespace Game
{
    public static partial class Extension
    {
        private const float ANGLEMIN = 45f;

        private const float ANGLEMAX = 135f;

        private static double value;

        public static bool Inside(this Vector2 origin, Vector2 destination, float distance)
        {
            value = Math.Pow(origin.x - destination.x, 2) + Math.Pow(origin.y - destination.y, 2);

            return Math.Pow(distance, 2) >= value;
        }

        public static bool Inside(this Vector3 origin, Vector3 destination, float distance)
        {
            value = Math.Pow(origin.x - destination.x, 2) + Math.Pow(origin.y - destination.y, 2) + Math.Pow(origin.z - destination.z, 2);

            return Math.Pow(distance, 2) >= value;
        }

        public static float Distance(this Vector2 origin, Vector2 destination)
        {
            value = Math.Pow(origin.x - destination.x, 2) + Math.Pow(origin.y - destination.y, 2);

            return (float)value;
        }

        public static float Distance(this Vector3 origin, Vector3 destination)
        {
            value = Math.Pow(origin.x - destination.x, 2) + Math.Pow(origin.y - destination.y, 2) + Math.Pow(origin.z - destination.z, 2);

            return (float)value;
        }

        public static bool IsHorizontal(this Vector2 vector)
        {
            value = Vector2.Angle(vector, Vector2.up);

            return value > ANGLEMIN && value < ANGLEMAX;
        }

        public static bool IsVertical(this Vector2 vector)
        {
            value = Vector2.Angle(vector, Vector2.up);

            return value < ANGLEMIN || value > ANGLEMAX;
        }
    }
}