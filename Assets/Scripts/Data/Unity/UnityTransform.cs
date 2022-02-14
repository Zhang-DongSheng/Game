using UnityEngine;

namespace Data.Serializable
{
    [System.Serializable]
    public struct UnityTransform
    {
        public UnityVector3 position;

        public UnityVector4 rotation;

        public UnityVector3 scale;

        public static implicit operator UnityTransform(Transform transform)
        {
            return new UnityTransform()
            {
                position = transform.position,
                rotation = transform.rotation,
                scale = transform.localScale
            };
        }
    }
}