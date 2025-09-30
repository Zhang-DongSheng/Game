using System.Collections.Generic;
using UnityEngine;

namespace Game.Pool
{
    public class PoolElement
    {
        private readonly string key;

        private readonly Transform root;

        private readonly GameObject prefab;

        private readonly Stack<GameObject> m_stack = new Stack<GameObject>();

        private readonly PoolReference m_reference = new PoolReference(PoolConfig.Sample, PoolConfig.Interval);

        private int number, capacity;

        public PoolElement(string key, GameObject prefab)
        {
            this.key = key;

            this.root = new GameObject(string.Format("[{0}]", key)).transform;

            this.root.SetParent(PoolManager.Instance.transform);

            this.prefab = prefab;

            this.capacity = PoolConfig.Min;
        }

        public void Detection()
        {
            m_reference.Update(Time.deltaTime, number);

            capacity = Mathf.Clamp(m_reference.Reference, PoolConfig.Min, PoolConfig.Max);

            if (m_stack.Count > capacity)
            {
                //m_stack.Pop();
            }
        }

        public GameObject Pop()
        {
            number++;

            GameObject model;

            if (m_stack.Count > 0)
            {
                model = m_stack.Pop();

                model.SetActive(true);
            }
            else
            {
                model = GameObject.Instantiate(prefab, root);
            }
            return model;
        }

        public void Push(GameObject value)
        {
            if (value == null) return;

            number--;

            foreach (var item in m_stack)
            {
                if (item.GetHashCode() == value.GetHashCode())
                {
                    Debuger.LogWarning(Author.Resource, "the same pool object : " + key);
                    return;
                }
            }
            value.SetActive(false);

            value.transform.SetParent(root);

            m_stack.Push(value);
        }

        public void Clear()
        {
            while (m_stack.Count > 0)
            {
                GameObject.Destroy(m_stack.Pop());
            }
            number = 0;
        }
    }
}