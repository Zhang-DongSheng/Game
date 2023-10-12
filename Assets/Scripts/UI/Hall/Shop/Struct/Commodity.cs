using System.Collections.Generic;

namespace Data
{
    public class Commodity
    {
        public uint identification;

        public List<Currency> currencies;

        public List<Prop> props;

        public Status status;

        public Cost cost;
    }
}