using Game.Network;
using Game.State;
using Game.UI;
using Protobuf;
using UnityEngine;

namespace Game.Logic
{
    public class LoginLogic : Singleton<LoginLogic>, ILogic
    {
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

                PlayerLogic.Instance.LoginSuccess();

                ScheduleLogic.Instance.callback = () =>
                {
                    GameStateController.Instance.EnterState<GameStateLobby>();
                };
                ScheduleLogic.Instance.Enter();
            });
        }
    }
}