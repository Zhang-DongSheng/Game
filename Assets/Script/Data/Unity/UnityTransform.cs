using UnityEngine;

namespace Data.Unity
{
    public struct UnityTransform
    {
        public float[] position;

        public float[] rotation;

        public float[] scale;

        public static implicit operator UnityTransform(Transform transform)
        {
            return new UnityTransform()
            {
                position = new float[3]
                {
                    transform.position.x,
                    transform.position.y,
                    transform.position.z
                },
                rotation = new float[3]
                {
                    transform.eulerAngles.x,
                    transform.eulerAngles.y,
                    transform.eulerAngles.z
                },
                scale = new float[3]
                {
                    transform.localScale.x,
                    transform.localScale.y,
                    transform.localScale.z
                },
            };
        }
    }
}