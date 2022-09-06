using System;

namespace Game
{
    public static partial class Utility
    {
        /// <summary>
        /// Ëæ»úÊý
        /// </summary>
        public static class Random
        {
            private static readonly System.Random random = new System.Random();

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