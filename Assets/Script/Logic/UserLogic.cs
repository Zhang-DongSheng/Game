using LitJson;
using UnityEngine;

namespace Game
{
    public class UserLogic : Singleton<UserLogic>
    {
        public void RequestLogin(JsonData json)
        {
            string account = json.GetString("account");

            string password = json.GetString("password");

            if (account.Length == 3 && password.Length == 6)
            {
                OnReceivedLogin();
            }
        }

        private void OnReceivedLogin()
        {
            EventMessageArgs args = new EventMessageArgs();

            EventManager.PostEvent(EventKey.Login, args);
        }
    }
}