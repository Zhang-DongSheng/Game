using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Game.Pool
{
    public class PoolElement
    {
        private readonly string key;

        private readonly Transform root;

        private readonly GameObject prefab;

        private readonly List<int> m_list = new List<int>();

        private readonly Stack<GameObject> m_stack = new Stack<GameObject>();

        private GameObject model;

        private int number, capacity;

        public PoolElement(string key, GameObject prefab)
        {
            this.key = key;

            this.prefab = prefab;

            this.root = new GameObject(Path.GetFileNameWithoutExtension(key)).transform;

            this.root.SetParent(PoolManager.Instance.transform);

            this.capacity = PoolConfig.Min;
        }

        public void Detection()
        {

        }

        public GameObject Pop()
        {
            number++;

            model = null;

            if (m_stack.Count > 0)
            {
                model = m_stack.Pop();
            }
            else
            {
                model = GameObject.Instantiate(prefab, root);

                m_list.Add(model.GetHashCode());

                Debuger.LogWarning(Author.Pool, $"{key} - Hash:{string.Join(',', m_list)}");
            }
            model.SetActive(true);

            return model;
        }

        public void Push(GameObject go)
        {
            if (!Equals(go)) return;

            number--;

            if (m_stack.Count < capacity)
            {
                var hash = go.GetHashCode();

                if (m_stack.Exists(x => x.GetHashCode() == hash))
                {
                    Debuger.LogWarning(Author.Pool, $"The {key} pool already has the same object");
                }
                else
                {
                    go.SetActive(false);

                    go.transform.SetParent(root);

                    m_stack.Push(go);
                }
            }
            else
            {
                Debuger.LogWarning(Author.Pool, $"The {key} pool is full");
                GameObject.Destroy(go);
            }
        }

        public bool Equals(GameObject go)
        {
            if (go == null) return false;

            var hash = go.GetHashCode();

            return m_list.Exists(x => x == hash);
        }

        public void Clear(bool dispose = false)
        {
            m_list.Clear();

            while (m_stack.Count > 0)
            {
                var go = m_stack.Pop();

                if (go != null)
                {
                    GameObject.Destroy(go);
                }
            }
            number = 0;

            if (dispose)
            {
                GameObject.Destroy(root.gameObject);
            }
        }
    }
}