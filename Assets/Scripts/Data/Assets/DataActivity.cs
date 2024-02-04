using Game;
using System;
using System.Collections.Generic;

namespace Data
{
    public class DataActivity : DataBase
    {
        public List<ActivityInformation> activitys = new List<ActivityInformation>();

        public static ActivityInformation Get(uint activityID)
        {
            var data = DataManager.Instance.Load<DataActivity>();

            if (data != null)
            {
                return data.activitys.Find(x => x.primary == activityID);
            }
            return null;
        }

        public override void Load(string content)
        {
            base.Load(content);

            int count = m_list.Count;

            for (int i = 0; i < count; i++)
            {
                var activity = m_list[i].GetType<ActivityInformation>();

                activity.primary = m_list[i].GetUInt("ID");

                activitys.Add(activity);
            }
        }

        public override void Clear()
        {
            activitys = new List<ActivityInformation>();
        }
    }
    [Serializable]
    public class ActivityInformation : InformationBase
    {
        public string name;

        public uint type;

        public long beginTime;

        public long endTime;

        public bool timeLimit;

        public string description;
    }
}