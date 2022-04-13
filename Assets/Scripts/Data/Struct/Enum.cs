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
        /// 货币
        /// </summary>
        Currency,
        /// <summary>
        /// 道具
        /// </summary>
        Prop,
        /// <summary>
        /// 充值
        /// </summary>
        Recharge,
    }
}
