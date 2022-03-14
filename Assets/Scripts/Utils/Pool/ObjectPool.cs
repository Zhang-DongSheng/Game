using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Pool
{
    public class ObjectPool<T> where T : class, new()
    {
        private readonly Stack<T> m_stack = new Stack<T>();

        private readonly Action<T> m_pop;

        private readonly Action<T> m_push;

        public ObjectPool(Action<T> pop, Action<T> push)
        {
            m_pop = pop;

            m_push = push;
        }

        public T Pop()
        {
            T value;

            if (m_stack.Count > 0)
            {
                value = m_stack.Pop();
            }
            else
            {
                value = new T();
            }
            m_pop?.Invoke(value); return value;
        }

        public void Push(T value)
        {
            if (value == null) return;

            if (m_stack.Count > 0 && ReferenceEquals(m_stack.Peek(), value))
            {
                Debug.LogError(string.Format("ObjectPool error. Trying to push object that is already in the pool, obj = {0}", value));
                return;
            }
            m_stack.Push(value); m_push?.Invoke(value);
        }
    }
}