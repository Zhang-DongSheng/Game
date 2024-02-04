using Game;
using System.Collections.Generic;

namespace Data
{
    public class DataCommodity : DataBase
    {
        public List<CommodityInformation> commodities = new List<CommodityInformation>();

        public static CommodityInformation Get(uint commodityID)
        {
            var data = DataManager.Instance.Load<DataCommodity>();

            if (data != null)
            {
                return data.commodities.Find(x => x.primary == commodityID);
            }
            return null;
        }

        public override void Load(string content)
        {
            base.Load(content);

            int count = m_list.Count;

            for (int i = 0; i < count; i++)
            {
                var commodity = m_list[i].GetType<CommodityInformation>();

                commodity.primary = m_list[i].GetUInt("ID");

                commodity.rewards = new List<UIntPair>();

                var rewards = m_list[i].GetJson("rewards");

                for (int j = 0; j < rewards.Count; j++)
                {
                    commodity.rewards.Add(new UIntPair()
                    {
                        x = uint.Parse(rewards[j][0].ToString()),
                        y = uint.Parse(rewards[j][1].ToString()),
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

        public uint cost;

        public float price;

        public bool purchase;

        public List<UIntPair> rewards;

        public string description;
    }
}