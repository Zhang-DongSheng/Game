using System;
using System.Collections.Generic;
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
        /// 数字处理
        /// </summary>
        public static float Command(IEnumerable<float> source, NumberCommand command)
        {
            float result = 0;

            int index = 0;

            var enumerator = source.GetEnumerator();

            switch (command)
            {
                case NumberCommand.Sum:
                    {
                        while (enumerator.MoveNext())
                        {
                            result += enumerator.Current;
                        }
                    }
                    break;
                case NumberCommand.Min:
                    {
                        while (enumerator.MoveNext())
                        {
                            if (index++ == 0)
                            {
                                result = enumerator.Current;
                            }
                            else if (result > enumerator.Current)
                            {
                                result = enumerator.Current;
                            }
                        }
                    }
                    break;
                case NumberCommand.Max:
                    {
                        while (enumerator.MoveNext())
                        {
                            if (index++ == 0)
                            {
                                result = enumerator.Current;
                            }
                            else if (result < enumerator.Current)
                            {
                                result = enumerator.Current;
                            }
                        }
                    }
                    break;
            }
            enumerator.Dispose();

            return result;
        }
        /// <summary>
        /// 保留指定小数位
        /// </summary>
        public static float Round(float value, int digit)
        {
            return (float)Math.Round(value, digit);
        }
        /// <summary>
        /// 转化为角度区间
        /// </summary>
        public static float Angle(this float number)
        {
            if (number > 360)
            {
                while (number > 360)
                {
                    number -= 360;
                }
            }
            else if (number < -360)
            {
                while (number < -360)
                {
                    number += 360;
                }
            }
            return number;
        }
    }

    public enum NumberCommand
    {
        Sum,
        Min,
        Max,
    }
}