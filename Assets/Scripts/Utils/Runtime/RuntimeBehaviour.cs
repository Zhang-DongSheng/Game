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
        /// 唤醒
        /// </summary>
        protected virtual void OnAwake()
        {

        }
        /// <summary>
        /// 注册事件
        /// </summary>
        protected virtual void OnRegister()
        {

        }
        /// <summary>
        /// 显示隐藏
        /// </summary>
        /// <param name="active">显隐</param>
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
        /// 低内存
        /// </summary>
        protected virtual void OnLowMemory(float delta)
        {

        }
        /// <summary>
        /// 注销事件
        /// </summary>
        protected virtual void OnUnregister()
        {

        }
        /// <summary>
        /// 释放
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