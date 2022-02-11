using UnityEngine;

namespace Game
{
    public static partial class Utility
    {
        public static class Math
        {
            /// <summary>
            /// ���Լ��
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
            /// ��С������
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
            /// �Ƕ�ת����
            /// </summary>
            public static float AngleToRadian(float angle)
            {
                return angle * Mathf.Deg2Rad;
            }
            /// <summary>
            /// ����ת�Ƕ�
            /// </summary>
            public static float RadianToAngle(float radian)
            {
                return radian * Mathf.Rad2Deg;
            }
            /// <summary>
            /// ����С��λ
            /// </summary>
            public static float Round(float value, int digit)
            {
                return (float)System.Math.Round(value, digit);
            }
        }
    }
}