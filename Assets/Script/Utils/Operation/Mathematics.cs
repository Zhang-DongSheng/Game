using UnityEngine;

namespace Game.Operation
{
    public static class Mathematics
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
            return UnityEngine.Random.Range(min, max);
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

        public static Vector3 AngleToPosition(float angle, float distance)
        {
            return new Vector3()
            {
                x = distance * Mathf.Cos(angle * Mathf.Deg2Rad),
                y = 0,
                z = distance * Mathf.Sin(angle * Mathf.Deg2Rad)
            };
        }

        public static Quaternion AngleToRotation(float horizontal, float vertical)
        {
            Vector3 vector = new Vector3(horizontal, 0, vertical);

            if (vector != Vector3.zero)
            {
                return Quaternion.LookRotation(vector, Vector3.up);
            }
            return Quaternion.identity;
        }

        public static float PositionToAngle(Vector3 from, Vector3 to)
        {
            Vector3 vector = to - from;

            vector.y = vector.z;

            return Vector2.SignedAngle(vector, Vector2.up);
        }
    }
}