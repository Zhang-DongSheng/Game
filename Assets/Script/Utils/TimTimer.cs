using System;
using System.Collections.Generic;

public class TimTimer
{
    private static readonly Dictionary<string, DateTime> timer = new Dictionary<string, DateTime>();

    public static void Begin(string key)
    {
        if (timer.ContainsKey(key))
        {
            timer[key] = DateTime.Now;
        }
        else
        {
            timer.Add(key, DateTime.Now);
        }
    }

    public static double End(string key)
    {
        if (timer.ContainsKey(key))
        {
            return DateTime.Now.Subtract(timer[key]).TotalMilliseconds;
        }
        else
        {
            return 0;
        }
    }
}