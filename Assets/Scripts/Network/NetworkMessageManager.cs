using Game.Network;
using Google.Protobuf;
using System;
using System.Collections.Generic;

namespace Game
{
    public class NetworkMessageManager : MonoSingleton<NetworkMessageManager>
    {
        private readonly Dictionary<int, Action<IMessage>> events = new Dictionary<int, Action<IMessage>>();

        private readonly Stack<RawMessage> caches = new Stack<RawMessage>();

        private void Update()
        {
            while (caches.Count > 0)
            {
                var raw = caches.Pop();

                if (events.TryGetValue(raw.key, out Action<IMessage> action))
                {
                    action?.Invoke(raw.message);

                    if (raw.key < 10000)
                    {
                        events.Remove(raw.key);
                    }
                }
            }
        }

        public void Register(int key, Action<IMessage> action)
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

        public void Unregister(int key, Action<IMessage> action)
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

        public void PostEvent(RawMessage message)
        {
            /* UnityException: XXX can only be called from the main thread.
             * 1.产生错误的原因:
             *   主线程(继承自monobehaviour)的方法 进行了事件注册clientSocket对象 调用了该事件
             * 2.分析
             *  unity对于API调用主线程做了限制:“Unity chose to limit API calls to main-thread, to make a simple and solid threading model that everyone can understand and use.”
             *  clientSocket对象本身是异步的 不能在异步对象中 去调用unity主线程(继承自monobehaviour)的方法
             * 3.解决办法
             * 在主线程的Update中 去判断是否需要调用该事件
             * */
            caches.Push(message);
        }
    }
}