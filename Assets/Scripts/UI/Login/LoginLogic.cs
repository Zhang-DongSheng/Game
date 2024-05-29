using Data;
using Game.Network;
using Game.State;
using Game.UI;
using Protobuf;
using UnityEngine;

namespace Game
{
    public class LoginLogic : Logic<LoginLogic>
    {
        public LoginInformation user { get; private set; }

        #region Request
        public void RequestInformation(string account, string password)
        {
            var msg = new C2SLoginRequest()
            {
                Account = account,
                Password = password
            };
            NetworkManager.Instance.Send(NetworkMessageDefine.C2SLoginRequest, msg, OnReceivedInformation);
        }
        #endregion

        #region Receive
        private void OnReceivedInformation(object handle)
        {
            Debug.LogError(handle);

            EventArgs args = new EventArgs();

            args.AddOrReplace("status", true);

            EventDispatcher.Post(UIEvent.Login, args);

            ScheduleLogic.Instance.callback = () =>
            {
                GameStateController.Instance.EnterState<GameLobbyState>();
            };
            ScheduleLogic.Instance.Enter();
        }
        #endregion
    }
}