using System;

namespace UnityEngine
{
    [RequireComponent(typeof(Animation))]
    public class AnimationListener : MonoBehaviour
    {
        public const string FUNCTION = "OnTriggerAnimation";

        private Action action;

        public void AddListener(Action action)
        {
            this.action = action;
        }

        private void OnTriggerAnimation()
        {
            action?.Invoke();
        }
    }
}