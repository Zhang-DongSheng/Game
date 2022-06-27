using System;

namespace Game
{
    public static partial class Utility
    {
        public static class _Random
        {
            private static readonly Random random = new Random();

            public static int Range(int min, int max)
            {
                return random.Next(min, max);
            }

            public static float Range(float min, float max)
            {
                return min + (float)random.NextDouble() * (max - min);
            }
        }
    }
}