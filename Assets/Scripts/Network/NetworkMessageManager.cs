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
             * 1.���������ԭ��:
             *   ���߳�(�̳���monobehaviour)�ķ��� �������¼�ע��clientSocket���� �����˸��¼�
             * 2.����
             *  unity����API�������߳���������:��Unity chose to limit API calls to main-thread, to make a simple and solid threading model that everyone can understand and use.��
             *  clientSocket���������첽�� �������첽������ ȥ����unity���߳�(�̳���monobehaviour)�ķ���
             * 3.����취
             * �����̵߳�Update�� ȥ�ж��Ƿ���Ҫ���ø��¼�
             * */
            caches.Push(message);
        }
    }
}