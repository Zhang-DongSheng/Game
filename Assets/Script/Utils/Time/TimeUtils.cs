using System;
using System.Globalization;

namespace UnityEngine
{
    public static class TimeUtils
    {
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

        public static double TotalSeconds(DateTime begin, DateTime end)
        {
            return begin.Subtract(end).TotalSeconds;
        }

        public static string ToString(DateTime time)
        {
            return time.ToString();
        }
    }
}