using System.Collections.Generic;

namespace Data
{
    /// <summary>
    /// ��̨ - ������Ʒ
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
