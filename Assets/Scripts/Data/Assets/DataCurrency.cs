using System.Collections.Generic;

namespace Data
{
    public class DataCurrency : DataBase
    {
        public List<PropInformation> currencies = new List<PropInformation>();

        public PropInformation Get(int identification)
        {
            return currencies.Find(x => x.identification == identification);
        }
    }
}