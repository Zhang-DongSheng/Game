using UnityEngine;

namespace Data.Serializable
{
    [System.Serializable]
    public struct UnityVector3
    {
        public float x;

        public float y;

        public float z;

        public static implicit operator UnityVector3(Vector3 vector)
        {
            return new UnityVector3()
            {
                x = vector.x,
                y = vector.y,
                z = vector.z
            };
        }
    }
}