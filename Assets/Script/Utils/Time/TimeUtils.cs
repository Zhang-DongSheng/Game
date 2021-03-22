using System;
using System.Globalization;
using System.Text;

namespace UnityEngine
{
    public static class TimeUtils
    {
        private static readonly StringBuilder builder = new StringBuilder();

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