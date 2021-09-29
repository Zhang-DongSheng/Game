namespace Data
{
    public enum TaskStatus
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
        Received,
    }

    public enum CurrencyType
    {
        Gold,
        Diamond,
        Coin,
    }

    public enum ActionType
    {
        None,
        Cost,
        Kill,
        Talk,
        Time,
    }
}