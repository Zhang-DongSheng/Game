using System;
using System.Collections.Generic;

namespace UnityEngine
{
    /// <summary>
    /// 事件管理器
    /// </summary>
    public static class EventManager
    {
        private static readonly Dictionary<EventKey, Action<EventMessageArgs>> events = new Dictionary<EventKey, Action<EventMessageArgs>>();
        /// <summary>
        /// 注册事件
        /// </summary>
        public static void Register(EventKey key, Action<EventMessageArgs> action)
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
        public static void Unregister(EventKey key, Action<EventMessageArgs> action)
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
        public static void Post(EventKey key, EventMessageArgs args = null)
        {
            if (events.ContainsKey(key))
            {
                events[key](args);
            }
            args?.Dispose();
        }
    }
    /// <summary>
    /// 事件通知数据
    /// </summary>
    public sealed class EventMessageArgs : IDisposable
    {
        private readonly Dictionary<string, object> messages;

        public EventMessageArgs()
        {
            messages = new Dictionary<string, object>();
        }
        /// <summary>
        /// 复制
        /// </summary>
        public EventMessageArgs Clone()
        {
            return MemberwiseClone() as EventMessageArgs;
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
                    Debug.LogException(e);
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