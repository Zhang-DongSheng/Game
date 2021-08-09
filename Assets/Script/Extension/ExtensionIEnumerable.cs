using System;
using System.Collections.Generic;

namespace Game
{
    public static partial class Extension
    {
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

        public static List<TValue> ToList<TKey, TValue>(this ICollection<KeyValuePair<TKey, TValue>> pairs)
        {
            List<TValue> list = new List<TValue>();

            foreach (KeyValuePair<TKey, TValue> pair in pairs)
            {
                list.Add(pair.Value);
            }
            return list;
        }

        public static void Foreach<T>(this IList<T> list, Action<T> action)
        {
            if (action == null) return;

            foreach (T item in list)
            {
                action(item);
            }
        }

        public static T First<T>(this IList<T> list, T value = default)
        {
            if (list.Count > 0)
            {
                return list[0];
            }
            return value;
        }

        public static T Last<T>(this IList<T> list, T value = default)
        {
            int count = list.Count;

            if (count > 0)
            {
                return list[count - 1];
            }
            return value;
        }
    }
}