using System;
using UnityEngine;

namespace Game
{
    public static partial class Extension
    {
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

        public static Rect Scale(this Rect rect, float scale)
        {
            return rect.Scale(scale, rect.center);
        }

        public static Rect Scale(this Rect rect, float scale, Vector2 pivot)
        {
            Rect result = rect;
            result.x -= pivot.x;
            result.y -= pivot.y;
            result.xMin *= scale;
            result.xMax *= scale;
            result.yMin *= scale;
            result.yMax *= scale;
            result.x += pivot.x;
            result.y += pivot.y;
            return result;
        }

        public static Rect Scale(this Rect rect, Vector2 scale)
        {
            return rect.Scale(scale, rect.center);
        }

        public static Rect Scale(this Rect rect, Vector2 scale, Vector2 pivot)
        {
            Rect result = rect;
            result.x -= pivot.x;
            result.y -= pivot.y;
            result.xMin *= scale.x;
            result.xMax *= scale.x;
            result.yMin *= scale.y;
            result.yMax *= scale.y;
            result.x += pivot.x;
            result.y += pivot.y;
            return result;
        }
    }
}