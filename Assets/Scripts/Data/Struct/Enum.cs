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

    public enum Quality
    {
        None,
        /// <summary>
        /// 黑铁
        /// </summary>
        Iron = 1,
        /// <summary>
        /// 青铜
        /// </summary>
        Bronze = 2,
        /// <summary>
        /// 白银
        /// </summary>
        Silver = 3,
        /// <summary>
        /// 黄金
        /// </summary>
        Gold = 4,
        /// <summary>
        /// 史诗
        /// </summary>
        Epic = 5,
        /// <summary>
        /// 神话
        /// </summary>
        Myth = 6,
        /// <summary>
        /// 专属
        /// </summary>
        Exclusive = 7,
    }

    public enum Consume
    {
        /// <summary>
        /// 货币
        /// </summary>
        Currency,
        /// <summary>
        /// 兑换
        /// </summary>
        Exchange,
        /// <summary>
        /// 充值
        /// </summary>
        Recharge,
    }
}