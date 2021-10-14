using UnityEngine;

namespace Game
{
    public abstract class BlendNode : MonoBehaviour
    {
        public float value;

        public abstract BlendData Blend();
    }
}