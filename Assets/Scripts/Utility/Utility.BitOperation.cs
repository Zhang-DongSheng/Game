namespace Game
{
    public static partial class Utility
    {
        public static class BitOperation
        {
            /// <summary>
            /// 与（&）
            /// </summary>
            /// <returns>与只有两个位都是1，结果才是1</returns>
            public static int With(int a, int b)
            {
                return a & b;
            }
            /// <summary>
            /// 或（|）
            /// </summary>
            /// <returns>只要两个位有一个是1，结果就是1</returns>
            public static int Or(int a, int b)
            {
                return a | b;
            }
            /// <summary>
            /// 非（~）
            /// </summary>
            /// <returns>如果位为0，结果是1，如果位为1，结果是0</returns>
            public static int Non(int value)
            {
                return ~value;
            }
            /// <summary>
            /// 异或（^）
            /// </summary>
            /// <returns>两个操作数的位中，相同则结果为0，不同则结果为1</returns>
            public static int Heteror(int a, int b)
            {
                return a ^ b;
            }
            /// <summary>
            /// 左移（<<）
            /// </summary>
            /// <returns>左移表示乘以2，左移多少位表示乘以2的几次幂</returns>
            public static int Left(int value, int shift)
            {
                return value << shift;
            }
            /// <summary>
            /// 右移（>>）
            /// </summary>
            /// <returns>移动多少位表示除以2的几次幂</returns>
            public static int Right(int value, int shift)
            {
                return value >> shift;
            }
            /// <summary>
            /// 十进制转二进制
            /// </summary>
            public static string Convert(int value, int length = 0)
            {
                string content = "";

                while (value / 2 != 0)
                {
                    content = value % 2 + content;
                    value /= 2;
                }
                if (value != 0)
                    content = value + content;

                while (content.Length < length)
                {
                    content = "0" + content;
                }
                return content;
            }
        }
    }
}