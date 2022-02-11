using System.Collections.Generic;

namespace Data
{
    public class DataCurrency : DataBase
    {
        public List<CurrencyInformation> currencies = new List<CurrencyInformation>();

        public CurrencyInformation Get(CurrencyType currency)
        {
            return currencies.Find(x => x.currency == currency);
        }
    }
    [System.Serializable]
    public class CurrencyInformation : InformationBase
    {
        public CurrencyType currency;

        public string name;

        public string icon;

        public string description;
    }

    public enum CurrencyType
    {
        Gold,
        Diamond,
        Coin,
    }
}