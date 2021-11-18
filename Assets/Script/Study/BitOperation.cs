namespace Study
{
    /// <summary>
    /// λ����
    /// </summary>
    public static class BitOperation
    {
        /// <summary>
        /// �루&��
        /// </summary>
        /// <returns>��ֻ������λ����1���������1</returns>
        public static int With(int a, int b)
        {
            return a & b;
        }
        /// <summary>
        /// ��|��
        /// </summary>
        /// <returns>ֻҪ����λ��һ����1���������1</returns>
        public static int Or(int a, int b)
        {
            return a | b;
        }
        /// <summary>
        /// �ǣ�~��
        /// </summary>
        /// <returns>���λΪ0�������1�����λΪ1�������0</returns>
        public static int Non(int value)
        {
            return ~value;
        }
        /// <summary>
        /// ���^��
        /// </summary>
        /// <returns>������������λ�У���ͬ����Ϊ0����ͬ����Ϊ1</returns>
        public static int Heteror(int a, int b)
        {
            return a ^ b;
        }
        /// <summary>
        /// ���ƣ�<<��
        /// </summary>
        /// <returns>���Ʊ�ʾ����2�����ƶ���λ��ʾ����2�ļ�����</returns>
        public static int Left(int value, int shift)
        {
            return value << shift;
        }
        /// <summary>
        /// ���ƣ�>>��
        /// </summary>
        /// <returns>�ƶ�����λ��ʾ����2�ļ�����</returns>
        public static int Right(int value, int shift)
        {
            return value >> shift;
        }
        /// <summary>
        /// ʮ����ת������
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