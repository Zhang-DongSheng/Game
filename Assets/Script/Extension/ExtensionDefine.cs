namespace Game
{
    public static partial class Extension
    {
        private const int GB = 1024 * 1024 * 1024;

        private const int MB = 1024 * 1024;

        private const int KB = 1024;

        private const float LIGHT = 0.0625f;

        private const float ANGLE45 = 45f;

        private const float ANGLE135 = 135f;

        private const float HALF = 0.5f;

        private static string unit;

        private static float value;

        private static double length;
    }

    public delegate bool Match<T1, T2>(T1 arg1, T2 arg2);
}