using System.Collections.Generic;
using UnityEngine;

namespace Game.Pool
{
    public class PoolElement
    {
        private readonly string key;

        private readonly Transform parent;

        private readonly Stack<PoolObject> m_stack = new Stack<PoolObject>();

        private readonly PoolReference m_reference = new PoolReference(30, 1);

        private int number, capacity;

        public PoolElement(string key ,Transform parent)
        {
            this.key = key;

            this.capacity = 10;

            this.parent = parent;
        }

        public void Detection()
        {
            m_reference.Update(Time.deltaTime, number);

            capacity = Mathf.Clamp(m_reference.Reference, 10, 100);

            if (m_stack.Count > capacity)
            {
                m_stack.Pop().OnRemove();
            }
        }

        public PoolObject Create()
        {
            number++;

            GameObject prefab = Game.Resource.ResourceManager.Load<GameObject>(key);

            Debuger.Assert(prefab != null, string.Format("PoolObject Error. Load is Null: {0}", key));

            GameObject instance = GameObject.Instantiate(prefab);

            instance.name = prefab.name;

            if (!instance.TryGetComponent(out PoolObject component))
            {
                component = instance.AddComponent<PoolObject>();
            }
            component.OnCreate(key);

            return component;
        }

        public PoolObject Pop()
        {
            number++;

            var value = m_stack.Pop();

            value.OnFetch();

            return value;
        }

        public void Push(PoolObject value)
        {
            number--;

            foreach (var item in m_stack)
            {
                if (ReferenceEquals(item, value))
                {
                    return;
                }
            }
            value.OnRecycle(parent);

            m_stack.Push(value);
        }

        public void Clear()
        {
            while (m_stack.Count > 0)
            {
                m_stack.Pop().OnRemove();
            }
            number = 0;
        }

        public int Count => m_stack.Count;
    }
}
