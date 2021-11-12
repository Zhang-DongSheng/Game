using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace UnityEngine
{
    public sealed class TimeManager : MonoSingleton<TimeManager>
    {
        private const string KEY = "game_total_time";

        private readonly Dictionary<string, Stopwatch> watch = new Dictionary<string, Stopwatch>();

        private readonly Dictionary<string, TimeHandler> handlers = new Dictionary<string, TimeHandler>();

        private readonly List<string> cache = new List<string>();

        private Dictionary<string, TimeHandler>.Enumerator eunmer;

        private TimeHandler handler;

        private int count;

        private void Awake()
        {
            Duration = Local.GetValue<long>(KEY) + (long)Time.time;
        }

        private void Update()
        {
            #region Handler
            eunmer = handlers.GetEnumerator();

            while (eunmer.MoveNext())
            {
                handler = eunmer.Current.Value;

                if (Time.time >= handler.timer)
                {
                    handler.Invoke();

                    if (handler.loop)
                    {
                        handler.timer = Time.time + handler.interval;
                    }
                    else
                    {
                        cache.Add(eunmer.Current.Key);
                    }
                }
            }

            count = cache.Count;

            if (count > 0)
            {
                for (int i = count - 1; i > -1; i--)
                {
                    if (handlers.ContainsKey(cache[i]))
                    {
                        handlers.Remove(cache[i]);
                    }
                    cache.RemoveAt(i);
                }
            }
            #endregion

            #region Duration
            Duration += (long)Time.deltaTime;
            #endregion
        }

        private void OnDestroy()
        {
            Local.SetValue(KEY, Duration);
        }

        public void Register(string key, TimeHandler value)
        {
            if (handlers.ContainsKey(key))
            {
                handlers[key].Register(value.action);
            }
            else
            {
                handlers.Add(key, value);
            }
        }

        public void Unregister(string key)
        {
            if (handlers.ContainsKey(key))
            {
                cache.Add(key);
            }
        }

        public void TimBegin(string key)
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

        public long TimEnd(string key)
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

    public class TimeHandler
    {
        public float interval;

        public float timer;

        public bool loop;

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