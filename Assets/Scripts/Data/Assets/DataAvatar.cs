using LitJson;
using System;
using System.Collections.Generic;

namespace Game.Data
{
    public class DataAvatar : DataBase
    {
        public List<AvatarInformation> list;

        public static AvatarInformation Get(uint avatarID)
        {
            var data = DataManager.Instance.Load<DataAvatar>();

            if (data != null)
            {
                return data.list.Find(x => x.primary == avatarID);
            }
            return null;
        }

        public static List<AvatarInformation> GetList(uint type)
        {
            var data = DataManager.Instance.Load<DataAvatar>();

            if (data != null)
            {
                return data.list.FindAll(x => x.type == type);
            }
            return null;
        }

        public override void Load(JsonData json)
        {
            int count = json.Count;

            for (int i = 0; i < count; i++)
            {
                var info = json[i].GetType<AvatarInformation>();

                info.primary = json[i].GetUInt("ID");

                list.Add(info);
            }
        }

        public override void Clear()
        {
            list = new List<AvatarInformation>();
        }
    }
    [Serializable]
    public class AvatarInformation : InformationBase
    {
        public int type;

        public string name;

        public string icon;

        public int[] sources;

        public string description;
    }
}