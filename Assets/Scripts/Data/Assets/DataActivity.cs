using Game;
using System;
using System.Collections.Generic;

namespace Data
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

        public override void Load(string content)
        {
            base.Load(content);

            int count = m_list.Count;

            for (int i = 0; i < count; i++)
            {
                var activity = m_list[i].GetType<ActivityInformation>();

                activity.primary = m_list[i].GetUInt("ID");

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

        public bool timeLimit;

        public long beginTime;

        public long endTime;

        public string description;
    }
}