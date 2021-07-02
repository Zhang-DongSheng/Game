using System;
using UnityEngine;

namespace Game
{
    public static partial class NumberExtension
    {
        public const int GB = 1024 * 1024 * 1024;

        public const int MB = 1024 * 1024;

        public const int KB = 1024;

        private static double value;

        public static int Between(this int self, int min, int max)
        {
            if (self < min)
            {
                self = min;
            }
            else if (self > max)
            {
                self = max;
            }
            return self;
        }

        public static float Between(this float self, float min, float max)
        {
            if (self < min)
            {
                self = min;
            }
            else if (self > max)
            {
                self = max;
            }
            return self;
        }

        public static double Between(this double self, double min, double max)
        {
            if (self < min)
            {
                self = min;
            }
            else if (self > max)
            {
                self = max;
            }
            return self;
        }

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

        public static float Distance(this float origin, float destination)
        {
            return Math.Abs(origin - destination);
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

        public static string ToNumber(this int self, int digit)
        {
            float number;

            string unit = string.Empty;

            if (self < 1000)
            {
                number = self;
            }
            else if (self < 1000000)
            {
                number = self / 1000; unit = "k+";
            }
            else
            {
                number = self / 1000000; unit = "m+";
            }
            return string.Format("{0}{1}", Math.Round(number, digit), unit);
        }

        public static string ToSize(this long self)
        {
            float value; string unit;

            if (self >= GB)
            {
                value = self / GB; unit = "G";
            }
            else if (self >= MB)
            {
                value = self / MB; unit = "M";
            }
            else if (self >= KB)
            {
                value = self / KB; unit = "K";
            }
            else
            {
                value = self; unit = "B";
            }

            return string.Format("{0}{1}", value, unit);
        }

        public static string ToTimeSpan(this float second)
        {
            TimeSpan span = TimeSpan.FromSeconds(second);

            return span.ToString();
        }
    }
}