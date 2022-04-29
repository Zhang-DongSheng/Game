using UnityEngine;

namespace Game
{
    public abstract class RuntimeBase : MonoBehaviour
    {
        protected virtual void Awake()
        {
            RuntimeMonoBehaviour.Instance.Register(this);
        }

        internal virtual void OnUpdate(float delta)
        {

        }

        internal virtual void OnFixUpdate(float delta)
        {

        }

        internal virtual void OnLaterUpdate()
        {

        }

        protected virtual void OnDestroy()
        {
            RuntimeMonoBehaviour.Instance.Unregister(this);
        }
    }
}