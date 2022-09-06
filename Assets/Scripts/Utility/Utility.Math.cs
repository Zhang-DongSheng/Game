using UnityEngine;

namespace Game
{
    public static partial class Utility
    {
        /// <summary>
        /// ��ѧ
        /// </summary>
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
            /// ת��Ϊ�Ƕ�����
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
            /// ����С��λ
            /// </summary>
            public static float Round(float value, int digit)
            {
                return (float)System.Math.Round(value, digit);
            }
            /// <summary>
            /// ���
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
            /// ��Сֵ
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
            /// ���ֵ
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
            /// ƽ����
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