using System;
using System.Collections.Generic;

public static partial class Extension
{
    public static V FindByIndex<K, V>(this Dictionary<K, V> pairs, int index)
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

    public static V Find<K, V>(this Dictionary<K, V> pairs, Predicate<V> match)
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

    public static List<T> ToList<T>(this string source, char separator)
    {
        if (string.IsNullOrEmpty(source)) return null;

        var list = new List<T>();

        var strs = source.Split(separator);

        foreach (var str in strs)
        {
            list.Add((T)Convert.ChangeType(str, typeof(T)));
        }
        return list;
    }
}