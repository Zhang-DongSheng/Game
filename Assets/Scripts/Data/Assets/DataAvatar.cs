using LitJson;
using System;
using System.Collections.Generic;

namespace Game.Data
{
    public class DataAvatar : DataBase
    {
        public List<AvatarInformation> list;

        public AvatarInformation Get(uint avatarID)
        {
            return list.Find(x => x.primary == avatarID);
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
        public uint type;

        public string name;

        public string icon;

        public uint[] sources;

        public string description;
    }
}