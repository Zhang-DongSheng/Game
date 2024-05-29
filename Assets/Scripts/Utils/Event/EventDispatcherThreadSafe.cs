using System;
using System.Collections.Generic;

namespace UnityEngine
{
    /// <summary>
    /// 线程安全事件派发器
    /// </summary>
    /// <remarks>
    /// UnityException: XXX can only be called from the main thread.
    /// 1.产生错误的原因:
    ///     主线程(继承自monobehaviour)的方法 进行了事件注册clientSocket对象 调用了该事件
    /// 2.分析
    ///     unity对于API调用主线程做了限制:“Unity chose to limit API calls to main-thread, to make a simple and solid threading model that everyone can understand and use.”
    ///     clientSocket对象本身是异步的 不能在异步对象中 去调用unity主线程(继承自monobehaviour) 的方法
    /// 3.解决办法
    /// 在主线程的Update中 去判断是否需要调用该事件
    /// </remarks>
    public class EventDispatcherThreadSafe : MonoSingleton<EventDispatcherThreadSafe>
    {
        private readonly Dictionary<string, Action<object>> events = new Dictionary<string, Action<object>>();

        private readonly Stack<EventTrigger> triggers = new Stack<EventTrigger>();

        private void Update()
        {
            while (triggers.Count > 0)
            {
                var trigger = triggers.Pop();

                if (events.TryGetValue(trigger.key, out Action<object> action))
                {
                    action?.Invoke(trigger.args);
                }
            }
        }

        public void Register(string key, Action<object> action)
        {
            if (events.ContainsKey(key))
            {
                if (events[key] != null)
                {
                    Delegate[] dels = events[key].GetInvocationList();
                    foreach (Delegate del in dels)
                    {
                        if (del.Equals(action))
                            return;
                    }
                }
                events[key] += action;
            }
            else
            {
                events[key] = action;
            }
        }

        public void Unregister(string key, Action<object> action)
        {
            if (events.ContainsKey(key))
            {
                events[key] -= action;

                if (events[key] == null)
                {
                    events.Remove(key);
                }
            }
        }

        public void Post(string key, object args = null)
        {
            triggers.Push(new EventTrigger()
            {
                key = key,
                args = args
            });
        }
        /// <summary>
        /// 事件触发
        /// </summary>
        struct EventTrigger
        {
            public string key;

            public object args;
        }
    }
}