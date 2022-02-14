using UnityEngine;

namespace Data.Serializable
{
    [System.Serializable]
    public struct UnityVector2
    {
        public float x;

        public float y;

        public static implicit operator UnityVector2(Vector2 vector)
        {
            return new UnityVector2()
            {
                x = vector.x,
                y = vector.y
            };
        }
    }
}