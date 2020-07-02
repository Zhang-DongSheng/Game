using System;
using System.Collections.Generic;

namespace UnityEngine
{
    /// <summary>
    /// 事件管理器
    /// </summary>
    public class EventManager
    {
        private readonly static Dictionary<string, Action<EventMessageArgs>> eventTask = new Dictionary<string, Action<EventMessageArgs>>();

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="eventKey">事件索引</param>
        /// <param name="actionValue">事件回调</param>
        public static void RegisterEvent(string eventKey, Action<EventMessageArgs> actionValue)
        {
            if (!eventTask.ContainsKey(eventKey))
            {
                eventTask[eventKey] = actionValue;
            }
            else
            {
                if (eventTask[eventKey] != null)
                {
                    Delegate[] dels = eventTask[eventKey].GetInvocationList();
                    foreach (Delegate del in dels)
                    {
                        if (del.Equals(actionValue))
                            return;
                    }
                }
                eventTask[eventKey] += actionValue;
            }
        }

        /// <summary>
        /// 注销事件
        /// </summary>
        /// <param name="eventKey">事件索引</param>
        /// <param name="actionValue">事件回调</param>
        public static void UnregisterEvent(string eventKey, Action<EventMessageArgs> actionValue)
        {
            if (eventTask.ContainsKey(eventKey))
            {
                eventTask[eventKey] -= actionValue;

                if (eventTask[eventKey] == null)
                {
                    eventTask.Remove(eventKey);
                }
            }
        }

        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="eventKey">事件索引</param>
        /// <param name="args">消息内容（只在单次触发有效，若要连续传递请使用 CopyMessage ）</param>
        public static void PostEvent(string eventKey, EventMessageArgs args)
        {
            if (eventTask.ContainsKey(eventKey))
            {
                eventTask[eventKey](args);
            }
            args.Dispose();
        }
    }

    /// <summary>
    /// 事件通知数据
    /// </summary>
    public class EventMessageArgs : IDisposable
    {
        public EventMessageArgs()
        {
            messages = new Dictionary<string, object>();
        }

        private Dictionary<string, object> messages;

        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="msg"></param>
        public void CopyMessage(EventMessageArgs msg)
        {
            messages = msg.messages;
        }

        /// <summary>
        /// 新增或替换
        /// </summary>
        /// <param name="key">索引</param>
        /// <param name="value">新数据</param>
        public void AddOrReplaceMessage(string key, object value)
        {
            if (CheckMessage(key))
                messages[key] = value;
            else
                messages.Add(key, value);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key">索引</param>
        public void RemoveMessage(string key)
        {
            if (CheckMessage(key))
                messages.Remove(key);
        }

        /// <summary>
        /// 获取内容
        /// </summary>
        /// <param name="key">索引</param>
        /// <returns>引用类型数据</returns>
        public object GetMessage(string key)
        {
            if (CheckMessage(key))
                return messages[key];
            else
                return null;
        }

        /// <summary>
        /// 获取内容
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">索引</param>
        /// <returns>数据</returns>
        public T GetMessage<T>(string key)
        {
            if (CheckMessage(key))
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
            messages = null;
        }

        private bool CheckMessage(string key)
        {
            return messages.ContainsKey(key);
        }
    }
}