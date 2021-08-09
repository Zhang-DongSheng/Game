using System;

namespace Game
{
    public static partial class Extension
    {
        static private string unit;
        /// <summary>
        /// 在...之间
        /// </summary>
        public static int Between(this int number, int min, int max)
        {
            if (number < min)
            {
                number = min;
            }
            else if (number > max)
            {
                number = max;
            }
            return number;
        }
        /// <summary>
        /// 在...之间
        /// </summary>
        public static float Between(this float number, float min, float max)
        {
            if (number < min)
            {
                number = min;
            }
            else if (number > max)
            {
                number = max;
            }
            return number;
        }
        /// <summary>
        /// 求和
        /// </summary>
        public static int Sum(this int number, params int[] paramters)
        {
            for (int i = 0; i < paramters.Length; i++)
            {
                number += paramters[i];
            }
            return number;
        }
        /// <summary>
        /// 求和
        /// </summary>
        public static float Sum(this float number, params float[] paramters)
        {
            for (int i = 0; i < paramters.Length; i++)
            {
                number += paramters[i];
            }
            return number;
        }
        /// <summary>
        /// 求和
        /// </summary>
        public static long Sum(this long number, params long[] paramters)
        {
            for (int i = 0; i < paramters.Length; i++)
            {
                number += paramters[i];
            }
            return number;
        }
        /// <summary>
        /// 求差
        /// </summary>
        public static int Difference(this int number, int compare)
        {
            return Math.Abs(number - compare);
        }
        /// <summary>
        /// 求差
        /// </summary>
        public static float Difference(this float number, float compare)
        {
            return Math.Abs(number - compare);
        }
        /// <summary>
        /// 显示数量
        /// </summary>
        public static string ToNumber(this int number, int digit)
        {
            if (number < 1000)
            {
                value = number;
            }
            else if (number < 1000000)
            {
                value = number / 1000; unit = "k+";
            }
            else
            {
                value = number / 1000000; unit = "m+";
            }
            return string.Format("{0}{1}", Math.Round(value, digit), unit);
        }
        /// <summary>
        /// 显示大小
        /// </summary>
        public static string ToSize(this long number)
        {
            if (number >= GB)
            {
                value = number / GB; unit = "G";
            }
            else if (number >= MB)
            {
                value = number / MB; unit = "M";
            }
            else if (number >= KB)
            {
                value = number / KB; unit = "K";
            }
            else
            {
                value = number; unit = "B";
            }
            return string.Format("{0}{1}", value, unit);
        }
        /// <summary>
        /// 显示时间
        /// </summary>
        public static string ToTime(this int seconds)
        {
            TimeSpan span = TimeSpan.FromSeconds(seconds);

            if (span.Days > 0)
            {
                value = span.Days; unit = "Day";
            }
            else if (span.Hours > 0)
            {
                value = span.Hours; unit = "h";
            }
            else if (span.Minutes > 0)
            {
                value = span.Minutes; unit = "m";
            }
            else
            {
                value = seconds; unit = "s";
            }
            return string.Format("{0}{1}", value, unit);
        }
    }
}