using Data;
using Game.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ActivityLogic : Logic<ActivityLogic>
    {
        private readonly List<ActivityData> _activities = new List<ActivityData>();

        protected override void OnRegister()
        {
            NetworkEventManager.Register(NetworkEventKey.Activity, OnReceivedInformation);
        }

        protected override void OnUnregister()
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

            Debuger.Log(Author.Test, "活动数量" + count);

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
        }
        #endregion
    }
}