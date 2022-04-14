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

    public enum Quality
    {
        None,
        /// <summary>
        /// ����
        /// </summary>
        Iron = 1,
        /// <summary>
        /// ��ͭ
        /// </summary>
        Bronze = 2,
        /// <summary>
        /// ����
        /// </summary>
        Silver = 3,
        /// <summary>
        /// �ƽ�
        /// </summary>
        Gold = 4,
        /// <summary>
        /// ʷʫ
        /// </summary>
        Epic = 5,
        /// <summary>
        /// ��
        /// </summary>
        Myth = 6,
        /// <summary>
        /// ר��
        /// </summary>
        Exclusive = 7,
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