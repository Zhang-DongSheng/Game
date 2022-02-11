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
        public static TValue IndexOf<TKey, TValue>(this ICollection<KeyValuePair<TKey, TValue>> pairs, int index)
        {
            if (index > -1 && pairs.Count > index)
            {
                foreach (KeyValuePair<TKey, TValue> pair in pairs)
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
        /// 查找
        /// </summary>
        public static TValue Find<TKey, TValue>(this ICollection<KeyValuePair<TKey, TValue>> pairs, Predicate<TValue> match)
        {
            foreach (KeyValuePair<TKey, TValue> pair in pairs)
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
        public static List<TValue> ToList<TKey, TValue>(this ICollection<KeyValuePair<TKey, TValue>> pairs)
        {
            var list = new List<TValue>();

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
        #endregion
    }
}