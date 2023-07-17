namespace Game
{
    public static partial class Extension
    {
        private const int Kilobyte = 1024;

        private const float Light = 0.0625f;

        private const float Angle45 = 45f;

        private const float Angle135 = 135f;

        private const float Half = 0.5f;

        private const string Slash = "/";

        private static readonly string[] UnitQuantity = new string[] { "Hundred", "Thousand", "Million" };

        private static readonly string[] UnitByte = new string[] { "Byte", "KB", "MB", "GB", "TB", "PB" };

        private static readonly string[] UnitTime = new string[] { "Second", "Minute", "Hour", "Day", "Month", "Year" };

        private static string unit;

        private static float value;

        private static double length;

        public enum Axis
        {
            X,
            Y,
            Z,
        }

        public enum VisibleType
        {
            Active,
            Alpha,
            Scale,
        }
    }

    public delegate bool Match<X, Y>(X x, Y y);
}