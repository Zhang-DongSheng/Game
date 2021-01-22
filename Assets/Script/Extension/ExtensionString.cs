using System;
using System.Collections.Generic;
using System.Text;

public static partial class Extension
{
    public static string Append(this string self, params string[] values)
    {
        StringBuilder builder = new StringBuilder(self);

        for(int i = 0;i<values.Length;i++)
        {
            builder.Append(values[i]);
        }
        return builder.ToString();
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

    public static string ToUpperFirst(this string text)
    {
        if (text.Length > 0)
        {
            string first = text.Substring(0, 1);

            if (!string.IsNullOrEmpty(first))
            {
                first = first.ToUpperInvariant();
            }

            text = first.Append(text.Remove(0, 1));
        }
        return text;
    }
}