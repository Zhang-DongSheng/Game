using Game;
using System.Collections.Generic;

namespace Data
{
    public class DataRole : DataBase
    {
        public List<RoleInformation> roles = new List<RoleInformation>();

        public RoleInformation Get(uint ID)
        {
            return roles.Find(x => x.primary == ID);
        }

        public override void Set(string content)
        {
            base.Set(content);

            int count = m_list.Count;

            for (int i = 0; i < count; i++)
            {
                var role = m_list[i].GetType<RoleInformation>();

                role.primary = m_list[i].GetUInt("ID");

                roles.Add(role);
            }
        }

        public override void Clear()
        {
            roles = new List<RoleInformation>();
        }
    }
    [System.Serializable]
    public class RoleInformation : InformationBase
    {
        public string name;

        public uint age;

        public bool sex;

        public uint quality;

        public int[] attributes;

        public string path;

        public string description;
    }
}