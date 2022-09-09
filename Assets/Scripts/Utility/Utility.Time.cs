using System;
using System.Globalization;
using System.Text;

namespace Game
{
    public static partial class Utility
    {
        /// <summary>
        /// Ê±¼ä
        /// </summary>
        public static class Time
        {
            private static readonly StringBuilder builder = new StringBuilder();

            private static readonly DateTime source = new DateTime(1970, 1, 1);

            private static TimeSpan span;

            public static DateTime LocalToUTC(DateTime time)
            {
                return time.ToUniversalTime();
            }

            public static DateTime UTCToLocal(DateTime time)
            {
                return time + TimeZoneInfo.Local.BaseUtcOffset;
            }

            public static DateTime ToDateTime(long ticket)
            {
                return source.AddMilliseconds(ticket);
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
                span = source.AddMilliseconds(ticks) - DateTime.UtcNow;

                return (float)span.TotalSeconds;
            }

            public static string ToString(DateTime time)
            {
                return time.ToString();
            }

            public static string ToString(TimeSpan span)
            {
                builder.Clear();

                if (span.Days > 0)
                {
                    builder.Append(span.Days);

                    builder.Append("d");

                    builder.Append(" ");

                    builder.Append(span.Hours);

                    builder.Append("h");
                }
                else if (span.Hours > 0)
                {
                    builder.Append(span.Hours);

                    builder.Append("h");

                    if (span.Minutes < 10)
                    {
                        builder.Append(Define.ZERO);
                    }
                    builder.Append(span.Minutes);

                    builder.Append("m");

                    if (span.Seconds < 10)
                    {
                        builder.Append(Define.ZERO);
                    }
                    builder.Append(span.Seconds);

                    builder.Append("s");
                }
                else
                {
                    builder.Append(span.Minutes);

                    if (span.Minutes < 10)
                    {
                        builder.Append(Define.ZERO);
                    }
                    builder.Append("m");

                    if (span.Seconds < 10)
                    {
                        builder.Append(Define.ZERO);
                    }
                    builder.Append(span.Seconds);

                    builder.Append("s");
                }
                return builder.ToString();
            }
        }
    }
}