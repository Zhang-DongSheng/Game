using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace UnityEngine
{
    public class TimeManager : MonoSingleton<TimeManager>
    {
        private const string KEY = "gameduration";

        private readonly static Dictionary<string, Stopwatch> watch = new Dictionary<string, Stopwatch>();

        private readonly Dictionary<string, TimeTask> handler = new Dictionary<string, TimeTask>();

        private Dictionary<string, TimeTask>.Enumerator eunmer;

        private readonly List<string> cache = new List<string>();

        private TimeTask current;

        private void Awake()
        {
            GameDuration = Local.GetLong(KEY) + (long)Time.time;
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

            GameDuration += (long)Time.deltaTime;
        }

        private void OnDestroy()
        {
            Local.SetLong(KEY, GameDuration);
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
        public long GameDuration { get; private set; }
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
            callBack?.Invoke();
        }
    }
}