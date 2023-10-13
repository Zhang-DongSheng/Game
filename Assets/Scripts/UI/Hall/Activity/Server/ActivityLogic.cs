using System.Collections.Generic;

namespace Game
{
    public class ActivityLogic : Singleton<ActivityLogic>, ILogic
    {
        public void Initialize()
        {
            NetworkEventManager.Register(NetworkEventKey.Activity, OnReceivedInformation);
        }

        public void Release()
        {
            NetworkEventManager.Unregister(NetworkEventKey.Activity, OnReceivedInformation);
        }

        public bool IsOpen(int activityID)
        {
            return true;
        }


        #region Request
        public void RequestInformation()
        {
            ScheduleLogic.Instance.Update(Schedule.Activity);
        }
        #endregion

        #region Receive
        private void OnReceivedInformation(NetworkEventHandle handle)
        {

        }
        #endregion


    }
}