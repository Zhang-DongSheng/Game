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
            NetworkMessageManager.Instance.Register(NetworkMessageKey.User, OnReceivedInformation);
        }

        protected override void OnUnregister()
        {
            NetworkMessageManager.Instance.Unregister(NetworkMessageKey.User, OnReceivedInformation);
        }

        #region Request
        public void RequestInformation(string account, string password)
        {
            RawMessage raw = new RawMessage();

            raw.key = (int)NetworkMessageKey.User;

            raw.message = new C2SLoginRequest()
            {
                Account = account,
                Password = password
            };
            raw.content = ProtoBufUtils.Serialize(raw.message);

            string content = JsonUtility.ToJson(raw);

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