using System;
using System.Collections.Generic;

namespace UnityEngine
{
    /// <summary>
    /// 事件管理器
    /// </summary>
    public class EventManager
    {
        private readonly static Dictionary<EventKey, Action<EventMessageArgs>> events = new Dictionary<EventKey, Action<EventMessageArgs>>();

        /// <summary>
        /// 注册事件
        /// </summary>
        public static void RegisterEvent(EventKey key, Action<EventMessageArgs> action)
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
        public static void UnregisterEvent(EventKey key, Action<EventMessageArgs> value)
        {
            if (events.ContainsKey(key))
            {
                events[key] -= value;

                if (events[key] == null)
                {
                    events.Remove(key);
                }
            }
        }
        /// <summary>
        /// 触发事件
        /// </summary>
        public static void PostEvent(EventKey key, EventMessageArgs args = null)
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
    public class EventMessageArgs : IDisposable
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
        /// 新增或替换
        /// </summary>
        public void AddOrReplaceMessage(string key, object value)
        {
            if (Contains(key))
                messages[key] = value;
            else
                messages.Add(key, value);
        }
        /// <summary>
        /// 删除
        /// </summary>
        public void RemoveMessage(string key)
        {
            if (Contains(key))
                messages.Remove(key);
        }
        /// <summary>
        /// 获取内容
        /// </summary>
        public object GetMessage(string key)
        {
            if (Contains(key))
                return messages[key];
            else
                return null;
        }
        /// <summary>
        /// 是否包含
        /// </summary>
        private bool Contains(string key)
        {
            return messages.ContainsKey(key);
        }
        /// <summary>
        /// 获取内容
        /// </summary>
        public T GetMessage<T>(string key)
        {
            if (Contains(key))
                return (T)messages[key];
            else
                return default(T);
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