using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class DataLotteryDraw : ScriptableObject
    {
        public List<LotteryInformation> lotteries = new List<LotteryInformation>();
    }

    public class LotteryInformation
    {
        public string key;

        public float weight;

        public string description;

        public List<PropInformation> props;
    }
}