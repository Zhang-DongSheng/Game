using Game.Attribute;
using Game.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Game
{
    public sealed class TimeManager : MonoSingleton<TimeManager>
    {
        private readonly Dictionary<string, Stopwatch> watch = new Dictionary<string, Stopwatch>();

        private readonly List<TimeHandler> handlers = new List<TimeHandler>();

        [Readonly] public long recent, time;

        [Readonly] public int count;

        private bool modify;

        private void Awake()
        {
            Duration = GlobalVariables.Get<long>(Const.TOTAL_TIME) + (long)Time.time;
        }

        private void Update()
        {
            time = (long)Time.time;

            if (count == 0) return;

            if (time >= recent)
            {
                modify = false;

                count = handlers.Count;

                for (int i = count - 1; i > -1; i--)
                {
                    if (time >= handlers[i].time)
                    {
                        handlers[i].Invoke();

                        if (handlers[i].loop )
                        {
                            handlers[i].time += handlers[i].interval;

                            modify = true;
                        }
                        else
                        {
                            handlers.RemoveAt(i);
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                if (modify)
                {
                    Sort();
                }
                else
                {
                    count = handlers.Count;

                    recent = count > 0 ? handlers[count - 1].time : 0;
                }
            }
            Duration += (long)Time.deltaTime;
        }

        private void Sort()
        {
            void Swap<T>(IList<T> list, int x, int y)
            {
                T temp = list[x];

                list[x] = list[y];

                list[y] = temp;
            }
            count = handlers.Count;

            int gap = count / 2;

            while (gap >= 1)
            {
                for (int i = gap; i < count; i++)
                {
                    for (int j = i; j >= gap &&
                        TimeHandler.Compare(handlers[j], handlers[j - gap]) < 0;
                        j -= gap)
                    {
                        Swap(handlers, j, j - gap);
                    }
                }
                gap /= 2;
            }
            recent = count > 0 ? handlers[count - 1].time : 0;
        }

        private void OnDestroy()
        {
            OnApplicationQuit();
        }

        private void OnApplicationQuit()
        {
            handlers.Clear();

            GlobalVariables.Set(Const.TOTAL_TIME, Duration);
        }

        public void Register(string key, long time, Action value)
        {
            Register(key, time, 0, value);
        }

        public void Register(string key, long time, long interval, Action value)
        {
            int index = handlers.FindIndex(handler => handler.key == key);

            if (index > -1)
            {
                Debuger.LogWarning(Author.Script, "Exist the same key, " + key);

                handlers[index].Register(value);
            }
            else
            {
                handlers.Add(new TimeHandler()
                {
                    key = key,
                    time = time,
                    interval = interval,
                    loop = interval > 0,
                    action = value,
                });
            }
            Sort();
        }

        public void Unregister(string key)
        {
            int index = handlers.FindIndex(handler => handler.key == key);

            if (index > -1)
            {
                handlers.RemoveAt(index);
            }
            Sort();
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
        public string key;

        public long time;

        public bool loop;

        public long interval;

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

        public static int Compare(TimeHandler x, TimeHandler y)
        {
            if (x.time != y.time)
            {
                return x.time > y.time ? -1 : 1;
            }
            return 0;
        }
    }
}