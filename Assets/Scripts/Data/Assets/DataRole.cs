using Game;
using System.Collections.Generic;

namespace Data
{
    public class DataRole : DataBase
    {
        public List<RoleInformation> list = new List<RoleInformation>();

        public static RoleInformation Get(uint roleID)
        {
            var data = DataManager.Instance.Load<DataRole>();

            if (data != null)
            {
                return data.list.Find(x => x.primary == roleID);
            }
            return null;
        }

        public override void Load(string content)
        {
            base.Load(content);

            int count = m_list.Count;

            for (int i = 0; i < count; i++)
            {
                var role = m_list[i].GetType<RoleInformation>();

                role.primary = m_list[i].GetUInt("ID");

                role.attributes = new List<IntPair>();

                var attributes = m_list[i].GetJson("attributes");

                for (int j = 0; j < attributes.Count; j++)
                {
                    role.attributes.Add(new IntPair()
                    {
                        x = int.Parse(attributes[j][0].ToString()),
                        y = int.Parse(attributes[j][1].ToString()),
                    });
                }
                list.Add(role);
            }
        }

        public override void Clear()
        {
            list = new List<RoleInformation>();
        }
    }
    [System.Serializable]
    public class RoleInformation : InformationBase
    {
        public string name;

        public uint age;

        public bool sex;

        public uint quality;

        public List<IntPair> attributes;

        public string path;

        public string description;
    }
}