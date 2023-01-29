using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public class CommodityInformation : InformationBase
    { 
        
    }
}
