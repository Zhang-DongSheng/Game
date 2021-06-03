using System;
using System.Collections.Generic;

namespace Game.Network
{
    public static class NetworkEventManager
    {
        private static readonly Dictionary<NetworkEventKey, Action<NetworkEventHandle>> events = new Dictionary<NetworkEventKey, Action<NetworkEventHandle>>();

        public static void Register(NetworkEventKey key, Action<NetworkEventHandle> action)
        {
            if (!events.ContainsKey(key))
            {
                events[key] = action;
            }
            else
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
        }

        public static void Unregister(NetworkEventKey key, Action<NetworkEventHandle> action)
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

        public static void PostEvent(NetworkEventKey key, NetworkEventHandle handle = null)
        {
            if (events.ContainsKey(key))
            {
                events[key](handle);
            }
            handle?.Dispose();
        }
    }

    public class NetworkEventHandle : IDisposable
    {
        public void Dispose()
        {

        }
    }
}