using Game.Data;
using Game.Network;
using Game.State;
using Protobuf;
using UnityEngine;

namespace Game.UI
{
    public class LoginLogic : Singleton<LoginLogic>, ILogic
    {
        public LoginInformation user { get; private set; }

        public void Initialize()
        {

        }

        public void RequestInformation(string account, string password)
        {
            var msg = new C2SLoginRequest()
            {
                Account = account,
                Password = password
            };
            NetworkManager.Instance.Send(NetworkMessageDefine.C2SLoginRequest, msg, (handle) =>
            {
                EventArgs args = new EventArgs();

                args.AddOrReplace("status", true);

                EventDispatcher.Post(UIEvent.Login, args);

                ScheduleLogic.Instance.callback = () =>
                {
                    GameStateController.Instance.EnterState<GameLobbyState>();
                };
                ScheduleLogic.Instance.Enter();
            });
        }
    }
}