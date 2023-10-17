using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class RuntimeBehaviour : MonoBehaviour
    {
        private readonly List<RuntimeEvent> _events = new List<RuntimeEvent>();

        protected void Awake()
        {
            OnAwake();

            OnRegister();
        }

        protected virtual void OnEnable()
        {
            OnVisible(true);

            Register(RuntimeEvent.FixedUpdate, OnFixedUpdate);

            Register(RuntimeEvent.Update, OnUpdate);

            Register(RuntimeEvent.LateUpdate, OnLateUpdate);

            Register(RuntimeEvent.LowMemory, OnLowMemory);
        }

        protected virtual void OnDisable()
        {
            Unregister();

            OnVisible(false);
        }

        protected virtual void OnAwake()
        {

        }

        protected virtual void OnRegister()
        {
            
        }

        protected virtual void OnVisible(bool active)
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

        protected virtual void OnLowMemory(float delta)
        {

        }

        protected virtual void OnUnregister()
        {
            
        }

        protected virtual void OnRelease()
        {
            
        }

        protected void Register(RuntimeEvent key, FunctionBySingle function)
        {
            if (this.Override(function.Method.Name))
            {
                if (_events.Contains(key))
                {
                    return;
                }
                else
                {
                    _events.Add(key);
                }
                RuntimeManager.Instance.Register(key, function);
            }
        }

        protected void Unregister()
        {
            foreach(var key in _events)
            {
                switch (key)
                { 
                    case RuntimeEvent.FixedUpdate:
                        RuntimeManager.Instance.Unregister(key, OnFixedUpdate);
                        break;
                    case RuntimeEvent.Update:
                        RuntimeManager.Instance.Unregister(key, OnUpdate);
                        break;
                    case RuntimeEvent.LateUpdate:
                        RuntimeManager.Instance.Unregister(key, OnLateUpdate);
                        break;
                    case RuntimeEvent.LowMemory:
                        RuntimeManager.Instance.Unregister(key, OnLowMemory);
                        break;
                }
            }
            _events.Clear();
        }

        protected void OnDestroy()
        {
            OnUnregister();

            Unregister();

            OnRelease();
        }
    }
}