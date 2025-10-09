using System;
using System.Collections.Generic;

namespace Game
{
    public static partial class Extension
    {
        #region Dictionary
        /// <summary>
        /// 索引
        /// </summary>
        public static V IndexOf<K, V>(this ICollection<KeyValuePair<K, V>> pairs, int index)
        {
            if (index > -1 && pairs.Count > index)
            {
                foreach (KeyValuePair<K, V> pair in pairs)
                {
                    if (index-- == 0)
                    {
                        return pair.Value;
                    }
                }
            }
            return default;
        }
        /// <summary>
        /// 批量添加
        /// </summary>
        public static void AddRange<K, V>(this Dictionary<K, V> source, Dictionary<K, V> other, bool replace = false)
        {
            foreach (var item in other)
            {
                if (source.ContainsKey(item.Key) && replace)
                {
                    source[item.Key] = item.Value;
                }
                else
                {
                    source.Add(item.Key, item.Value);
                }
            }
        }
        /// <summary>
        /// 查找
        /// </summary>
        public static V Find<K, V>(this ICollection<KeyValuePair<K, V>> pairs, Predicate<V> match)
        {
            foreach (KeyValuePair<K, V> pair in pairs)
            {
                if (match(pair.Value))
                {
                    return pair.Value;
                }
            }
            return default;
        }
        /// <summary>
        /// 转换为集合
        /// </summary>
        public static List<V> ToList<K, V>(this ICollection<KeyValuePair<K, V>> pairs)
        {
            var list = new List<V>();

            foreach (var pair in pairs)
            {
                list.Add(pair.Value);
            }
            return list;
        }
        #endregion

        #region IEnumerable
        /// <summary>
        /// 遍历
        /// </summary>
        public static void Foreach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (action == null) return;

            var enumerator = source.GetEnumerator();

            while (enumerator.MoveNext())
            {
                action?.Invoke(enumerator.Current);
            }
            enumerator.Dispose();
        }
        /// <summary>
        /// 转换为集合
        /// </summary>
        public static List<T> ToList<T>(this IEnumerable<T> source)
        {
            var list = new List<T>();

            var enumerator = source.GetEnumerator();

            while (enumerator.MoveNext())
            {
                list.Add(enumerator.Current);
            }
            enumerator.Dispose();

            return list;
        }
        /// <summary>
        /// 转换为指定集合
        /// </summary>
        public static List<TResult> ToList<T, TResult>(this IEnumerable<T> source, Func<T, TResult> convert)
        {
            var list = new List<TResult>();

            var enumerator = source.GetEnumerator();

            while (enumerator.MoveNext())
            {
                list.Add(convert(enumerator.Current));
            }
            enumerator.Dispose();

            return list;
        }
        /// <summary>
        /// 非空
        /// </summary>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            if (source == null) return false;

            using (var enumerator = source.GetEnumerator())
            {
                if (enumerator.MoveNext()) return true;
            }
            return false;
        }
        /// <summary>
        /// 位置
        /// </summary>
        public static ListRange Range<T>(this IEnumerable<T> source, T value)
        {
            if (source == null) return ListRange.Outside;

            int count = 0, index = -1;

            using (var enumerator = source.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    if (enumerator.Current.Equals(value))
                    {
                        index = count;
                    }
                    count++;
                }
            }
            // compute
            if (index == -1)
            {
                return ListRange.Outside;
            }
            else if (index == 0)
            {
                return ListRange.First;
            }
            else if (index == count - 1)
            {
                return ListRange.Last;
            }
            else
            {
                return ListRange.Inside;
            }
        }
        #endregion

        #region Array
        public static bool Exist(this int[] array, int value)
        {
            int count = array != null ? array.Length : 0;

            for (int i = 0; i < count; i++)
            {
                if (array[i].Equals(value))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool Exist(this float[] array, float value)
        {
            int count = array != null ? array.Length : 0;

            for (int i = 0; i < count; i++)
            {
                if (array[i].Equals(value))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool Exist(this string[] array, string value)
        {
            int count = array != null ? array.Length : 0;

            for (int i = 0; i < count; i++)
            {
                if (array[i].Equals(value))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool Exist<T>(this T[] array, T value)
        {
            int count = array != null ? array.Length : 0;

            for (int i = 0; i < count; i++)
            {
                if (array[i].Equals(value))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool AllExist(this string[] array, string[] values)
        {
            bool exist = true;

            foreach (string value in values)
            {
                exist = Exist(array, value);

                if (!exist) break;
            }
            return exist;
        }
        #endregion

        #region IList
        /// <summary>
        /// 交换
        /// </summary>
        public static void Swap<T>(this IList<T> list, int from, int to)
        {
            if (from == to)
                return;
            if (list.Count > from && list.Count > to)
            {
                var temp = list[from];
                list[from] = list[to];
                list[to] = temp;
            }
        }
        /// <summary>
        /// 填充
        /// </summary>
        public static void Fill<T>(this IList<T> list, T value, int count = -1)
        {
            if (count == -1)
            {
                count = list.Count;

                for (int i = 0; i < count; i++)
                {
                    list[i] = value;
                }
            }
            else
            {
                list.Clear();

                for (int i = 0; i < count; i++)
                {
                    list.Add(value);
                }
            }
        }
        /// <summary>
        /// 截取
        /// </summary>
        public static List<T> Truncate<T>(this IList<T> list, int index, int length)
        {
            var result = new List<T>(length);

            int count = list.Count;

            int start = 0;

            int last = count - 1;

            int right = length / 2;

            int left = length % 2 == 0 ? right - 1 : right;

            int offset;

            if (index - left < start)
            {
                offset = start - index + left;
            }
            else if (index + right > last)
            {
                offset = last - index - right;
            }
            else
            {
                offset = 0;
            }
            // compute
            for (int i = 0; i < count; i++)
            {
                if (i >= index - left + offset &&
                    i <= index + right + offset)
                {
                    result.Add(list[i]);
                }
            }
            return result;
        }
        #endregion

        #region Stack
        public static bool Exists<T>(this Stack<T> stack, Predicate<T> match)
        {
            foreach (var item in stack)
            {
                if (match(item))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}