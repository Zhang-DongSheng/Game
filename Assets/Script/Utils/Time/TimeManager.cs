using System;
using System.Collections.Generic;

namespace UnityEngine
{
    public class TimeManager : MonoSingleton<TimeManager>
    {
        private readonly Dictionary<string, DateTime> timing = new Dictionary<string, DateTime>();

        private readonly Dictionary<string, TimeTask> handler = new Dictionary<string, TimeTask>();

        private Dictionary<string, TimeTask>.Enumerator eunmer;

        private readonly List<string> cache = new List<string>();

        private TimeTask current;

        private void Awake()
        {
            GameLength = Local.GetLong("gamelength");
        }

        private void Update()
        {
            eunmer = handler.GetEnumerator();

            while (eunmer.MoveNext())
            {
                current = eunmer.Current.Value;

                if (Time.time >= current.timer)
                {
                    current.callBack?.Invoke();

                    if (current.loop)
                    {
                        current.timer = Time.time + current.interval;
                    }
                    else
                    {
                        cache.Add(eunmer.Current.Key);
                    }
                }
            }

            if (cache.Count > 0)
            {
                for (int i = cache.Count - 1; i >= 0; i--)
                {
                    if (handler.ContainsKey(cache[i]))
                    {
                        handler.Remove(cache[i]);
                    }
                    cache.RemoveAt(i);
                }
            }

            GameLength += (long)Time.deltaTime;
        }

        public void Register(string key, TimeTask task)
        {
            if (handler.ContainsKey(key))
            {
                handler[key].Register(task.callBack);
            }
            else
            {
                handler.Add(key, task);
            }
        }

        public void Unregister(string key)
        {
            if (handler.ContainsKey(key))
            {
                cache.Add(key);
            }
        }

        private void OnDestroy()
        {
            Local.SetLong("gamelength", GameLength);
        }

        public void TimBegin(string key)
        {
            if (timing.ContainsKey(key))
            {
                timing[key] = DateTime.Now;
            }
            else
            {
                timing.Add(key, DateTime.Now);
            }
        }

        public double TimEnd(string key)
        {
            if (timing.ContainsKey(key))
            {
                return DateTime.Now.Subtract(timing[key]).TotalMilliseconds;
            }
            else
            {
                return 0;
            }
        }

        public long GameLength { get; private set; }
    }

    public class TimeTask
    {
        public string name;

        public bool loop;

        public float timer;

        public float interval;

        public Action callBack;

        public void Register(Action action)
        {
            if (callBack != null)
            {
                Delegate[] dels = callBack.GetInvocationList();
                foreach (Delegate del in dels)
                {
                    if (del.Equals(action))
                        return;
                }
            }
            callBack += action;
        }

        public void Invoke()
        { 
            
        }
    }
}