using Game.Resource;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Pool
{
    public class PoolManager : MonoSingleton<PoolManager>
    {
        static Dictionary<string, Stack<PoolObject>> m_pool = new Dictionary<string, Stack<PoolObject>>();

        public PoolObject Pop(string key)
        {
            if (m_pool.ContainsKey(key) && m_pool[key].Count > 0)
            {
                var temp = m_pool[key].Pop();

                temp.OnFetch();

                return temp;
            }
            else
            {
                return Create(key);
            }
        }

        public void Push(string key, PoolObject value)
        {
            if (m_pool.ContainsKey(key))
            {
                if (m_pool[key].Count > 0 && ReferenceEquals(m_pool[key].Peek(), value))
                {
                    Debuger.LogWarning(Author.Resource, string.Format("PoolObject Error. Trying to push object that is already in the pool, obj = {0}", value));
                    return;
                }
                m_pool[key].Push(value);
            }
            else
            {
                var stack = new Stack<PoolObject>();

                stack.Push(value);

                m_pool.Add(key, stack);
            }
        }

        public PoolObject Create(string key)
        {
            GameObject prefab = ResourceManager.Load<GameObject>(key);

            Debuger.Assert(prefab == null, string.Format("PoolObject Error. Load is Null: {0}", key));

            GameObject instance = Instantiate(prefab);

            instance.name = prefab.name;

            PoolObject component = instance.GetComponent<PoolObject>();

            Debuger.Assert(component == null, string.Format("PoolObject Error. component is Null: {0}", key));

            component.OnCreate();

            return null;
        }

        public void Clear(string key)
        {
            if (string.IsNullOrEmpty(key)) return;

            if (m_pool.ContainsKey(key) && m_pool[key].Count > 0)
            {
                while (m_pool[key].Count > 0)
                {
                    m_pool[key].Pop().OnDestory();
                }
                m_pool.Remove(key);
            }
        }
    }
}