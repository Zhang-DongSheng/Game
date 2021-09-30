using Data;
using System;
using System.Collections.Generic;

namespace UnityEngine
{
    public sealed class Workshop : IDisposable
    {
        private readonly string identification;

        private readonly int capacity = -1;

        private readonly string secret;

        private readonly Object asset;

        private readonly Stack<Object> memory = new Stack<Object>();

        public Workshop(ResourceInformation resource)
        {
            identification = resource.key;

            secret = resource.secret;

            capacity = resource.capacity;

            this.asset = resource.asset;
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
                return GameObject.Instantiate(asset);
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