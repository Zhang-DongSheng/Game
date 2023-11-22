using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public static partial class Utility
    {
        /// <summary>
        /// 排序算法
        /// </summary>
        public static class Sort
        {
            public static IList<T> SortList<T>(IList<T> collection, Comparer<T> comparer, int type = 0)
            {
                int count = collection.Count;

                if (count < 2) return collection;

                switch (type)
                {
                    case 0:
                        return _MergeSort(collection, 0, count - 1, comparer);
                    case 1:
                        return _ShellSort(collection, comparer);
                    case 2:
                        return _BubbleSort(collection, comparer);
                    case 3:
                        return _InsertionSort(collection, comparer);
                    case 4:
                        return _SelectionSort(collection, comparer);
                    case 5:
                        return _QuickSort(collection, 0, count - 1, comparer);
                    case 6:
                        return _RadixSort(collection, comparer);
                    case 7:
                        return _HeapSort(collection, comparer);
                    case 8:
                        return _CombSort(collection, comparer);
                    case 9:
                        return _GnomeSort(collection, comparer);
                    case 10:
                        return _OddEvenSort(collection, comparer);
                    case 11:
                        return _CycleSort(collection, comparer);
                    default:
                        return collection;
                }
            }
            /// <summary>
            /// 排序By整数
            /// </summary>
            public static IList<int> SortIntList(IList<int> collection, int type = 0)
            {
                int count = collection.Count;

                if (count < 2) return collection;

                switch (type)
                {
                    case 12:
                        return _BucketSort(collection);
                    case 13:
                        return _PigeonHoleSort(collection);
                    default:
                        return SortList(collection, Comparer<int>.Default, type);
                }
            }
            /// <summary>
            /// 归并排序(o(n㏒n))
            /// </summary>
            private static List<T> _MergeSort<T>(IList<T> collection, int min, int max, Comparer<T> comparer)
            {
                if (min == max) return new List<T>() { collection[min] };

                List<T> list = new List<T>();

                int middle = (min + max) / 2;

                List<T> left = _MergeSort(collection, min, middle, comparer);

                List<T> right = _MergeSort(collection, middle + 1, max, comparer);

                int l = 0, r = 0;

                while (true)
                {
                    if (comparer.Compare(left[l], right[r]) < 0)
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
            private static IList<T> _ShellSort<T>(IList<T> collection, Comparer<T> comparer)
            {
                int count = collection.Count;

                int gap = count / 2;

                while (gap >= 1)
                {
                    for (int i = gap; i < count; i++)
                    {
                        for (int j = i; j >= gap && comparer.Compare(collection[j], collection[j - gap]) < 0; j -= gap)
                        {
                            Swap(collection, j, j - gap);
                        }
                    }
                    gap /= 2;
                }
                return collection;
            }
            /// <summary>
            /// 冒泡排序(o(n²))
            /// </summary>
            private static IList<T> _BubbleSort<T>(IList<T> collection, Comparer<T> comparer)
            {
                int count = collection.Count;

                for (int i = 0; i < count; i++)
                {
                    for (int j = i + 1; j < count; j++)
                    {
                        if (comparer.Compare(collection[i], collection[j]) < 0)
                        {
                            Swap(collection, i, j);
                        }
                    }
                }
                return collection;
            }
            /// <summary>
            /// 插入排序(o(n²))
            /// </summary>
            private static IList<T> _InsertionSort<T>(IList<T> collection, Comparer<T> comparer)
            {
                int index;

                for (int i = 0; i < collection.Count; i++)
                {
                    T temp = collection[i];

                    index = i;

                    for (; index > 0 && comparer.Compare(temp, collection[index - 1]) < 0; index--)
                    {
                        collection[index] = collection[index - 1];
                    }
                    collection[index] = temp;
                }
                return collection;
            }
            /// <summary>
            /// 选择排序(o(n²))
            /// </summary>
            private static IList<T> _SelectionSort<T>(IList<T> collection, Comparer<T> comparer)
            {
                int index, count = collection.Count;

                for (int i = 0; i < count; i++)
                {
                    index = i;

                    for (int j = i + 1; j < count; j++)
                    {
                        if (comparer.Compare(collection[index], collection[j]) > 0)
                        {
                            index = j;
                        }
                    }

                    if (index != i)
                    {
                        Swap(collection, index, i);
                    }
                }
                return collection;
            }
            /// <summary>
            /// 快速排序(o(n㏒n))
            /// </summary>
            private static IList<T> _QuickSort<T>(IList<T> collection, int left, int right, Comparer<T> comparer)
            {
                if (left >= right) return null;

                T temp = collection[left];

                int i = left, j = right;

                try
                {
                    while (i < j)
                    {
                        while (i < j && comparer.Compare(temp, collection[j]) < 0)
                        {
                            j--;
                        }
                        if (i == j) break;

                        collection[i++] = collection[j];

                        while (i < j && comparer.Compare(temp, collection[i]) > 0)
                        {
                            i++;
                        }
                        if (i == j) break;

                        collection[j--] = collection[i];
                    }
                    collection[i] = temp;

                    _QuickSort(collection, left, i - 1, comparer);

                    _QuickSort(collection, i + 1, right, comparer);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                return collection;
            }
            /// <summary>
            /// 基数排序
            /// </summary>
            private static IList<T> _RadixSort<T>(IList<T> collection, Comparer<T> comparer)
            {
                return collection;
            }
            /// <summary>
            /// 堆排序
            /// </summary>
            private static IList<T> _HeapSort<T>(IList<T> collection, Comparer<T> comparer)
            {
                int last = collection.Count - 1;

                for (int node = last / 2; node >= 0; --node)
                {
                    _HeapSortMinHeapify(collection, node, last, comparer);
                }
                while (last >= 0)
                {
                    collection.Swap(0, last);
                    last--;
                    _HeapSortMinHeapify(collection, 0, last, comparer);
                }
                return collection;
            }
            /// <summary>
            /// 堆排序 of Max
            /// </summary>
            private static void _HeapSortMaxHeapify<T>(IList<T> collection, int nodeIndex, int lastIndex, Comparer<T> comparer)
            {
                // assume left(i) and right(i) are max-heaps
                int left = (nodeIndex * 2) + 1;
                int right = left + 1;
                int largest = nodeIndex;

                // If collection[left] > collection[nodeIndex]
                if (left <= lastIndex && comparer.Compare(collection[left], collection[nodeIndex]) > 0)
                    largest = left;

                // If collection[right] > collection[largest]
                if (right <= lastIndex && comparer.Compare(collection[right], collection[largest]) > 0)
                    largest = right;

                // Swap and heapify
                if (largest != nodeIndex)
                {
                    collection.Swap(nodeIndex, largest);
                    _HeapSortMaxHeapify(collection, largest, lastIndex, comparer);
                }
            }
            /// <summary>
            /// 堆排序 of Min
            /// </summary>
            private static void _HeapSortMinHeapify<T>(IList<T> collection, int nodeIndex, int lastIndex, Comparer<T> comparer)
            {
                // assume left(i) and right(i) are max-heaps
                int left = (nodeIndex * 2) + 1;
                int right = left + 1;
                int smallest = nodeIndex;
                // If collection[left] > collection[nodeIndex]
                if (left <= lastIndex && comparer.Compare(collection[left], collection[nodeIndex]) < 0)
                    smallest = left;
                // If collection[right] > collection[largest]
                if (right <= lastIndex && comparer.Compare(collection[right], collection[smallest]) < 0)
                    smallest = right;
                // Swap and heapify
                if (smallest != nodeIndex)
                {
                    collection.Swap(nodeIndex, smallest);
                    _HeapSortMinHeapify(collection, smallest, lastIndex, comparer);
                }
            }
            /// <summary>
            /// 梳子排序
            /// </summary>
            private static IList<T> _CombSort<T>(IList<T> collection, Comparer<T> comparer)
            {
                double gap = collection.Count;
                bool swaps = true;
                while (gap > 1 || swaps)
                {
                    gap /= 1.247330950103979;
                    if (gap < 1) { gap = 1; }
                    int i = 0;
                    swaps = false;
                    while (i + gap < collection.Count)
                    {
                        int igap = i + (int)gap;
                        if (comparer.Compare(collection[i], collection[igap]) < 0)
                        {
                            collection.Swap(i, igap);
                            swaps = true;
                        }
                        i++;
                    }
                }
                return collection;
            }
            /// <summary>
            /// 侏儒排序
            /// </summary>
            private static IList<T> _GnomeSort<T>(IList<T> collection, Comparer<T> comparer)
            {
                int index = 1;

                while (index < collection.Count)
                {
                    if (comparer.Compare(collection[index], collection[index - 1]) <= 0)
                    {
                        index++;
                    }
                    else
                    {
                        collection.Swap(index, index - 1);

                        if (index > 1)
                        {
                            index--;
                        }
                    }
                }
                return collection;
            }
            /// <summary>
            /// 奇偶排序
            /// </summary>
            private static IList<T> _OddEvenSort<T>(IList<T> collection, Comparer<T> comparer)
            {
                bool sorted = false;
                while (!sorted)
                {
                    sorted = true;
                    for (var i = 1; i < collection.Count - 1; i += 2)
                    {
                        if (comparer.Compare(collection[i], collection[i + 1]) < 0)
                        {
                            collection.Swap(i, i + 1);
                            sorted = false;
                        }
                    }
                    for (var i = 0; i < collection.Count - 1; i += 2)
                    {
                        if (comparer.Compare(collection[i], collection[i + 1]) < 0)
                        {
                            collection.Swap(i, i + 1);
                            sorted = false;
                        }
                    }
                }
                return collection;
            }
            /// <summary>
            /// 圈排序
            /// </summary>
            private static IList<T> _CycleSort<T>(IList<T> collection, Comparer<T> comparer)
            {
                int count = collection.Count;

                for (int i = 0; i < count; i++)
                {
                    T item = collection[i];
                    int position = i;
                    do
                    {
                        int to = 0;
                        for (int j = 0; j < count; j++)
                        {
                            if (j != i && comparer.Compare(collection[j], item) < 0)
                            {
                                to++;
                            }
                        }

                        if (position != to)
                        {
                            while (position != to && comparer.Compare(item, collection[to]) == 0)
                            {
                                to++;
                            }
                            T temp = collection[to];
                            collection[to] = item;
                            item = temp;
                            position = to;
                        }
                    } while (position != i);
                }
                return collection;
            }
            /// <summary>
            /// 桶排序
            /// </summary>
            private static IList<int> _BucketSort(IList<int> collection)
            {
                int max = collection.Max();

                int min = collection.Min();

                List<int>[] bucket = new List<int>[max - min + 1];

                for (int i = 0; i < bucket.Length; i++)
                {
                    bucket[i] = new List<int>();
                }
                foreach (var i in collection)
                {
                    bucket[i - min].Add(i);
                }

                int index = 0;

                foreach (List<int> i in bucket)
                {
                    if (i.Count > 0)
                    {
                        foreach (int j in i)
                        {
                            collection[index] = j;
                            index++;
                        }
                    }
                }
                return collection;
            }
            /// <summary>
            /// 鸽巢排序
            /// </summary>
            private static IList<int> _PigeonHoleSort(IList<int> collection)
            {
                int min = collection.Min();
                int max = collection.Max();
                int size = max - min + 1;
                int[] holes = new int[size];
                foreach (int x in collection)
                {
                    holes[x - min]++;
                }
                int i = 0;
                for (int count = size - 1; count >= 0; count--)
                {
                    while (holes[count]-- > 0)
                    {
                        collection[i] = count + min;
                        i++;
                    }
                }
                return collection;
            }
            /// <summary>
            /// 交换
            /// </summary>
            private static void Swap<T>(IList<T> list, int x, int y)
            {
                T temp = list[x];

                list[x] = list[y];

                list[y] = temp;
            }
        }
    }
}