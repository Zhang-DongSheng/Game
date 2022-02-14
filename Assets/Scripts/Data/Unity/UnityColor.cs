using UnityEngine;

namespace Data.Serializable
{
    [System.Serializable]
    public struct UnityColor
    {
        public float r;

        public float g;

        public float b;

        public float a;

        public static implicit operator UnityColor(Color color)
        {
            return new UnityColor()
            {
                r = color.r,
                g = color.g,
                b = color.b,
                a = color.a
            };
        }

        public static implicit operator UnityColor(Color32 color)
        {
            return new UnityColor()
            {
                r = color.r,
                g = color.g,
                b = color.b,
                a = color.a
            };
        }
    }
}