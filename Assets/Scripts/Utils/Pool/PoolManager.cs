using Game.Resource;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Pool
{
    public class PoolManager : MonoSingleton<PoolManager>
    {
        private readonly Dictionary<string, PoolElement> m_pool = new Dictionary<string, PoolElement>();

        private void Update()
        {
            foreach (var element in m_pool.Values)
            {
                element.Detection();
            }
        }
        /// <summary>
        /// 取出
        /// </summary>
        public void Pop(string key, Action<GameObject> callback)
        {
            if (m_pool.TryGetValue(key, out var element))
            {
                callback?.Invoke(element.Pop());
            }
            else
            {
                ResourceManager.LoadAsync<GameObject>(key, (obj) =>
                {
                    if (obj != null)
                    {
                        var element = new PoolElement(key, obj);
                        m_pool.Add(key, element);
                        callback?.Invoke(element.Pop());
                    }
                    else
                    {
                        Debuger.LogError(Author.Resource, "load pool prefab error : " + key);
                        callback?.Invoke(null);
                    }
                });
            }
        }
        /// <summary>
        /// 存入
        /// </summary>
        public void Push(string key, GameObject value)
        {
            if (m_pool.TryGetValue(key, out var element))
            {
                element.Push(value);
            }
            else
            {
                GameObject.Destroy(value);
                Debuger.LogWarning(Author.Resource, "can't exist pool : " + key);
            }
        }
        /// <summary>
        /// 清除
        /// </summary>
        public void Clear(string key, bool remove = false)
        {
            if (string.IsNullOrEmpty(key)) return;

            if (m_pool.ContainsKey(key))
            {
                m_pool[key].Clear();

                if (remove)
                {
                    m_pool.Remove(key);
                }
            }
        }
        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            foreach (var element in m_pool.Values)
            {
                element.Clear();
            }
            m_pool.Clear();
        }
    }
}