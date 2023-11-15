using System;
using System.Text;

namespace Game
{
    public static partial class Utility
    {
        /// <summary>
        /// ×Ö·û´®
        /// </summary>
        public static class String
        {
            private static readonly StringBuilder builder = new StringBuilder();

            public static float GetInt(string content)
            {
                builder.Clear();

                foreach (char c in content)
                {
                    if (Convert.ToInt32(c) >= 48 && Convert.ToInt32(c) <= 57)
                    {
                        builder.Append(c);
                    }
                    else if (Convert.ToInt32(c) == ASCii.HYPHEN)
                    {
                        if (builder.Length == 0)
                        {
                            builder.Append(c);
                        }
                    }
                }

                if (builder.Length > 0 && int.TryParse(builder.ToString(), out int value))
                { 
                    return value;
                }
                return -1;
            }

            public static float GetFloat(string content)
            {
                builder.Clear();

                foreach (char c in content)
                {
                    if (Convert.ToInt32(c) >= ASCii.NUMBER0 && Convert.ToInt32(c) <= ASCii.NUMBER9)
                    {
                        builder.Append(c);
                    }
                    else if (Convert.ToInt32(c) == ASCii.PERIOD)
                    {
                        builder.Append(c);
                    }
                    else if (Convert.ToInt32(c) == ASCii.HYPHEN)
                    {
                        if (builder.Length == 0)
                        {
                            builder.Append(c);
                        }
                    }
                }

                if (builder.Length > 0 && float.TryParse(builder.ToString(), out float value))
                {
                    return value;
                }
                return -1;
            }
        }
    }
}