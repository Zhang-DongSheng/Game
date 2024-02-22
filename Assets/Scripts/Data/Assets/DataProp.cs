using Game;
using System.Collections.Generic;

namespace Data
{
    public class DataProp : DataBase
    {
        public List<PropInformation> list = new List<PropInformation>();

        public DataHelper helper = new DataHelper();

        public static PropInformation Get(uint propID, bool quick = true)
        {
            var data = DataManager.Instance.Load<DataProp>();

            if (data != null)
            {
                if (quick)
                {
                    return data.helper.Get(data.list, propID);
                }
                else
                {
                    return data.list.Find(x => x.primary == propID);
                }
            }
            return null;
        }

        public override void Load(string content)
        {
            base.Load(content);

            int count = m_list.Count;

            for (int i = 0; i < count; i++)
            {
                var prop = m_list[i].GetType<PropInformation>();

                prop.primary = m_list[i].GetUInt("ID");

                list.Add(prop);
            }
            helper.Divide(list);
        }

        public override void Clear()
        {
            list = new List<PropInformation>();
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