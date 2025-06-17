using System;
using System.Collections.Generic;

namespace UnityEngine
{
    /// <summary>
    /// 事件派发器
    /// </summary>
    public static class EventDispatcher
    {
        private static readonly Dictionary<string, Action<EventArgs>> events = new Dictionary<string, Action<EventArgs>>();
        /// <summary>
        /// 注册事件
        /// </summary>
        public static void Register(string key, Action<EventArgs> action)
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
        /// <summary>
        /// 注销事件
        /// </summary>
        public static void Unregister(string key, Action<EventArgs> action)
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
        /// <summary>
        /// 触发事件
        /// </summary>
        public static void Post(string key, EventArgs args = null)
        {
            if (events.ContainsKey(key))
            {
                events[key](args);
            }
            args?.Dispose();
        }
    }
    /// <summary>
    /// 事件参数
    /// </summary>
    public sealed class EventArgs : IDisposable
    {
        private readonly Dictionary<string, object> messages;

        public EventArgs(params object[] parameters)
        {
            messages = new Dictionary<string, object>();

            int length = parameters.Length;

            for (int i = 0; i < length; i++)
            {
                AddOrReplace(i.ToString(), parameters[i]);
            }
        }
        /// <summary>
        /// 索引访问器
        /// </summary>
        public object this[string key]
        {
            get
            {
                return Get(key);
            }
            set
            {
                AddOrReplace(key, value);
            }
        }
        /// <summary>
        /// 索引访问器
        /// </summary>
        public object this[int index]
        {
            get
            {
                foreach (var value in messages.Values)
                {
                    if (index-- == 0)
                    {
                        return value;
                    }
                }
                return null;
            }
        }
        /// <summary>
        /// 新增或替换
        /// </summary>
        public void AddOrReplace(string key, object value)
        {
            if (Contains(key))
            {
                messages[key] = value;
            }
            else
            {
                messages.Add(key, value);
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        public void Remove(string key)
        {
            if (Contains(key))
            {
                messages.Remove(key);
            }
        }
        /// <summary>
        /// 获取内容
        /// </summary>
        public object Get(string key)
        {
            if (Contains(key))
            {
                return messages[key];
            }
            return null;
        }
        /// <summary>
        /// 获取内容
        /// </summary>
        public T Get<T>(string key)
        {
            if (Contains(key))
            {
                try
                {
                    return (T)messages[key];
                }
                catch (Exception e)
                {
                    Debuger.LogException(Author.Script, e);
                }
            }
            return default;
        }
        /// <summary>
        /// 是否包含
        /// </summary>
        private bool Contains(string key)
        {
            return messages.ContainsKey(key);
        }
        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            messages.Clear();
        }

        public override string ToString()
        {
            return string.Join(",", messages);
        }
    }
}