namespace Game
{
    public static partial class Extension
    {
        private const int Kilobyte = 1024;

        private const float LIGHT = 0.0625f;

        private const float ANGLE45 = 45f;

        private const float ANGLE135 = 135f;

        private const float HALF = 0.5f;

        private static readonly string[] UnitQuantity = new string[] { "Hundred", "Thousand", "Million" };

        private static readonly string[] UnitByte = new string[] { "Byte", "KB", "MB", "GB", "TB", "PB" };

        private static readonly string[] UnitTime = new string[] { "Second", "Minute", "Hour", "Day", "Month", "Year" };

        private static string unit;

        private static float value;

        private static double length;
    }

    public delegate bool Match<T1, T2>(T1 arg1, T2 arg2);
}