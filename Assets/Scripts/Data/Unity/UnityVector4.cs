using UnityEngine;

namespace Data.Serializable
{
    [System.Serializable]
    public struct UnityVector4
    {
        public float x;

        public float y;

        public float z;

        public float w;

        public static implicit operator UnityVector4(Vector4 vector)
        {
            return new UnityVector4()
            {
                x = vector.x,
                y = vector.y,
                z = vector.z,
                w = vector.w
            };
        }

        public static implicit operator UnityVector4(Quaternion quaternion)
        {
            return new UnityVector4()
            {
                x = quaternion.x,
                y = quaternion.y,
                z = quaternion.z,
                w = quaternion.w
            };
        }
    }
}