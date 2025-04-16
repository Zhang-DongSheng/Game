using System.Collections.Generic;

namespace Game
{
    public static partial class Utility
    {
        /// <summary>
        /// 集合
        /// </summary>
        public static class Set
        {
            /// <summary>
            /// 等于
            /// </summary>
            public static bool Equal<T>(List<T> list, List<T> other)
            {
                if (list == null || other == null) return false;

                if (list.Count != other.Count) return false;

                var result = new List<T>(other);

                var count = list.Count;

                for (int i = 0; i < count; i++)
                {
                    var index = result.FindIndex(x => x.Equals(list[i]));

                    if (index > -1)
                    {
                        result.RemoveAt(index);
                    }
                }
                return result.Count == 0;
            }
            /// <summary>
            /// 交集
            /// </summary>
            public static List<T> Intersection<T>(List<T> list, List<T> other)
            {
                var result = new List<T>();

                var count = list.Count;

                for (int i = 0; i < count; i++)
                {
                    if (other.Exists(x => x.Equals(list[i])))
                    {
                        result.Add(list[i]);
                    }
                }
                return result;
            }
            /// <summary>
            /// 并集
            /// </summary>
            public static List<T> Union<T>(List<T> list, List<T> other)
            {
                var result = new List<T>(other);

                var count = list.Count;

                for (int i = 0; i < count; i++)
                {
                    if (result.Exists(x => x.Equals(list[i])))
                    {
                        // exist the smae element.
                    }
                    else
                    {
                        result.Add(list[i]);
                    }
                }
                return result;
            }
            /// <summary>
            /// 补集
            /// </summary>
            public static List<T> Complement<T>(List<T> list, List<T> other)
            {
                var result = new List<T>();

                var count = list.Count;

                for (int i = 0; i < count; i++)
                {
                    if (other.Exists(x => x.Equals(list[i])))
                    {

                    }
                    else
                    {
                        result.Add(list[i]);
                    }
                }
                count = other.Count;

                for (int i = 0; i < count; i++)
                {
                    if (list.Exists(x => x.Equals(other[i])))
                    {

                    }
                    else
                    {
                        result.Add(list[i]);
                    }
                }
                return result;
            }
            /// <summary>
            /// 开区间
            /// </summary>
            public static List<int> OpenInterval(int min, int max)
            {
                var result = new List<int>();

                for (int i = min + 1; i < max; i++)
                {
                    result.Add(i);
                }
                return result;
            }
            /// <summary>
            /// 闭区间
            /// </summary>
            public static List<int> ClosedInterval(int min, int max)
            {
                var result = new List<int>();

                for (int i = min; i < max + 1; i++)
                {
                    result.Add(i);
                }
                return result;
            }
        }
    }
}