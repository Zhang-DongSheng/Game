using System;
using UnityEngine;

namespace Game
{
    public static partial class Extension
    {
        /// <summary>
        /// 在...之间
        /// </summary>
        public static int Clamp(this int value, int min, int max)
        {
            value = Mathf.Clamp(value, min, max);

            return value;
        }
        /// <summary>
        /// 在...之间
        /// </summary>
        public static float Clamp(this float value, float min, float max)
        {
            value = Mathf.Clamp(value, min, max);

            return value;
        }
        /// <summary>
        /// 进度
        /// </summary>
        public static float Progress(this float value, float max, float min = 0)
        {
            float interval = max - min;

            if (interval == 0) return 0;

            value -= min;

            return value / interval;
        }
        /// <summary>
        /// 求和
        /// </summary>
        public static int Sum(this int value, params int[] paramters)
        {
            for (int i = 0; i < paramters.Length; i++)
            {
                value += paramters[i];
            }
            return value;
        }
        /// <summary>
        /// 求和
        /// </summary>
        public static float Sum(this float value, params int[] paramters)
        {
            for (int i = 0; i < paramters.Length; i++)
            {
                value += paramters[i];
            }
            return value;
        }
        /// <summary>
        /// 求差
        /// </summary>
        public static int Difference(this int from, int to)
        {
            return Math.Abs(from - to);
        }
        /// <summary>
        /// 求差
        /// </summary>
        public static float Difference(this float from, float to)
        {
            return Math.Abs(from - to);
        }
        /// <summary>
        /// 保留指定小数位
        /// </summary>
        public static float Round(this float value, int digit)
        {
            return (float)Math.Round(value, digit);
        }
        /// <summary>
        /// 是否大于
        /// </summary>
        public static bool MoreThan<T>(this T source, T target) where T : IComparable<T>
        {
            return source.CompareTo(target) > 0;
        }
        /// <summary>
        /// 是否小于
        /// </summary>
        public static bool LessThan<T>(this T source, T target) where T : IComparable<T>
        {
            return source.CompareTo(target) < 0;
        }
    }
}