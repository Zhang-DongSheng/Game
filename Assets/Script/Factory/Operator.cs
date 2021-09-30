using Data;
using System;
using System.Collections.Generic;

namespace UnityEngine
{
    public abstract class Operator : IDisposable
    {
        protected readonly string identification;

        protected readonly int capacity = -1;

        protected string secret;

        protected Object asset;

        protected Stack<Object> memory = new Stack<Object>();

        public Operator(ResourceInformation resource)
        {
            identification = resource.key;

            secret = resource.secret;

            capacity = resource.capacity;

            this.asset = resource.asset;
        }

        public abstract Object Pop(string value);

        public abstract void Push(Object asset);

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