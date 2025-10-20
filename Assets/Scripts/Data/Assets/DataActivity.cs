using LitJson;
using System;
using System.Collections.Generic;

namespace Game.Data
{
    public class DataActivity : DataBase
    {
        public List<ActivityInformation> list;

        public static ActivityInformation Get(uint activityID)
        {
            var data = DataManager.Instance.Load<DataActivity>();

            if (data != null)
            {
                return data.list.Find(x => x.activityID == activityID);
            }
            return null;
        }

        public static List<ActivityInformation> List()
        {
            var data = DataManager.Instance.Load<DataActivity>();

            if (data != null)
            {
                return data.list;
            }
            return null;
        }

        public override void Load(JsonData json)
        {
            int count = json.Count;

            for (int i = 0; i < count; i++)
            {
                var activity = json[i].GetType<ActivityInformation>();

                activity.primary = json[i].GetUInt("ID");

                list.Add(activity);
            }
        }

        public override void Sort()
        {
            list.Sort(InformationBase.Compare);

            order = true;
        }

        public override void Clear()
        {
            list = new List<ActivityInformation>();
        }
    }
    [Serializable]
    public class ActivityInformation : InformationBase
    {
        public uint activityID;

        public string name;

        public long start;

        public long end;

        public int limit;

        public string description;
    }
}