using System.Collections.Generic;

namespace Game
{
    public static partial class Utility
    {
        /// <summary>
        /// 比较
        /// </summary>
        public static class Compare
        {
            public static bool List<T>(IList<T> list, IList<T> other)
            {
                if (list == null || other == null) return false;

                if (list.Count != other.Count) return false;

                var clone = new List<T>(other);

                int count = list.Count;

                for (int i = 0; i < count; i++)
                {
                    var index = clone.FindIndex(x => x.Equals(list[i]));

                    if (index > -1)
                    {
                        clone.RemoveAt(index);
                    }
                }
                return clone.Count == 0;
            }
        }
    }
}