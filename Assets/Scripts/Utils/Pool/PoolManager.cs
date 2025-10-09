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
            var delta = Time.deltaTime;

            foreach (var element in m_pool.Values)
            {
                element.Update(delta);
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
                        Debuger.LogError(Author.Pool, $"The {key} pool instance failed to initialize");
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
                Debuger.LogWarning(Author.Pool, $"No {key} pool exists");
                GameObject.Destroy(value);
            }
        }
        /// <summary>
        /// 存入
        /// </summary>
        public void Push(GameObject value)
        {
            foreach (var element in m_pool.Values)
            {
                if (element.Equals(value))
                {
                    element.Push(value);
                    return;
                }
            }
            Debuger.LogWarning(Author.Pool, $"No {value.name} pool exists");
        }
        /// <summary>
        /// 清除
        /// </summary>
        public void Clear(string key, bool dispose = false)
        {
            if (string.IsNullOrEmpty(key)) return;

            if (m_pool.ContainsKey(key))
            {
                m_pool[key].Clear(dispose);

                if (dispose)
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
                element.Clear(true);
            }
            m_pool.Clear();
        }
    }
}