using Game;
using System.Collections.Generic;

namespace Game.Data
{
    public class DataRole : DataBase
    {
        public List<RoleInformation> list = new List<RoleInformation>();

        public static RoleInformation Get(uint roleID)
        {
            var data = DataManager.Instance.Load<DataRole>();

            if (data != null)
            {
                return DataManager.Get(data.list, roleID, data.order);
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

                role.attributes = new List<Pair<int, float>>();

                var attributes = m_list[i].GetJson("attributes");

                for (int j = 0; j < attributes.Count; j++)
                {
                    role.attributes.Add(new Pair<int, float>()
                    {
                        x = (int)attributes[j][0],
                        y = (float)attributes[j][1],
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

        public List<Pair<int, float>> attributes;

        public string path;

        public string description;
    }
}