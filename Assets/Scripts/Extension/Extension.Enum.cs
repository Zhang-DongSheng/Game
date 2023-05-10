using System;

namespace Game
{
    public static partial class Extension
    {
        public static int Index(this Enum value)
        {
            int index = 0;

            var array = Enum.GetValues(value.GetType());

            foreach (var item in array)
            {
                if (item == value)
                {
                    break;
                }
                index++;
            }
            return index;
        }

        public static int Count(this Enum value)
        {
            var array = Enum.GetValues(value.GetType());

            return array.Length;
        }
    }
}