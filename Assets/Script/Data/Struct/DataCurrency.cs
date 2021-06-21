using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class DataCurrency : ScriptableObject
    {
        public List<CurrencyInformation> currencies = new List<CurrencyInformation>();

        public CurrencyInformation Get(CurrencyType currency)
        {
            return currencies.Find(x => x.currency == currency);
        }
    }
    [System.Serializable]
    public class CurrencyInformation
    {
        public CurrencyType currency;

        public string name;

        public string icon;

        public string description;
    }

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

    public enum CurrencyType
    {
        Gold,
        Diamond,
        Coin,
    }
}