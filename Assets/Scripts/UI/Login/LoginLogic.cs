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

        protected override void OnRegister()
        {
            NetworkMessageManager.Instance.Register(NetworkMessageDefine.C2SLoginRequest, OnReceivedInformation);
        }

        protected override void OnUnregister()
        {
            NetworkMessageManager.Instance.Unregister(NetworkMessageDefine.C2SLoginRequest, OnReceivedInformation);
        }

        #region Request
        public void RequestInformation(string account, string password)
        {
            var msg = new C2SLoginRequest()
            {
                Account = account,
                Password = password
            };
            Network.NetworkManager.Instance.Send(NetworkMessageDefine.C2SLoginRequest, msg);
        }
        #endregion

        #region Receive
        private void OnReceivedInformation(object handle)
        {
            Debug.LogError(handle);

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