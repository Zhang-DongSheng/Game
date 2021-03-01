using System;
using UnityEngine.UI;

namespace UnityEngine.SAM
{
    public static class Config
    {
        public const int ZERO = 0;

        public const int ONE = 1;

        public const int Ninety = 90;

        public const float SPEED = 6f;
    }
    /// <summary>
    /// 状态区间
    /// </summary>
    [Serializable]
    public struct BoolInterval
    {
        public bool origin, destination;

        public bool Lerp(float value)
        {
            return value != 0 ? destination : origin;
        }
    }
    /// <summary>
    /// 浮点区间
    /// </summary>
    [Serializable]
    public struct FloatInterval
    {
        public float origin, destination;

        public float Lerp(float value)
        {
            return Mathf.Lerp(origin, destination, value);
        }

        public float Rndom()
        {
            return Mathf.Lerp(origin, destination, Random.Range(0, 1f));
        }
    }
    /// <summary>
    /// 2维向量区间
    /// </summary>
    [Serializable]
    public struct Vector2Interval
    {
        public Vector2 origin, destination;

        public Vector2 Lerp(float value)
        {
            return Vector2.Lerp(origin, destination, value);
        }

        public Vector2 Rndom()
        {
            return Vector2.Lerp(origin, destination, Random.Range(0, 1f));
        }
    }
    /// <summary>
    /// 向量区间
    /// </summary>
    [Serializable]
    public struct Vector3Interval
    {
        public Vector3 origin, destination;

        public Vector3 Lerp(float value)
        {
            return Vector3.Lerp(origin, destination, value);
        }

        public Vector3 Rndom()
        {
            return Vector3.Lerp(origin, destination, Random.Range(0, 1f));
        }
    }
    /// <summary>
    /// 颜色区间
    /// </summary>
    [Serializable]
    public struct ColorInterval
    {
        public Color origin, destination;

        public Color Lerp(float value)
        {
            return Color.Lerp(origin, destination, value);
        }

        public Color Rndom()
        {
            return Color.Lerp(origin, destination, Random.Range(0, 1f));
        }
    }
    /// <summary>
    /// 状态
    /// </summary>
    public enum Status
    {
        Idel,
        Ready,
        Transition,
        Pause,
        Compute,
        Completed,
    }
    /// <summary>
    /// 方向
    /// </summary>
    public enum Axis
    {
        None,
        Horizontal,
        Vertical,
    }
    /// <summary>
    /// 循环模式
    /// </summary>
    public enum Circle
    {
        Once,
        Loop,
        PingPong,
    }
}