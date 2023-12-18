namespace Data
{
    public enum Status
    {
        /// <summary>
        /// 未完成
        /// </summary>
        Undone,
        /// <summary>
        /// 可领取
        /// </summary>
        Available,
        /// <summary>
        /// 已领取
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