using Game;
using System;
using System.Collections.Generic;

namespace Data
{
    public class DataActivity : DataBase
    {
        public List<ActivityInformation> activitys = new List<ActivityInformation>();

        public override void Set(string content)
        {
            base.Set(content);

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

    }
}