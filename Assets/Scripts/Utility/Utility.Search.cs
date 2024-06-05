using System;
using System.Collections.Generic;

namespace Game
{
    public static partial class Utility
    {
        /// <summary>
        /// 查找
        /// </summary>
        public static class Search
        {
            /// <summary>
            /// 顺序查找 <para>顺序查找也称为线形查找，属于无序查找算法</para>
            /// </summary>
            public static T SequenceSearch<T>(IList<T> list, Predicate<T> match)
            {
                int count = list == null ? 0 : list.Count;

                if (count == 0) return default;

                for (int i = 0; i < count; i++)
                {
                    if (match(list[i]))
                    {
                        return list[i];
                    }
                }
                return default;
            }
            /// <summary>
            /// 二分查找 <para>元素必须是有序的，属于有序查找算法</para> 
            /// </summary>
            public static T BinarySearch<T>(List<T> list, Func<T, int> match)
            {
                int count = list == null ? 0 : list.Count;

                if (count == 0) return default;

                int min = 0, max = count - 1;

                if (match(list[min]) > 0 ||
                    match(list[max]) < 0)
                {
                    return default;
                }
                int middle, result;

                while (min <= max)
                {
                    // 位运算，x >> 1 相当于 x / 2
                    middle = (min + max) >> 1;

                    result = match(list[middle]);

                    if (result == 0)
                    {
                        return list[middle];
                    }
                    else if (result > 0)
                    {
                        max = middle - 1;
                    }
                    else
                    {
                        min = middle + 1;
                    }
                }
                return default;
            }
        }
    }
}