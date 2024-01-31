using System;
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

            Register();
        }

        protected virtual void OnDisable()
        {
            Unregister();

            OnVisible(false);
        }
        /// <summary>
        /// ����
        /// </summary>
        protected virtual void OnAwake()
        {

        }
        /// <summary>
        /// ע���¼�
        /// </summary>
        protected virtual void OnRegister()
        {

        }
        /// <summary>
        /// ��ʾ����
        /// </summary>
        /// <param name="active">����</param>
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
        /// <summary>
        /// ���ڴ�
        /// </summary>
        protected virtual void OnLowMemory(float delta)
        {

        }
        /// <summary>
        /// ע���¼�
        /// </summary>
        protected virtual void OnUnregister()
        {

        }
        /// <summary>
        /// �ͷ�
        /// </summary>
        protected virtual void OnRelease()
        {

        }

        protected void Register()
        {
            foreach (var e in Enum.GetValues(typeof(RuntimeEvent)))
            {
                var key = (RuntimeEvent)e;

                if (this.Override(string.Format("On{0}", key)))
                {
                    if (_events.Contains(key))
                    {
                        return;
                    }
                    else
                    {
                        _events.Add(key);
                    }
                    RuntimeManager.Instance.Register(key, Function(key));
                }
            }
        }

        protected void Unregister()
        {
            foreach (var key in _events)
            {
                RuntimeManager.Instance.Unregister(key, Function(key));
            }
            _events.Clear();
        }

        protected Action<float> Function(RuntimeEvent runtime)
        {
            switch (runtime)
            {
                case RuntimeEvent.FixedUpdate:
                    return OnFixedUpdate;
                case RuntimeEvent.Update:
                    return OnUpdate;
                case RuntimeEvent.LateUpdate:
                    return OnLateUpdate;
                case RuntimeEvent.LowMemory:
                    return OnLowMemory;
                default: return null;
            }
        }

        protected void OnDestroy()
        {
            OnUnregister();

            Unregister();

            OnRelease();
        }
    }
}