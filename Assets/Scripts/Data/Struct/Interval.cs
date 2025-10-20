using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public abstract class Interval<T>
    {
        [Range(0, 1)] public float value;

        public T start;

        public T end;

        public T result;

        public virtual T Lerp(float value)
        {
            return result;
        }

        public virtual T Rndom()
        {
            return Lerp(Random.Range(0, 1));
        }
    }
    /// <summary>
    /// 状态区间
    /// </summary>
    [Serializable]
    public class BoolInterval : Interval<bool>
    {
        public override bool Lerp(float value)
        {
            this.value = value;

            result = value > 0.5f ? start : end;

            return result;
        }
    }
    /// <summary>
    /// 整型区间
    /// </summary>
    [Serializable]
    public class IntInterval : Interval<int>
    {
        public override int Lerp(float value)
        {
            this.value = value;

            result = Convert.ToInt32(Mathf.Lerp(start, end, value));

            return result;
        }
    }
    /// <summary>
    /// 浮点区间
    /// </summary>
    [Serializable]
    public class FloatInterval : Interval<float>
    {
        public override float Lerp(float value)
        {
            this.value = value;

            result = Mathf.Lerp(start, end, value);

            return result;
        }
    }
    /// <summary>
    /// 2维向量区间
    /// </summary>
    [Serializable]
    public class Vector2Interval : Interval<Vector2>
    {
        public override Vector2 Lerp(float value)
        {
            this.value = value;

            result = Vector2.Lerp(start, end, value);

            return result;
        }
    }
    /// <summary>
    /// 3维向量区间
    /// </summary>
    [Serializable]
    public class Vector3Interval : Interval<Vector3>
    {
        public override Vector3 Lerp(float value)
        {
            this.value = value;

            result = Vector3.Lerp(start, end, value);

            return result;
        }

        public static Vector3Interval Default
        {
            get
            {
                return new Vector3Interval()
                {
                    start = Vector3.zero,
                    end = Vector3.one,
                };
            }
        }
    }
    /// <summary>
    /// 颜色区间
    /// </summary>
    [Serializable]
    public class ColorInterval : Interval<Color>
    {
        public override Color Lerp(float value)
        {
            this.value = value;

            result = Color.Lerp(start, end, value);

            return result;
        }

        public static ColorInterval Default
        {
            get
            {
                return new ColorInterval()
                {
                    start = Color.white,
                    end = Color.white,
                };
            }
        }
    }
}