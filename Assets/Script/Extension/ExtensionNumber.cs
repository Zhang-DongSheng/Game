using System;

namespace Game
{
    public static partial class NumberExtension
    {
        private const int GB = 1024 * 1024 * 1024;

        private const int MB = 1024 * 1024;

        private const int KB = 1024;

        private const int DAY = 60 * 60 * 24;

        private const int HOUR = 60 * 60;

        private const int MINUTE = 60;

        private static float value;

        private static string unit;

        public static int Between(this int self, int min, int max)
        {
            if (self < min)
            {
                self = min;
            }
            else if (self > max)
            {
                self = max;
            }
            return self;
        }

        public static float Between(this float self, float min, float max)
        {
            if (self < min)
            {
                self = min;
            }
            else if (self > max)
            {
                self = max;
            }
            return self;
        }

        public static double Between(this long self, long min, long max)
        {
            if (self < min)
            {
                self = min;
            }
            else if (self > max)
            {
                self = max;
            }
            return self;
        }

        public static int Sum(this int self, params int[] numbers)
        {
            for (int i = 0; i < numbers.Length; i++)
            {
                self += numbers[i];
            }
            return self;
        }

        public static float Sum(this float self, params float[] numbers)
        {
            for (int i = 0; i < numbers.Length; i++)
            {
                self += numbers[i];
            }
            return self;
        }

        public static long Sum(this long self, params long[] numbers)
        {
            for (int i = 0; i < numbers.Length; i++)
            {
                self += numbers[i];
            }
            return self;
        }

        public static int Difference(this int self, int target)
        {
            return Math.Abs(self - target);
        }

        public static float Difference(this float self, float target)
        {
            return Math.Abs(self - target);
        }

        public static long Difference(this long self, long target)
        {
            return Math.Abs(self - target);
        }

        public static bool MoreThan(this int self, int target)
        {
            return self >= target;
        }

        public static bool MoreThan(this float self, float target)
        {
            return self >= target;
        }

        public static bool MoreThan(this long self, long target)
        {
            return self >= target;
        }

        public static bool LessThan(this int self, int target)
        {
            return self <= target;
        }

        public static bool LessThan(this float self, float target)
        {
            return self <= target;
        }

        public static bool LessThan(this long self, long target)
        {
            return self <= target;
        }

        public static string ToNumber(this int self, int digit)
        {
            if (self < 1000)
            {
                value = self;
            }
            else if (self < 1000000)
            {
                value = self / 1000; unit = "k+";
            }
            else
            {
                value = self / 1000000; unit = "m+";
            }
            return string.Format("{0}{1}", Math.Round(value, digit), unit);
        }

        public static string ToSize(this long self)
        {
            if (self >= GB)
            {
                value = self / GB; unit = "G";
            }
            else if (self >= MB)
            {
                value = self / MB; unit = "M";
            }
            else if (self >= KB)
            {
                value = self / KB; unit = "K";
            }
            else
            {
                value = self; unit = "B";
            }
            return string.Format("{0}{1}", value, unit);
        }

        public static string ToTime(this int second)
        {
            if (second >= DAY)
            {
                value = second / DAY; unit = "Day";
            }
            else if (second >= HOUR)
            {
                value = second / HOUR; unit = "h";
            }
            else if (second >= MINUTE)
            {
                value = second / MINUTE; unit = "m";
            }
            else
            {
                value = second; unit = "s";
            }
            return string.Format("{0}{1}", value, unit);
        }
    }
}