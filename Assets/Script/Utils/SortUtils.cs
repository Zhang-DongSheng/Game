using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortUtils
{
    #region Function
    public static List<T> MergeSort<T>(List<T> source) where T : IComparer<T>
    {
        return MergeSort(source, 0, source.Count - 1);
    }
    #endregion

    #region Core
    /// <summary>
    /// 归并排序
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source">元数据</param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    private static List<T> MergeSort<T>(List<T> source, int min, int max) where T : IComparer<T>
    {
        if (min == max) return new List<T>() { source[min] };

        List<T> list = new List<T>();

        int middle = (min + max) / 2;

        List<T> left = MergeSort(source, min, middle);

        List<T> right = MergeSort(source, middle + 1, max);

        int l = 0, r = 0;

        while (true)
        {

            if (left[l].Compare(left[l], right[r]) < 0)
            {
                list.Add(left[l]);

                if (++l == left.Count)
                {
                    list.AddRange(right.GetRange(r, right.Count)); break;
                }
            }
            else
            {
                list.Add(right[r]);

                if (++r == right.Count)
                {
                    list.AddRange(left.GetRange(l, left.Count)); break;
                }
            }
        }

        return list;
    }

    /// <summary>
    /// 希尔排序
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="number"></param>
    /// <returns></returns>
    private static List<T> ShellSort<T>(List<T> source, int number = 3) where T : IComparer<T>
    {
        return source;
    }
    #endregion
}