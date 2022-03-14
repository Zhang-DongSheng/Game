using System.Collections.Generic;

namespace Game.Pool
{
    public class ListPool<T>
    {
        private static ObjectPool<List<T>> m_pools = new ObjectPool<List<T>>(null, x => x.Clear());

        public static List<T> Pop()
        {
            return m_pools.Pop();
        }

        public static void Push(List<T> list)
        {
            m_pools.Push(list);
        }
    }
}