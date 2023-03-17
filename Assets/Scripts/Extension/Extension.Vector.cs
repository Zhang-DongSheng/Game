using System;
using UnityEngine;

namespace Game
{
    public static partial class Extension
    {
        /// <summary>
        /// 三维向量转二维
        /// </summary>
        public static Vector2 Vector3To2(this Vector3 vector)
        {
            return new Vector2()
            {
                x = vector.x,
                y = vector.z,
            };
        }
        /// <summary>
        /// 二维向量转三维
        /// </summary>
        public static Vector3 Vector2To3(this Vector2 vector, float height = 0)
        {
            return new Vector3()
            {
                x = vector.x,
                y = height,
                z = vector.y,
            };
        }
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

            return value > Angle45 && value < Angle135;
        }
        /// <summary>
        /// 纵向
        /// </summary>
        public static bool IsVertical(this Vector2 vector)
        {
            value = Vector2.Angle(vector, Vector2.up);

            return value < Angle45 || value > Angle135;
        }
    }
}