using Game.Data;
using Game.Network;
using Protobuf;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Logic
{
    public class ActivityLogic : Singleton<ActivityLogic>, ILogic
    {
        private readonly List<ActivityData> _activities = new List<ActivityData>();

        public void Initialize()
        {

        }

        public bool IsOpen(uint activityID)
        {
            var activity = _activities.Find(x => x.activityID == activityID);

            if (activity == null) return false;

            if (activity.limited)
            {
                return TimeSynchronization.Instance.InSide(activity.start, activity.end);
            }
            return true;
        }

        public void RequestInformation()
        {
            var msg = new C2SActivityRequest();

            NetworkManager.Instance.Send(NetworkMessageDefine.C2SActivityRequest, msg, (handle) =>
            {
                _activities.Clear();

                var data = DataManager.Instance.Load<DataActivity>();

                int count = data.list.Count;

                Debuger.Log(Author.Data, "活动数量：" + count);

                for (int i = 0; i < count; i++)
                {
                    if (data.list[i].timeLimit)
                    {
                        if (TimeSynchronization.Instance.InSide(data.list[i].beginTime, data.list[i].endTime))
                        {
                            _activities.Add(new ActivityData(data.list[i]));
                        }
                    }
                    else
                    {
                        _activities.Add(new ActivityData(data.list[i]));
                    }
                }
                ScheduleLogic.Instance.Update(Schedule.Activity);
            });
        }
    }
}