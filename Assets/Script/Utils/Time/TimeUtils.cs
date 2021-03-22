using System;
using System.Collections.Generic;
using System.Globalization;

public static class TimeUtils
{
    private static readonly Dictionary<string, DateTime> timer = new Dictionary<string, DateTime>();

    public static DateTime Now
    {
        get
        {
            return DateTime.Now;
        }
    }

    public static DateTime UtcNow
    {
        get
        {
            return DateTime.UtcNow;
        }
    }

    public static DateTime LocalToUtc(DateTime time)
    { 
        return time - TimeZoneInfo.Local.BaseUtcOffset;
    }

    public static DateTime UtcToLocal(DateTime time)
    {
        return time + TimeZoneInfo.Local.BaseUtcOffset;
    }

    public static void TimBegin(string key)
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

    public static double TimEnd(string key)
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

    public static double TotalSeconds(DateTime begin, DateTime end)
    {
        return begin.Subtract(end).TotalSeconds;
    }

    public static DateTime ToDateTime(long ticket)
    {
        return new DateTime(ticket);
    }

    public static DateTime ToDateTime(string time)
    {
        DateTimeFormatInfo format = new DateTimeFormatInfo()
        {
            ShortDatePattern = "yyyy-MM-dd",
        };
        return Convert.ToDateTime(time, format);
    }

    public static string ToString(DateTime time)
    {
        return time.ToString();
    }
}