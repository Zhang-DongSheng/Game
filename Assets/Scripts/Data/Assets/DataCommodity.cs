using Game;
using System.Collections.Generic;

namespace Data
{
    public class DataCommodity : DataBase
    {
        public List<CommodityInformation> list = new List<CommodityInformation>();

        public static CommodityInformation Get(uint commodityID)
        {
            var data = DataManager.Instance.Load<DataCommodity>();

            if (data != null)
            {
                return DataManager.Get(data.list, commodityID, data.order);
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

                var cost = m_list[i].GetJson("cost");

                commodity.cost = new UIntPair()
                {
                    x = uint.Parse(cost[0].ToString()),
                    y = uint.Parse(cost[1].ToString()),
                };
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
                list.Add(commodity);
            }
        }

        public override void Sort()
        {
            list.Sort(InformationBase.Compare);

            order = true;
        }

        public override void Clear()
        {
            list = new List<CommodityInformation>();
        }
    }
    [System.Serializable]
    public class CommodityInformation : InformationBase
    {
        public string name;

        public UIntPair cost;

        public int number;

        public List<UIntPair> rewards;

        public string description;
    }
}