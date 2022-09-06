using UnityEngine;

namespace Game
{
    public static partial class Utility
    {
        /// <summary>
        /// 数学
        /// </summary>
        public static class Math
        {
            /// <summary>
            /// 最大公约数
            /// </summary>
            public static int GreatestCommonDivisor(int num1, int num2)
            {
                int temp;

                if (num1 < num2)
                {
                    temp = num1; num1 = num2; num2 = temp;
                }

                while (num2 != 0)
                {
                    temp = num1 % num2;

                    num1 = num2;

                    num2 = temp;
                }
                return num1;
            }
            /// <summary>
            /// 最小公倍数
            /// </summary>
            public static int LeastCommonMultiple(int num1, int num2)
            {
                int temp;

                int num3 = num1 * num2;

                if (num1 < num2)
                {
                    temp = num1; num1 = num2; num2 = temp;
                }

                while (num2 != 0)
                {
                    temp = num1 % num2;

                    num1 = num2;

                    num2 = temp;
                }
                return num3 / num1;
            }
            /// <summary>
            /// 角度转弧度
            /// </summary>
            public static float AngleToRadian(float angle)
            {
                return angle * Mathf.Deg2Rad;
            }
            /// <summary>
            /// 弧度转角度
            /// </summary>
            public static float RadianToAngle(float radian)
            {
                return radian * Mathf.Rad2Deg;
            }
            /// <summary>
            /// 转化为角度区间
            /// </summary>
            public static float AngleIn360(float value)
            {
                if (value > 360)
                {
                    while (value > 360)
                    {
                        value -= 360;
                    }
                }
                else if (value < 0)
                {
                    while (value < 0)
                    {
                        value += 360;
                    }
                }
                return value;
            }
            /// <summary>
            /// 保留小数位
            /// </summary>
            public static float Round(float value, int digit)
            {
                return (float)System.Math.Round(value, digit);
            }
            /// <summary>
            /// 求和
            /// </summary>
            public static float Sum(params float[] numbers)
            {
                float value = 0;

                int length = numbers != null ? numbers.Length : 0;

                for (int i = 0; i < length; i++)
                {
                    value += numbers[i];
                }
                return value;
            }
            /// <summary>
            /// 最小值
            /// </summary>
            public static float Min(params float[] numbers)
            {
                float value;

                int length = numbers != null ? numbers.Length : 0;

                value = length > 0 ? numbers[0] : 0;

                for (int i = 1; i < length; i++)
                {
                    if (value > numbers[i])
                    {
                        value = numbers[i];
                    }
                }
                return value;
            }
            /// <summary>
            /// 最大值
            /// </summary>
            public static float Max(params float[] numbers)
            {
                float value;

                int length = numbers != null ? numbers.Length : 0;

                value = length > 0 ? numbers[0] : 0;

                for (int i = 1; i < length; i++)
                {
                    if (value < numbers[i])
                    {
                        value = numbers[i];
                    }
                }
                return value;
            }
            /// <summary>
            /// 平均数
            /// </summary>
            public static float Average(params float[] numbers)
            {
                float value = 0;

                int length = numbers != null ? numbers.Length : 0;

                for (int i = 0; i < length; i++)
                {
                    value += numbers[i];
                }

                if (value > 1)
                {
                    return value / length;
                }
                return value;
            }
        }
    }
}