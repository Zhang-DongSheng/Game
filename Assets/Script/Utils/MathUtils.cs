using UnityEngine;

namespace Utils
{
    public static class MathUtils
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
        /// 随机数
        /// </summary>
        public static int Range(int min, int max)
        {
            return Random.Range(min, max);
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
        /// 已知两点求角度
        /// </summary>
        public static float VectorToAngle(Vector2 center, Vector2 target)
        {
            return Vector2.SignedAngle(target - center, Vector2.up);
        }
        /// <summary>
        /// 已知角度和距离
        /// </summary>
        public static Vector2 AngleToVector(Vector2 center, float angle, float distance)
        {
            Vector2 position = new Vector2()
            {
                x = center.x + distance * Mathf.Cos(angle * Mathf.Deg2Rad),
                y = center.y + distance * Mathf.Sin(angle * Mathf.Deg2Rad)
            };
            return position;
        }
        /// <summary>
        /// 距离
        /// </summary>
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