using System;
using System.Collections.Generic;

namespace Game
{
    public static partial class Extension
    {
        public static TValue IndexOf<TKey, TValue>(this Dictionary<TKey, TValue> pairs, int index)
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

        public static TValue Find<TKey, TValue>(this Dictionary<TKey, TValue> pairs, Predicate<TValue> match)
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

        public static List<TValue> ToList<TKey, TValue>(this Dictionary<TKey, TValue> pairs)
        {
            List<TValue> list = new List<TValue>();

            foreach (KeyValuePair<TKey, TValue> pair in pairs)
            {
                list.Add(pair.Value);
            }
            return list;
        }
    }
}