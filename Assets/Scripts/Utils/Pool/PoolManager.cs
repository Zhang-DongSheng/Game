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
        /// ע�᡾���衿
        /// </summary>
        public void Register(string key, Func<string, GameObject> create)
        {
            if (string.IsNullOrEmpty(key)) return;

            if (m_pool.ContainsKey(key))
            {
                Debuger.LogError(Author.Resource, "pool exist the same key : " + key);
            }
            else
            {
                m_pool.Add(key, new PoolElement(key, create));
            }
        }
        /// <summary>
        /// ȡ��
        /// </summary>
        public PoolObject Pop(string key)
        {
            if (m_pool.ContainsKey(key))
            {
                return m_pool[key].Pop();
            }
            return null;
        }
        /// <summary>
        /// ����
        /// </summary>
        public void Push(string key, PoolObject value)
        {
            if (m_pool.ContainsKey(key))
            {
                m_pool[key].Push(value);
            }
            else
            {
                Debuger.LogWarning(Author.Resource, "can't exist pool : " + key);
            }
        }
        /// <summary>
        /// ���
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
        /// ���
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