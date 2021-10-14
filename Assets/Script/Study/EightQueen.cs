using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Study
{
    /// <summary>
    /// 八皇后问题
    /// </summary>
    public class EightQueen : MonoBehaviour
    {
        private const int column = 8;

        private readonly List<int> array = new List<int>(column) { 0, 0, 0, 0, 0, 0, 0, 0 };

        private int count;

        private void Start()
        {
            Queen(0);
        }

        private void Queen(int number)
        {
            if (number == column)
            {
                count++;

                Print(array);

                return;
            }
            else
            {
                for (int i = 0; i < column; i++)
                {
                    array[number] = i;

                    if (Place(number))
                    {
                        Queen(number + 1);
                    }
                }
            }
        }

        private bool Place(int number)
        {
            for (int i = 0; i < number; i++)
            {
                if (array[i] == array[number] || Math.Abs(number - 1) == Math.Abs(array[number] - array[i]))
                {
                    return false;
                }
            }
            return true;
        }

        private void Print(List<int> list)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("第");

            builder.Append(count);

            builder.Append("种");

            builder.Append("\n");

            for (int i = 0; i < column; i++)
            {
                for (int j = 0; i < column; j++)
                {
                    if (j == list[i])
                    {
                        builder.Append("*");
                    }
                    else
                    {
                        builder.Append("-");
                    }
                }
                builder.Append("\n");
            }

            Debug.LogError(builder.ToString());
        }
    }
}