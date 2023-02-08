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
        }
    }
}