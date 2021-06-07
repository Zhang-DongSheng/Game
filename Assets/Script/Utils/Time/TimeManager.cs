using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace UnityEngine
{
    public sealed class TimeManager : MonoSingleton<TimeManager>
    {
        private const string KEY = "_duration";

        private readonly static Dictionary<string, Stopwatch> watch = new Dictionary<string, Stopwatch>();

        private readonly Dictionary<string, TimeEvent> handler = new Dictionary<string, TimeEvent>();

        private readonly List<string> cache = new List<string>();

        private Dictionary<string, TimeEvent>.Enumerator eunmer;

        private TimeEvent current;

        private void Awake()
        {
            Duration = Local.GetLong(KEY) + (long)Time.time;
        }

        private void Update()
        {
            eunmer = handler.GetEnumerator();

            while (eunmer.MoveNext())
            {
                current = eunmer.Current.Value;

                if (Time.time >= current.timer)
                {
                    current.Invoke();

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
            Duration += (long)Time.deltaTime;
        }

        private void OnDestroy()
        {
            Local.SetLong(KEY, Duration);
        }

        public void Register(string key, TimeEvent value)
        {
            if (handler.ContainsKey(key))
            {
                handler[key].Register(value.action);
            }
            else
            {
                handler.Add(key, value);
            }
        }

        public void Unregister(string key)
        {
            if (handler.ContainsKey(key))
            {
                cache.Add(key);
            }
        }

        public static void TimBegin(string key)
        {
            if (watch.ContainsKey(key))
            {
                watch[key].Start();
            }
            else
            {
                watch.Add(key, Stopwatch.StartNew());
            }
        }

        public static float TimEnd(string key)
        {
            long time = 0;

            if (watch.ContainsKey(key))
            {
                time = watch[key].ElapsedTicks;

                watch[key].Stop();
            }
            return time;
        }
        /// <summary>
        /// 游戏总时长，单位(s)
        /// </summary>
        public long Duration { get; private set; }
    }

    public class TimeEvent
    {
        public bool loop;

        public float timer;

        public float interval;

        public Action action;

        public void Register(Action action)
        {
            if (action != null)
            {
                Delegate[] dels = this.action.GetInvocationList();
                foreach (Delegate del in dels)
                {
                    if (del.Equals(action))
                        return;
                }
            }
            this.action += action;
        }

        public void Invoke()
        {
            action?.Invoke();
        }
    }
}