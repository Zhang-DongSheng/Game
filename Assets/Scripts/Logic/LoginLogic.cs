using Data;
using Game.State;
using UnityEngine;

namespace Game
{
    public class LoginLogic : Singleton<LoginLogic>, ILogic
    {
        public User user { get; private set; }

        public void Init()
        {
            NetworkEventManager.Register(NetworkEventKey.User, OnReceivedInformation);
        }

        #region Request
        public void RequestInformation(string account, string password)
        {
            OnReceivedInformation(null);
        }
        #endregion

        #region Receive
        private void OnReceivedInformation(NetworkEventHandle handle)
        {
            EventMessageArgs args = new EventMessageArgs();

            args.AddOrReplace("status", true);

            EventManager.Post(EventKey.Login, args);

            ScheduleLogic.Instance.callback = () =>
            {
                GameStateController.Instance.EnterState<GameLobbyState>();
            };
            ScheduleLogic.Instance.Enter();
        }
        #endregion
    }
}