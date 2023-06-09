using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Pool
{
    public class PoolElement
    {
        private readonly string key;

        private readonly Transform parent;

        private readonly Func<string, GameObject> create;

        private readonly Stack<PoolObject> m_stack = new Stack<PoolObject>();

        private readonly PoolReference m_reference = new PoolReference(PoolCinfig.Sample, PoolCinfig.Interval);

        private int number, capacity;

        public PoolElement(string key, Transform parent, Func<string, GameObject> create)
        {
            this.key = key;

            this.parent = parent;

            this.create = create;

            this.capacity = PoolCinfig.Min;
        }

        public void Detection()
        {
            m_reference.Update(Time.deltaTime, number);

            capacity = Mathf.Clamp(m_reference.Reference, PoolCinfig.Min, PoolCinfig.Max);

            if (m_stack.Count > capacity)
            {
                m_stack.Pop().OnRemove();
            }
        }

        public PoolObject Create()
        {
            number++;

            var instance = create.Invoke(key);

            if (instance == null) return null;

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