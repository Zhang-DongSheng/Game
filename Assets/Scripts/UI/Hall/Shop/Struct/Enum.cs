namespace Data
{
    public enum Status
    {
        /// <summary>
        /// δ���
        /// </summary>
        Undone,
        /// <summary>
        /// ����ȡ
        /// </summary>
        Available,
        /// <summary>
        /// ����ȡ
        /// </summary>
        Claimed,
    }

    public enum Consume
    {
        /// <summary>
        /// ����
        /// </summary>
        Currency,
        /// <summary>
        /// �һ�
        /// </summary>
        Exchange,
        /// <summary>
        /// ��ֵ
        /// </summary>
        Recharge,
    }
}