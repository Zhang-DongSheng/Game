using System;

namespace Game
{
    public abstract class RuntimeBase : IDisposable
    {
        public RuntimeBase()
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

        public void Dispose()
        {
            RuntimeMonoBehaviour.Instance.Unregister(this);
        }
    }
}