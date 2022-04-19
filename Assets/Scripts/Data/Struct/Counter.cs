using System.Collections.Generic;

namespace Data
{
    public class Counter
    {
        public CounterCategory category;

        public string name;

        public long time;

        public List<Commodity> commodities;
    }

    public enum CounterCategory
    {
        Sell,
        Package,
        Recharge,
    }
}