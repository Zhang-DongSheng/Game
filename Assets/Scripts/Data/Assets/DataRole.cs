using Game;
using System.Collections.Generic;

namespace Data
{
    public class DataRole : DataBase
    {
        public List<RoleInformation> roles = new List<RoleInformation>();

        public static RoleInformation Get(uint roleID)
        {
            var data = DataManager.Instance.Load<DataRole>();

            if (data != null)
            {
                return data.roles.Find(x => x.primary == roleID);
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