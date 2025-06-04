using System.Collections.Generic;

namespace Game
{
    public class LimitedRandom<T>
    {
        private readonly List<T> list = new List<T>();

        public int Count
        {
            get { return list.Count; }
        }

        public T Next()
        {
            int count = list.Count;

            int index = UnityEngine.Random.Range(0, count);

            var result = list[index];

            list.RemoveAt(index);

            return result;
        }

        public void Add(T item)
        {
            list.Add(item);
        }

        public void AddRange(IEnumerable<T> collection)
        {
            list.AddRange(collection);
        }

        public void RemoveAt(int index)
        {
            int count = list.Count;

            if (count > index)
            {
                list.RemoveAt(index);
            }
        }

        public void Clear()
        {
            list.Clear();
        }

        public static LimitedRandom<int> Range(int min, int max)
        {
            var radom = new LimitedRandom<int>();

            for (int i = min; i < max; i++)
            {
                radom.list.Add(i);
            }
            return radom;
        }
    }
}