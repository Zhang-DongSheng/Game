using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    /// <summary>
    /// ״̬����
    /// </summary>
    [Serializable]
    public struct BoolInterval
    {
        public bool Lerp(float value)
        {
            return value > 0.5f ? true : false;
        }
    }
    /// <summary>
    /// ��������
    /// </summary>
    public struct IntInterval
    {
        public int origination, destination;

        public int Lerp(float value)
        {
            return Convert.ToInt32(Mathf.Lerp(origination, destination, value));
        }

        public int Rndom()
        {
            return Convert.ToInt32(Mathf.Lerp(origination, destination, Random.Range(0, 1f)));
        }
    }
    /// <summary>
    /// ��������
    /// </summary>
    [Serializable]
    public struct FloatInterval
    {
        public float origination, destination;

        public float Lerp(float value)
        {
            return Mathf.Lerp(origination, destination, value);
        }

        public float Rndom()
        {
            return Mathf.Lerp(origination, destination, Random.Range(0, 1f));
        }

        public static FloatInterval Default { get { return new FloatInterval { origination = 0, destination = 1 }; } }
    }
    /// <summary>
    /// 2ά��������
    /// </summary>
    [Serializable]
    public struct Vector2Interval
    {
        public Vector2 origination, destination;

        public Vector2 Lerp(float value)
        {
            return Vector2.Lerp(origination, destination, value);
        }

        public Vector2 Rndom()
        {
            return Lerp(Random.Range(0, 1f));
        }

        public static Vector2Interval One { get { return new Vector2Interval { origination = Vector2.one, destination = Vector2.one }; } }
    }
    /// <summary>
    /// 3ά��������
    /// </summary>
    [Serializable]
    public struct Vector3Interval
    {
        public Vector3 origination, destination;

        public Vector3 Lerp(float value)
        {
            return Vector3.Lerp(origination, destination, value);
        }

        public Vector3 Rndom()
        {
            return Lerp(Random.Range(0, 1f));
        }

        public static Vector3Interval One { get { return new Vector3Interval { origination = Vector3.one, destination = Vector3.one }; } }
    }
    /// <summary>
    /// ��ɫ����
    /// </summary>
    [Serializable]
    public struct ColorInterval
    {
        public Color origination, destination;

        public Color Lerp(float value)
        {
            return Color.Lerp(origination, destination, value);
        }

        public Color Rndom()
        {
            return Color.Lerp(origination, destination, Random.Range(0, 1f));
        }

        public static ColorInterval White { get { return new ColorInterval { origination = Color.white, destination = Color.white }; } }

        public static ColorInterval Balck { get { return new ColorInterval { origination = Color.black, destination = Color.black }; } }
    }
}