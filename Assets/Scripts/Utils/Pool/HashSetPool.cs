using System.Collections.Generic;

namespace Game.Pool
{
    public class HashSetPool<T>
    {
        private static ObjectPool<HashSet<T>> m_pools = new ObjectPool<HashSet<T>>(null, x => x.Clear());

        public static HashSet<T> Pop()
        {
            return m_pools.Pop();
        }

        public static void Push(HashSet<T> hashSet)
        {
            m_pools.Push(hashSet);
        }
    }
}