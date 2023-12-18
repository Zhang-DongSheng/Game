using Game;
using System.Collections.Generic;

namespace Data
{
    public class DataCommodity : DataBase
    {
        public List<CommodityInformation> commodities = new List<CommodityInformation>();

        public CommodityInformation Get(uint ID)
        {
            return commodities.Find(x => x.primary == ID);
        }

        public override void Set(string content)
        {
            base.Set(content);

            int count = m_list.Count;

            for (int i = 0; i < count; i++)
            {
                var commodity = m_list[i].GetType<CommodityInformation>();

                commodity.primary = m_list[i].GetUInt("ID");

                commodity.rewards = new List<RewardInformation>();

                var rewards = m_list[i].GetJson("rewards");

                for (int j = 0; j < rewards.Count; j++)
                {
                    commodity.rewards.Add(new RewardInformation()
                    {
                        propID = uint.Parse(rewards[j][0].ToString()),
                        amount = int.Parse(rewards[j][1].ToString()),
                    });
                }
                commodities.Add(commodity);
            }
        }

        public override void Clear()
        {
            commodities = new List<CommodityInformation>();
        }
    }
    [System.Serializable]
    public class CommodityInformation : InformationBase
    {
        public string name;

        public int cost;

        public int price;

        public bool purchase;

        public List<RewardInformation> rewards;

        public string description;
    }
}