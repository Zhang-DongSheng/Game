using System;
using System.Collections.Generic;

public static partial class Extension
{
    public static V IndexOf<K, V>(this Dictionary<K, V> pairs, int index)
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

    public static List<V> ToList<K, V>(this Dictionary<K, V> pairs)
    {
        List<V> list = new List<V>();

        foreach (KeyValuePair<K, V> pair in pairs)
        {
            list.Add(pair.Value);
        }
        return list;
    }
}