using UnityEngine;

namespace Utils
{
    public static class MathUtils
    {
        /// <summary>
        /// 最大公约数
        /// </summary>
        /// <param name="_"></param>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns></returns>
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
        /// <param name="_"></param>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns></returns>
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
        /// 随机数
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int Range(int min, int max)
        {
            return Random.Range(min, max);
        }
        /// <summary>
        /// 角度转弧度
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static float AngleToRadian(float angle)
        {
            return angle * Mathf.Deg2Rad;
        }
        /// <summary>
        /// 弧度转角度
        /// </summary>
        /// <param name="radian"></param>
        /// <returns></returns>
        public static float RadianToAngle(float radian)
        {
            return radian * Mathf.Rad2Deg;
        }
        /// <summary>
        /// 距离
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns></returns>
        public static float Distance(float num1, float num2)
        {
            float temp;

            if (num1 < num2)
            {
                temp = num1;
                num1 = num2;
                num2 = temp;
            }

            return num1 - num2;
        }
    }
}