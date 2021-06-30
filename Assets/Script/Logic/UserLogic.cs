using UnityEngine;

namespace Game
{
    public class UserLogic : Singleton<UserLogic>, ILogic
    {
        public void Init()
        {
            NetworkEventManager.Register(NetworkEventKey.User, OnReceivedInformation);
        }

        #region Request
        public void RequestInformation()
        {

        }
        #endregion

        #region Receive
        private void OnReceivedInformation(NetworkEventHandle handle)
        {
            EventMessageArgs args = new EventMessageArgs();

            args.AddOrReplace("status", true);

            args.AddOrReplace("message", "");

            EventManager.Post(EventKey.Login, args);

            ScheduleLogic.Instance.Enter();
        }
        #endregion
    }
}