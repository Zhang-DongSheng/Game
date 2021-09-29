using System.Collections.Generic;

namespace Data
{
    public class DataProp : DataBase
    {
        public List<PropInformation> props = new List<PropInformation>();

        public PropInformation Get(int identification, bool quick = false)
        {
            if (quick)
            {
                return QuickLook(props, identification);
            }
            else
            {
                return props.Find(x => x.identification == identification);
            }
        }
    }
    [System.Serializable]
    public class PropInformation : InformationBase
    {
        public string name;

        public string icon;

        public int source;

        public float price;

        public string description;
    }
}