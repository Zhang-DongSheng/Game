using Data;
using System;
using System.Collections.Generic;

namespace UnityEngine.Factory
{
    public sealed class Workshop : IDisposable
    {
        private readonly string ID;

        private readonly int capacity = -1;

        private readonly string secret;

        private readonly Object prefab;

        private readonly Stack<Object> memory = new Stack<Object>();

        public Workshop(PrefabInformation prefab)
        {
            ID = prefab.key;

            secret = prefab.secret;

            capacity = prefab.capacity;

            this.prefab = prefab.prefab;
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
                return GameObject.Instantiate(prefab);
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

        public void Dispose()
        {

        }
    }
}