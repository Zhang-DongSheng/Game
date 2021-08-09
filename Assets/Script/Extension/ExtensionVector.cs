using System;
using UnityEngine;

namespace Game
{
    public static partial class Extension
    {
        /// <summary>
        /// 在...区间内
        /// </summary>
        public static bool Inside(this Vector2 origin, Vector2 destination, float distance)
        {
            length = Math.Pow(origin.x - destination.x, 2) + Math.Pow(origin.y - destination.y, 2);

            return Math.Pow(distance, 2) >= length;
        }
        /// <summary>
        /// 在...区间内
        /// </summary>
        public static bool Inside(this Vector3 origin, Vector3 destination, float distance)
        {
            length = Math.Pow(origin.x - destination.x, 2) + Math.Pow(origin.y - destination.y, 2) + Math.Pow(origin.z - destination.z, 2);

            return Math.Pow(distance, 2) >= length;
        }
        /// <summary>
        /// 距离[开方]
        /// </summary>
        public static float Distance(this Vector2 origin, Vector2 destination)
        {
            length = Math.Pow(origin.x - destination.x, 2) + Math.Pow(origin.y - destination.y, 2);

            return (float)length;
        }
        /// <summary>
        /// 距离[开方]
        /// </summary>
        public static float Distance(this Vector3 origin, Vector3 destination)
        {
            length = Math.Pow(origin.x - destination.x, 2) + Math.Pow(origin.y - destination.y, 2) + Math.Pow(origin.z - destination.z, 2);

            return (float)length;
        }
        /// <summary>
        /// 横向
        /// </summary>
        public static bool IsHorizontal(this Vector2 vector)
        {
            value = Vector2.Angle(vector, Vector2.up);

            return value > ANGLE45 && value < ANGLE135;
        }
        /// <summary>
        /// 纵向
        /// </summary>
        public static bool IsVertical(this Vector2 vector)
        {
            value = Vector2.Angle(vector, Vector2.up);

            return value < ANGLE45 || value > ANGLE135;
        }
    }
}