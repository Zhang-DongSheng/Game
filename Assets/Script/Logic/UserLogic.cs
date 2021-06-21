using LitJson;
using UnityEngine;

namespace Game
{
    public class UserLogic : Singleton<UserLogic>
    {
        public void Init()
        { 
            
        }

        public void RequestLogin(JsonData json)
        {
            OnReceivedLogin();
        }

        private void OnReceivedLogin()
        {
            EventMessageArgs args = new EventMessageArgs();

            args.AddOrReplace("status", true);

            args.AddOrReplace("message", "");

            EventManager.Post(EventKey.Login, args);
        }
    }
}