using System.Collections.Generic;

namespace Data
{
    /// <summary>
    /// 柜台 - 售卖商品
    /// </summary>
    public class Counter
    {
        public CounterEnum counter;

        public string name;

        public long time;

        public List<Commodity> commodities;
    }

    public enum CounterEnum
    {
        Recharge,
        Sell,
        Package,
    }
}
