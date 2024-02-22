using Data;
using Game.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ActivityLogic : Singleton<ActivityLogic>, ILogic
    {
        private readonly List<Activity> _activities = new List<Activity>();

        public void Initialize()
        {
            NetworkEventManager.Register(NetworkEventKey.Activity, OnReceivedInformation);
        }

        public void Release()
        {
            NetworkEventManager.Unregister(NetworkEventKey.Activity, OnReceivedInformation);
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

        #region Request
        public void RequestInformation()
        {
            OnReceivedInformation(null);
        }
        #endregion

        #region Receive
        private void OnReceivedInformation(NetworkEventHandle handle)
        {
            _activities.Clear();

            var data = DataManager.Instance.Load<DataActivity>();

            int count = data.list.Count;

            Debuger.Log(Author.Test, "�����" + count);

            for (int i = 0; i < count; i++)
            {
                if (data.list[i].timeLimit)
                {
                    if (TimeSynchronization.Instance.InSide(data.list[i].beginTime, data.list[i].endTime))
                    {
                        _activities.Add(new Activity(data.list[i]));
                    }
                }
                else
                {
                    _activities.Add(new Activity(data.list[i]));
                }
            }
            ScheduleLogic.Instance.Update(Schedule.Activity);
        }
        #endregion
    }
}