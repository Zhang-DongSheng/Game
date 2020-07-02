using System;
using System.Collections.Generic;
using System.Text;

namespace UnityEngine
{
    public class TimeManager : MonoBehaviour
    {
        private static readonly Dictionary<string, TimeTask> Loop_Task = new Dictionary<string, TimeTask>();
        private static readonly List<string> loop_cache = new List<string>();

        private static readonly Dictionary<string, TimeTask> Delay_Task = new Dictionary<string, TimeTask>();
        private static readonly List<string> delay_cache = new List<string>();

        private Dictionary<string, TimeTask>.Enumerator loop_eunmer;
        private Dictionary<string, TimeTask>.Enumerator delay_eunmer;

        private TimeTask current_LT;
        private TimeTask current_DT;

        private static float _timer;
        public static float Game_Time
        {
            get
            {
                if (_timer != 0)
                {
                    return _timer;
                }
                else
                {
                    return PlayerPrefs.GetFloat("Game_Time");
                }
            }
            private set
            {
                if (value > int.MaxValue)
                {
                    value = 0;
                }
                _timer = value;
            }
        }

        private void Awake()
        {
            Game_Time = PlayerPrefs.GetFloat("Game_Time");
        }

        private void Update()
        {
            #region 计时器
            Game_Time += Time.deltaTime;
            #endregion

            #region 循环任务
            loop_eunmer = Loop_Task.GetEnumerator();

            while (loop_eunmer.MoveNext())
            {
                current_LT = loop_eunmer.Current.Value;

                if (Time.time >= current_LT.timer)
                {
                    if (current_LT.callBack != null)
                    {
                        current_LT.callBack();
                    }
                    current_LT.timer = Time.time + current_LT.interval;
                }
            }

            if (loop_cache.Count > 0)
            {
                for (int i = loop_cache.Count - 1; i >= 0; i--)
                {
                    if (Loop_Task.ContainsKey(loop_cache[i]))
                    {
                        Loop_Task.Remove(loop_cache[i]);
                    }
                    loop_cache.RemoveAt(i);
                }
            }
            #endregion

            #region 延时任务
            delay_eunmer = Delay_Task.GetEnumerator();

            while (delay_eunmer.MoveNext())
            {
                current_DT = delay_eunmer.Current.Value;

                if (Time.time >= current_DT.timer)
                {
                    if (current_DT.callBack != null)
                    {
                        current_DT.callBack();
                    }
                    delay_cache.Add(delay_eunmer.Current.Key);
                }
            }

            if (delay_cache.Count > 0)
            {
                for (int i = delay_cache.Count - 1; i >= 0; i--)
                {
                    if (Delay_Task.ContainsKey(delay_cache[i]))
                    {
                        Delay_Task.Remove(delay_cache[i]);
                    }
                    delay_cache.RemoveAt(i);
                }
            }
            #endregion
        }

        public static void RegisterLoopEvent(string eventKey, TimeTask task)
        {
            if (Loop_Task.ContainsKey(eventKey))
            {
                Loop_Task[eventKey] = task;
            }
            else
            {
                Loop_Task.Add(eventKey, task);
            }
        }

        public static void UnregisterLoopEvent(string eventKey)
        {
            if (Loop_Task.ContainsKey(eventKey))
            {
                loop_cache.Add(eventKey);
            }
        }

        public static void RegisterDelayEvent(string eventKey, TimeTask task)
        {
            if (Delay_Task.ContainsKey(eventKey))
            {
                Delay_Task[eventKey] = task;
            }
            else
            {
                Delay_Task.Add(eventKey, task);
            }
        }

        public static void UnregisterDelayEvent(string eventKey)
        {
            if (Delay_Task.ContainsKey(eventKey))
            {
                delay_cache.Add(eventKey);
            }
        }

        public static string ToTime(int number)
        {
            StringBuilder sb = new StringBuilder();

            if (number > 86400)
            {
                int day = number / 86400;
                int hour = number % 86400 / 3600;
                int minute = number % 3600 / 60;
                int second = number % 60;

                sb.Append(day);
                sb.Append("d");
                sb.Append(hour < 10 ? "0" + hour.ToString() : hour.ToString());
                sb.Append("h");
                sb.Append(minute < 10 ? "0" + minute.ToString() : minute.ToString());
                sb.Append("m");
                sb.Append(second < 10 ? "0" + second.ToString() : second.ToString());
                sb.Append("s");
            }
            else if (number > 3600)
            {
                int hour = number / 3600;
                int minute = number % 3600 / 60;
                int second = number % 60;

                sb.Append(hour < 10 ? "0" + hour.ToString() : hour.ToString());
                sb.Append(":");
                sb.Append(minute < 10 ? "0" + minute.ToString() : minute.ToString());
                sb.Append(":");
                sb.Append(second < 10 ? "0" + second.ToString() : second.ToString());
            }
            else
            {
                int minute = number / 60;
                int second = number % 60;

                sb.Append(minute < 10 ? "0" + minute.ToString() : minute.ToString());
                sb.Append(":");
                sb.Append(second < 10 ? "0" + second.ToString() : second.ToString());
            }

            return sb.ToString();
        }

        private void OnDestroy()
        {
            PlayerPrefs.SetFloat("Game_Time", Game_Time);
        }
    }

    public class TimeTask : IDisposable
    {
        public float timer;
        public float interval;
        public Action callBack;

        private TimeTask() { }

        public TimeTask(float time, Action callBack)
        {
            timer = Time.time + time;
            this.callBack = callBack;
        }

        public TimeTask(float time, float interval, Action callBack)
        {
            timer = Time.time + time;
            this.interval = interval;
            this.callBack = callBack;
        }

        public void Dispose()
        {
            callBack = null;
        }
    }
}