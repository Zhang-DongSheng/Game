using System.Collections.Generic;

namespace Data
{
    public class Currency
    {
        public CurrencyType currency;

        public int number;

        public Currency(CurrencyType currency, int number)
        {
            this.currency = currency;

            this.number = number;
        }
    }

    public class Prop
    {
        public int identification;

        public int number;

        public int parallelism;
    }

    public class Reward
    {
        public string title;

        public List<Currency> currencies;

        public List<Prop> props;
    }

    public class Task
    {
        public int ID;

        public Dictionary<ActionType, int> progress = new Dictionary<ActionType, int>();

        public TaskStatus status;
    }
}