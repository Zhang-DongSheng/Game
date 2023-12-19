using Data;
using Game.UI;
using System.Collections.Generic;

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

            int count = data.activitys.Count;

            for (int i = 0; i < count; i++)
            {
                _activities.Add(new Activity()
                {

                });
            }
            ScheduleLogic.Instance.Update(Schedule.Activity);
        }
        #endregion


    }
}