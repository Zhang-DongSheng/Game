using Game;
using System.Collections.Generic;

namespace Game.Data
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