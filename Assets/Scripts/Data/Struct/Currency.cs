using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class Currency
    {
        public CurrencyEnum currency;

        public int number;

        public Currency(CurrencyEnum currency, int number)
        {
            this.currency = currency;

            this.number = number;
        }
    }
    /// <summary>
    /// ªı±“÷÷¿‡
    /// </summary>
    public enum CurrencyEnum
    {
        Gold = 101,
        Diamond = 102,
        Coin = 103,
    }
}