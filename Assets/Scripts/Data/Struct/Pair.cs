using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class BoolPair
    {
        public bool x;

        public bool y;
    }
    [System.Serializable]
    public class IntPair
    {
        public int x;

        public int y;

        public static implicit operator UIntPair(IntPair pair)
        {
            return new UIntPair()
            {
                x = (uint)pair.x,
                y = (uint)pair.y,
            };
        }
    }
    [System.Serializable]
    public class UIntPair
    {
        public uint x;

        public uint y;

        public static implicit operator IntPair(UIntPair pair)
        {
            return new IntPair()
            {
                x = (int)pair.x,
                y = (int)pair.y,
            };
        }
    }
    [System.Serializable]
    public class FloatPair
    {
        public float x;

        public float y;
    }
    [System.Serializable]
    public class LongPair
    {
        public long x;

        public long y;
    }
    [System.Serializable]
    public class ColorPair
    {
        public Color x;

        public Color y;
    }
    [System.Serializable]
    public class StringPair
    {
        public string x;

        public string y;
    }
    [System.Serializable]
    public class Pair<T>
    {
        public T x;

        public T y;
    }
    [System.Serializable]
    public class Pair<K, V>
    {
        public K x;

        public V y;
    }
}