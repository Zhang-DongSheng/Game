using System.Collections.Generic;

namespace Game
{
    public static partial class Utility
    {
        /// <summary>
        /// 枚举
        /// </summary>
        public static class Enum
        {
            public static int Index<T>(int key) where T : System.Enum
            {
                int index = 0;

                foreach (var value in System.Enum.GetValues(typeof(T)))
                {
                    if ((int)value == key)
                    {
                        return index;
                    }
                    index++;
                }
                return -1;
            }

            public static T FromString<T>(string key) where T : System.Enum
            {
                if (System.Enum.IsDefined(typeof(T), key))
                {
                    return (T)System.Enum.Parse(typeof(T), key);
                }
                return default(T);
            }

            public static List<T> GetList<T>() where T : System.Enum
            {
                var list = new List<T>();

                foreach (var e in System.Enum.GetValues(typeof(T)))
                {
                    list.Add((T)e);
                }
                return list;
            }
        }
    }
}