using LitJson;
using UnityEngine;

namespace Game
{
    public class UserLogic : Singleton<UserLogic>
    {
        public void RequestLogin(JsonData json)
        {
            OnReceivedLogin();
        }

        private void OnReceivedLogin()
        {
            EventMessageArgs args = new EventMessageArgs();

            args.AddOrReplaceMessage("status", true);

            args.AddOrReplaceMessage("message", "");

            EventManager.PostEvent(EventKey.Login, args);
        }
    }
}