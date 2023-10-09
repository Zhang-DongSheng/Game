using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Pool
{
    public class PoolElement
    {
        private readonly string key;

        private readonly Transform root;

        private readonly Func<string, GameObject> create;

        private readonly Stack<PoolObject> m_stack = new Stack<PoolObject>();

        private readonly PoolReference m_reference = new PoolReference(PoolConfig.Sample, PoolConfig.Interval);

        private int number, capacity;

        public PoolElement(string key, Func<string, GameObject> create)
        {
            this.key = key;

            this.root = new GameObject(string.Format("[{0}]", key)).transform;

            this.root.parent = PoolManager.Instance.transform;

            this.create = create;

            this.capacity = PoolConfig.Min;
        }

        public void Detection()
        {
            m_reference.Update(Time.deltaTime, number);

            capacity = Mathf.Clamp(m_reference.Reference, PoolConfig.Min, PoolConfig.Max);

            if (m_stack.Count > capacity)
            {
                m_stack.Pop().OnRemove();
            }
        }

        public PoolObject Pop()
        {
            number++;

            PoolObject value;

            if (m_stack.Count > 0)
            {
                value = m_stack.Pop();
            }
            else
            {
                var instance = create.Invoke(key);

                if (instance == null) return null;

                if (instance.TryGetComponent(out value))
                {

                }
                else
                {
                    value = instance.AddComponent<PoolObject>();
                }
                value.OnCreate(key);
            }
            value.OnFetch(); return value;
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
            value.OnRecycle(root); m_stack.Push(value);
        }

        public void Clear()
        {
            while (m_stack.Count > 0)
            {
                m_stack.Pop().OnRemove();
            }
            number = 0;
        }
    }
}