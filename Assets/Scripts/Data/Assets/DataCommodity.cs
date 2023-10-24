using System.Collections.Generic;

namespace Data
{
    public class DataCommodity : DataBase
    {
        public List<CommodityInformation> commodities = new List<CommodityInformation>();

        public CommodityInformation Get(int ID)
        {
            return commodities.Find(x => x.primary == ID);
        }

        public override void Set(string content)
        {
            base.Set(content);
        }

        public override void Clear()
        {

        }
    }
    [System.Serializable]
    public class CommodityInformation : InformationBase
    { 
        
    }
}
