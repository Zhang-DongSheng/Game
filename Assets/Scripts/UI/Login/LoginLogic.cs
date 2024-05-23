using Data;
using Game.Network;
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
            NetworkMessageManager.Instance.Register(NetworkMessageKey.User, OnReceivedInformation);
        }

        protected override void OnUnregister()
        {
            NetworkMessageManager.Instance.Unregister(NetworkMessageKey.User, OnReceivedInformation);
        }

        #region Request
        public void RequestInformation(string account, string password)
        {
            RawMessage message = new RawMessage();

            message.key = (int)NetworkMessageKey.User;

            message.content = account + "/" + password;

            string content = JsonUtility.ToJson(message);

            Network.NetworkManager.Instance.Send(content);
        }
        #endregion

        #region Receive
        private void OnReceivedInformation(object handle)
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