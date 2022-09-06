using UnityEngine;

namespace Game
{
    public static partial class Extension
    {
        /// <summary>
        /// 插值
        /// </summary>
        public static class Interpolation
        {
            public static float Linear(float t)
            {
                return t;
            }

            public static float SmoothStep(float t)
            {
                return t * t * (3.0f - 2.0f * t);
            }

            public static float SmootherStep(float t)
            {
                return t * t * t * (t * (6.0f * t - 15.0f) + 10.0f);
            }

            public static float SineIn(float t)
            {
                return Mathf.Sin((t - 1.0f) * Mathf.PI * 0.5f) + 1.0f;
            }

            public static float SineOut(float t)
            {
                return Mathf.Sin(t * Mathf.PI * 0.5f);
            }

            public static float SineInOut(float t)
            {
                return (1.0f - Mathf.Cos(t * Mathf.PI)) * 0.5f;
            }

            public static float QuadraticIn(float t)
            {
                return t * t;
            }

            public static float QuadraticOut(float t)
            {
                return -(t * (t - 2.0f));
            }

            public static float QuadraticInOut(float t)
            {
                return (t < 0.5f) ? 2.0f * (t * t) : (-2.0f * t * t) + (4 * t) - 1;
            }

            public static float CubicIn(float t)
            {
                return Mathf.Pow(t, 3);
            }

            public static float CubicOut(float t)
            {
                return 1.0f - Mathf.Pow(1.0f - t, 3);
            }

            public static float CubicInOut(float t)
            {
                return (t < 0.5f) ? Mathf.Pow(t * 2.0f, 3) * 0.5f : 1.0f - Mathf.Pow((1.0f - t) * 2.0f, 3) * 0.5f;
            }

            public static float QuarticIn(float t)
            {
                return Mathf.Pow(t, 4);
            }

            public static float QuarticOut(float t)
            {
                return 1.0f - Mathf.Pow(1.0f - t, 4);
            }

            public static float QuarticInOut(float t)
            {
                return (t < 0.5f) ? Mathf.Pow(t * 2.0f, 4) * 0.5f : 1.0f - Mathf.Pow((1.0f - t) * 2.0f, 4) * 0.5f;
            }

            public static float QuinticIn(float t)
            {
                return Mathf.Pow(t, 5);
            }

            public static float QuinticOut(float t)
            {
                return 1.0f - Mathf.Pow(1.0f - t, 5);
            }

            public static float QuinticInOut(float t)
            {
                return (t < 0.5f) ? Mathf.Pow(t * 2.0f, 5) * 0.5f : 1.0f - Mathf.Pow((1.0f - t) * 2.0f, 5) * 0.5f;
            }

            public static float CircularIn(float t)
            {
                return 1.0f - Mathf.Sqrt(1.0f - (t * t));
            }

            public static float CircularOut(float t)
            {
                return Mathf.Sqrt((2.0f - t) * t);
            }

            public static float CircularInOut(float t)
            {
                return (t < 0.5f) ? (1.0f - Mathf.Sqrt(1.0f - 4.0f * (t * t))) * 0.5f : (Mathf.Sqrt(-((t * 2.0f) - 3.0f) * ((t * 2.0f) - 1.0f)) + 1.0f) * 0.5f;
            }

            public static float ExpoIn(float t)
            {
                return (t == 0.0f) ? 0.0f : Mathf.Pow(2, 10.0f * (t - 1.0f));
            }

            public static float ExpoOut(float t)
            {
                return (t == 1.0f) ? 1.0f : 1.0f - Mathf.Pow(2, -10.0f * t);
            }

            public static float ExpoInOut(float t)
            {
                if (t == 0.0f || t == 1.0f) return t;

                t *= 2.0f;

                if (t < 1.0f)
                {
                    return Mathf.Pow(1024.0f, t - 1.0f) * 0.5f;
                }

                return (-Mathf.Pow(2.0f, -10.0f * (t - 1.0f)) + 2.0f) * 0.5f;
            }

            public static float BackIn(float t)
            {
                float s = 1.70158f;

                return t * t * ((s + 1.0f) * t - s);
            }

            public static float BackOut(float t)
            {
                float s = 1.70158f;

                t--;

                return t * t * ((s + 1.0f) * t + s) + 1.0f;
            }

            public static float BackInOut(float t)
            {
                float s = 1.70158f * 1.525f;

                t *= 2.0f;

                if (t < 1.0f)
                {
                    return (t * t * ((s + 1.0f) * t - s)) * 0.5f;
                }

                t -= 2.0f;

                return (t * t * ((s + 1.0f) * t + s) + 2.0f) * 0.5f;
            }

            public static float BounceIn(float t)
            {
                return 1.0f - BounceOut(1.0f - t);
            }

            public static float BounceOut(float t)
            {
                if (t < (1.0f / 2.75f))
                    return 7.5625f * t * t;
                else if (t < (2.0f / 2.75f))
                    return 7.5625f * (t -= (1.5f / 2.75f)) * t + 0.75f;
                else if (t < (2.5f / 2.75f))
                    return 7.5625f * (t -= (2.25f / 2.75f)) * t + 0.9375f;
                return 7.5625f * (t -= (2.625f / 2.75f)) * t + 0.984375f;
            }

            public static float BounceInOut(float t)
            {
                if (t < 0.5f) return BounceIn(t * 2.0f) * 0.5f;

                return BounceOut(t * 2.0f - 1.0f) * 0.5f + 0.5f;
            }

            public static float ElasticIn(float t)
            {
                if (t == 0.0f || t == 1.0f) return t;

                return -Mathf.Pow(2.0f, 10.0f * (t - 1.0f)) * Mathf.Sin((t - 1.1f) * 5.0f * Mathf.PI);
            }

            public static float ElasticOut(float t)
            {
                if (t == 0.0f || t == 1.0f) return t;

                return Mathf.Pow(2.0f, -10.0f * t) * Mathf.Sin((t - 0.1f) * 5.0f * Mathf.PI) + 1.0f;
            }

            public static float ElasticInOut(float t)
            {
                if (t == 0.0f || t == 1.0f) return t;

                t *= 2.0f;

                if (t < 1.0f)
                {
                    return Mathf.Pow(2, 10.0f * (t - 1)) * Mathf.Sin((t - 1.1f) * 5.0f * Mathf.PI) * -0.5f;
                }
                return (Mathf.Pow(2.0f, -10.0f * (t - 1)) * Mathf.Sin((t - 1.1f) * 5.0f * Mathf.PI) * 0.5f) + 1.0f;
            }

            public static float CatmullRom(float t, float p0, float p1, float p2, float p3)
            {
                return 0.5f * ((2.0f * p1) + (-p0 + p2) * t + (2.0f * p0 - 5.0f * p1 + 4.0f * p2 - p3) * t * t + (-p0 + 3.0f * p1 - 3.0f * p2 + p3) * t * t * t);
            }
        }
    }
}