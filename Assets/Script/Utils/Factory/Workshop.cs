using System;
using System.Collections.Generic;

namespace UnityEngine.Factory
{
    public class Workshop
    {
        private readonly Transform parent;

        private readonly Object prefab;

        private readonly int capacity;

        private readonly Stack<Object> memory = new Stack<Object>();

        public Workshop(string path, Transform parent, int capacity = -1)
        {
            this.parent = parent;

            this.capacity = capacity;

            try
            {
                prefab = Resources.Load(path);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        ~Workshop()
        {
            if (Prefab != null)
            {
                Resources.UnloadAsset(Prefab);
            }
        }

        public void Push(Object asset)
        {
            if (memory.Count < capacity || capacity == -1)
            {
                memory.Push(asset);
            }
            else
            {
                GameObject.Destroy(asset);
            }
        }

        public Object Pop()
        {
            if (memory.Count > 0)
            {
                return memory.Pop();
            }
            else
            {
                return GameObject.Instantiate(Prefab, Parent);
            }
        }

        public void Clear()
        {
            while (memory.Count > 0)
            {
                GameObject.Destroy(memory.Pop());
            }
            memory.Clear();
        }

        protected Transform Parent { get { return parent; } }

        protected Object Prefab { get { return prefab; } }
    }
}