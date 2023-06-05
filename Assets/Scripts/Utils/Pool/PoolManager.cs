using System.Collections.Generic;

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
                m_pool.Add(key, new PoolElement(key, transform));
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
                var element = new PoolElement(key, transform);

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
    }
}