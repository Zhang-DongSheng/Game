using Game;
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

        public override void Set(string content)
        {
            base.Set(content);

            int count = m_list.Count;

            for (int i = 0; i < count; i++)
            {
                var prop = m_list[i].GetType<PropInformation>();

                prop.primary = m_list[i].GetUInt("ID");

                props.Add(prop);
            }
        }

        public override void Clear()
        {
            props = new List<PropInformation>();
        }
    }
    [System.Serializable]
    public class PropInformation : InformationBase
    {
        public string name;

        public string icon;

        public int quality;

        public int category;

        public int source;

        public float price;

        public string description;
    }
}