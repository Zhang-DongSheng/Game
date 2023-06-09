using Game.Resource;
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

        public PoolObject Pop(string key)
        {
            if (m_pool.ContainsKey(key))
            {
                if (m_pool[key].Count > 0)
                {
                    return m_pool[key].Pop();
                }
            }
            else
            {
                m_pool.Add(key, new PoolElement(key, transform, Create));
            }
            return m_pool[key].Create();
        }

        public void Push(string key, PoolObject value)
        {
            if (m_pool.ContainsKey(key))
            {
                m_pool[key].Push(value);
            }
            else
            {
                var element = new PoolElement(key, transform, Create);

                element.Push(value);

                m_pool.Add(key, element);
            }
        }

        public void Clear(string key)
        {
            if (string.IsNullOrEmpty(key)) return;

            if (m_pool.ContainsKey(key))
            {
                while (m_pool[key].Count > 0)
                {
                    m_pool[key].Clear();
                }
                m_pool.Remove(key);
            }
        }

        public GameObject Create(string path)
        {
            var prefab = ResourceManager.Load<GameObject>(path);

            return GameObject.Instantiate<GameObject>(prefab);
        }
    }
    /// <summary>
    /// 对象池常量
    /// </summary>
    public static class PoolCinfig
    {
        public const int Min = 10;

        public const int Max = 100;

        public const int Sample = 30;

        public const float Interval = 1;
    }
}