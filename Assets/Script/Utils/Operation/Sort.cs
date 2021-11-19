using System;
using System.Collections;
using System.Collections.Generic;

namespace Game.Operation
{
    public static class Sort
    {
        #region Function
        public static List<T> MergeSort<T>(List<T> source) where T : IComparer
        {
            return _MergeSort(source, 0, source.Count - 1);
        }

        public static List<T> ShellSort<T>(List<T> source) where T : IComparer
        {
            return _ShellSort(source, 2);
        }

        public static List<T> BubbleSort<T>(List<T> source) where T : IComparer
        {
            return _BubbleSort(source);
        }

        public static List<T> InsertionSort<T>(List<T> source) where T : IComparer
        {
            return _InsertionSort(source);
        }

        public static List<T> SelectionSort<T>(List<T> source) where T : IComparer
        {
            return _SelectionSort(source);
        }

        public static List<T> QuickSort<T>(List<T> source) where T : IComparer
        {
            return _QuickSort(source, 0, source.Count - 1);
        }
        #endregion

        #region Core
        /// <summary>
        /// 归并排序(o(n㏒n))
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">元数据</param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private static List<T> _MergeSort<T>(List<T> source, int min, int max) where T : IComparer
        {
            if (min == max) return new List<T>() { source[min] };

            List<T> list = new List<T>();

            int middle = (min + max) / 2;

            List<T> left = _MergeSort(source, min, middle);

            List<T> right = _MergeSort(source, middle + 1, max);

            int l = 0, r = 0;

            while (true)
            {
                if (left[l].Compare(left[l], right[r]) < 0)
                {
                    list.Add(left[l]);

                    if (++l == left.Count)
                    {
                        list.AddRange(right.GetRange(r, right.Count - r)); break;
                    }
                }
                else
                {
                    list.Add(right[r]);

                    if (++r == right.Count)
                    {
                        list.AddRange(left.GetRange(l, left.Count - l)); break;
                    }
                }
            }

            return list;
        }
        /// <summary>
        /// 希尔排序(o(n¹.¼))
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        private static List<T> _ShellSort<T>(List<T> source, int number) where T : IComparer
        {
            int gap = source.Count / number;

            while (gap >= 1)
            {
                for (int i = gap; i < source.Count; i++)
                {
                    for (int j = i; j >= gap && source[j].Compare(source[j], source[j - gap]) < 0; j -= gap)
                    {
                        Swap(source, j, j - gap);
                    }
                }
                gap /= number;
            }

            return source;
        }
        /// <summary>
        /// 冒泡排序(o(n²))
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        private static List<T> _BubbleSort<T>(List<T> source) where T : IComparer
        {
            for (int i = 0; i < source.Count; i++)
            {
                for (int j = i + 1; j < source.Count; j++)
                {
                    if (source[i].Compare(source[i], source[j]) < 0)
                    {
                        Swap(source, i, j);
                    }
                }
            }
            return source;
        }
        /// <summary>
        /// 插入排序(o(n²))
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="source">数据源</param>
        /// <returns>排序结果</returns>
        private static List<T> _InsertionSort<T>(List<T> source) where T : IComparer
        {
            int index;

            for (int i = 0; i < source.Count; i++)
            {
                T temp = source[i];

                index = i;

                for (; index > 0 && temp.Compare(temp, source[index - 1]) < 0; index--)
                {
                    source[index] = source[index - 1];
                }
                source[index] = temp;
            }

            return source;
        }
        /// <summary>
        /// 选择排序(o(n²))
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        private static List<T> _SelectionSort<T>(List<T> source) where T : IComparer
        {
            int index;

            for (int i = 0; i < source.Count; i++)
            {
                index = i;

                for (int j = i + 1; j < source.Count; j++)
                {
                    if (source[index].Compare(source[index], source[j]) > 0)
                    {
                        index = j;
                    }
                }

                if (index != i)
                {
                    Swap(source, index, i);
                }
            }

            return source;
        }
        /// <summary>
        /// 快速排序(o(n㏒n))
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        private static List<T> _QuickSort<T>(List<T> source, int left, int right) where T : IComparer
        {
            if (left >= right) return null;

            T temp = source[left];

            int i = left, j = right;

            try
            {
                while (i < j)
                {
                    while (i < j && temp.Compare(temp, source[j]) < 0)
                    {
                        j--;
                    }

                    if (i == j) break;

                    source[i++] = source[j];

                    while (i < j && temp.Compare(temp, source[i]) > 0)
                    {
                        i++;
                    }

                    if (i == j) break;

                    source[j--] = source[i];
                }

                source[i] = temp;

                _QuickSort(source, left, i - 1);

                _QuickSort(source, i + 1, right);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return source;
        }
        /// <summary>
        /// 基数排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        private static List<T> _RadixSort<T>(List<T> source) where T : IComparer
        {
            return source;
        }
        /// <summary>
        /// 桶排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        private static List<T> _BucketSort<T>(List<T> source) where T : IComparer
        {
            //for (int i = 0; i < source.Count; i++)
            //{
            //    for (int j = i + 1; j < source.Count; j++)
            //    {
            //        if (source[i].Compare(source[i], source[j]) < 0)
            //        {
            //            Swap(source, i, j);
            //        }
            //    }
            //}
            return source;
        }
        #endregion

        #region Utils
        private static void Swap<T>(List<T> list, int x, int y)
        {
            T temp = list[x];

            list[x] = list[y];

            list[y] = temp;
        }
        #endregion

        class SortStruct : IComparer
        {
            public int ID;

            public int index;

            public int order;

            public int Compare(object x, object y)
            {
                SortStruct x1 = x as SortStruct;

                SortStruct y1 = y as SortStruct;

                return x1.ID > y1.ID ? 1 : -1;
            }
        }
    }
}