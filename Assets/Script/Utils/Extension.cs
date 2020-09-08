using Data;
using System;
using System.Collections.Generic;

public static class Extension
{
    #region Dictionary
    public static V GetByIndex<K, V>(this Dictionary<K, V> pairs, int index)
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
    #endregion

    #region Number
    public static string NumberToString(this int value, int digit)
    {
        float number;

        string unit = string.Empty;

        if (value < 1000)
        {
            number = value;
        }
        else if (value < 1000000)
        {
            number = value / 1000; unit = "k+";
        }
        else
        {
            number = value / 1000000; unit = "m+";
        }
        return string.Format("{0}{1}", Math.Round(number, digit), unit);
    }
    #endregion
}
