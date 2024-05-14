using Data;
using Game.State;
using Game.UI;
using UnityEngine;

namespace Game
{
    public class LoginLogic : Logic<LoginLogic>
    {
        public LoginInformation user { get; private set; }

        protected override void OnRegister()
        {
            NetworkEventManager.Register(NetworkEventKey.User, OnReceivedInformation);
        }

        protected override void OnUnregister()
        {
            NetworkEventManager.Unregister(NetworkEventKey.User, OnReceivedInformation);
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