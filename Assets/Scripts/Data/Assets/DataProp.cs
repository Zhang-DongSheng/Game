using System.Collections.Generic;

namespace Data
{
    public class DataProp : DataBase
    {
        public List<PropInformation> props = new List<PropInformation>();

        public PropInformation Get(int key, bool quick = false)
        {
            return props.Find(x => x.primary == key);
        }

        public PropInformation Get(uint key, bool quick = false)
        {
            return props.Find(x => x.primary == key);
        }
    }
    [System.Serializable]
    public class PropInformation : InformationBase
    {
        public string name;

        public string icon;

        public Quality quality;

        public int category;

        public float price;

        public int source;

        public string description;
    }
}