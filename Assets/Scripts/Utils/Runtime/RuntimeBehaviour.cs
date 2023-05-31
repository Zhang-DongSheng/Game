using UnityEngine;

namespace Game
{
    public abstract class RuntimeBehaviour : MonoBehaviour
    {
        protected void Awake()
        {
            OnAwake();

            Register(RuntimeEvent.FixedUpdate, OnFixedUpdate);

            Register(RuntimeEvent.Update, OnUpdate);

            Register(RuntimeEvent.LateUpdate, OnLateUpdate);
        }

        protected void OnDestroy()
        {
            OnRelease();

            Unregister(RuntimeEvent.FixedUpdate, OnFixedUpdate);

            Unregister(RuntimeEvent.Update, OnUpdate);

            Unregister(RuntimeEvent.LateUpdate, OnLateUpdate);
        }

        protected virtual void OnAwake()
        {

        }

        protected virtual void OnUpdate(float delta)
        {

        }

        protected virtual void OnFixedUpdate(float delta)
        {

        }

        protected virtual void OnLateUpdate(float delta)
        {

        }

        protected virtual void OnRelease()
        {

        }

        protected void Register(RuntimeEvent key, FunctionBySingle function)
        {
            if (this.Override(function.Method.Name))
            {
                RuntimeManager.Instance.Register(key, function);
            }
        }

        protected void Unregister(RuntimeEvent key, FunctionBySingle function)
        {
            if (this.Override(function.Method.Name))
            {
                RuntimeManager.Instance.Unregister(key, function);
            }
        }
    }
}