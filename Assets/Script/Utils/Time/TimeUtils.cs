using System;
using System.Globalization;

namespace UnityEngine
{
    public static class TimeUtils
    {
        private static readonly DateTime Base = new DateTime(621355968000000000);

        private static TimeSpan span;

        public static DateTime LocalToUtc(DateTime time)
        {
            return time.ToUniversalTime();
        }

        public static DateTime UtcToLocal(DateTime time)
        {
            return time + TimeZoneInfo.Local.BaseUtcOffset;
        }

        public static DateTime ToDateTime(long ticket)
        {
            return Base.AddMilliseconds(ticket);
        }

        public static DateTime ToDateTime(string time)
        {
            DateTimeFormatInfo format = new DateTimeFormatInfo()
            {
                ShortDatePattern = "yyyy-MM-dd",
            };
            return Convert.ToDateTime(time, format);
        }

        public static TimeSpan FormatTicks(long ticks)
        {
            return TimeSpan.FromTicks(ticks);
        }

        public static TimeSpan FormatSecond(float second)
        {
            return TimeSpan.FromSeconds(second);
        }

        public static double TotalSeconds(DateTime begin, DateTime end)
        {
            return begin.Subtract(end).TotalSeconds;
        }

        public static float Remaining(long ticks)
        {
            span = Base.AddMilliseconds(ticks) - DateTime.UtcNow;

            return (float)span.TotalSeconds;
        }

        public static string ToString(DateTime time)
        {
            return time.ToString();
        }

        public static string ToString(TimeSpan span)
        {
            return span.ToString();
        }
    }
}