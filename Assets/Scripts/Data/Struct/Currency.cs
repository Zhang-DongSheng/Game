using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    /// <summary>
    /// ªı±“÷÷¿‡
    /// </summary>
    public enum CurrencyType
    {
        Gold,
        Diamond,
        Coin,
    }
}