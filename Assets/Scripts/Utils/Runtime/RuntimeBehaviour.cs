using UnityEngine;

namespace Game
{
    public abstract class RuntimeBehaviour : MonoBehaviour
    {
        protected readonly RuntimeParameter parameter = new RuntimeParameter();

        protected virtual void OnEnable()
        {
            Register(RuntimeEvent.FixedUpdate, OnFixedUpdate);

            Register(RuntimeEvent.Update, OnUpdate);

            Register(RuntimeEvent.LateUpdate, OnLateUpdate);
        }

        protected virtual void OnDisable()
        {
            Unregister(RuntimeEvent.FixedUpdate, OnFixedUpdate);

            Unregister(RuntimeEvent.Update, OnUpdate);

            Unregister(RuntimeEvent.LateUpdate, OnLateUpdate);
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

        protected void Register(RuntimeEvent key, FunctionBySingle function)
        {
            if (this.Override(function.Method.Name))
            {
                parameter.Register(key);

                RuntimeManager.Instance.Register(key, function);
            }
        }

        protected void Unregister(RuntimeEvent key, FunctionBySingle function)
        {
            if (parameter.Exists(key) == false) return;

            parameter.Unregister(key);

            if (this.Override(function.Method.Name))
            {
                RuntimeManager.Instance.Unregister(key, function);
            }
        }
    }
}