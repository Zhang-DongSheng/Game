using Game;
using LitJson;
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

        public override void Load(JsonData json)
        {
            int count = json.Count;

            for (int i = 0; i < count; i++)
            {
                var role = json[i].GetType<RoleInformation>();

                role.primary = json[i].GetUInt("ID");

                role.attributes = new List<Pair<int, float>>();

                var attributes = json[i].GetJson("attributes");

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