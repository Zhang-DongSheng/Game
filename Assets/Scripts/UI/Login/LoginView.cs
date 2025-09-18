using Game.Const;
using Game.Data;
using Game.Logic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class LoginView : ViewBase
    {
        private readonly Regex regexAccount = new Regex(@"^[A-Za-z0-9]{3,15}$");

        private readonly Regex regexPassword = new Regex(@"^.*(?=.{6,15})(?=.*\d)(?=.*[A-Z])(?=.*[a-z])(?=.*[!@#$%^&*? ]).*$");

        [SerializeField] private InputField input_account;

        [SerializeField] private InputField input_password;

        [SerializeField] private Toggle tog_remember;

        [SerializeField] private Toggle tog_automatic;

        [SerializeField] private Button btn_login;

        private string account, password;

        private bool remember, automatic;

        protected override void OnAwake()
        {
            input_account.onValueChanged.AddListener(OnValueChangedAccount);

            input_password.onValueChanged.AddListener(OnValueChangedPassword);

            tog_remember.onValueChanged.AddListener(OnValueChangedRemember);

            tog_automatic.onValueChanged.AddListener(OnValueChangedAutomatic);

            btn_login.onClick.AddListener(OnClickLogin);
        }

        protected override void OnRegister()
        {
            EventDispatcher.Register(UIEvent.Login, OnReceivedLogin);
        }

        protected override void OnUnregister()
        {
            EventDispatcher.Unregister(UIEvent.Login, OnReceivedLogin);
        }

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            remember = GlobalVariables.Get<bool>(GlobalKey.LOGIN_PASSWORD_REMEMBER);

            automatic = GlobalVariables.Get<bool>(GlobalKey.LOGIN_AUTOMATIC);

            account = GlobalVariables.Get<string>(GlobalKey.LOGIN_ACCOUNT);

            password = GlobalVariables.Get<string>(GlobalKey.LOGIN_PASSWORD);

            input_account.text = account;

            input_password.text = password;

            tog_remember.isOn = remember;

            tog_automatic.isOn = automatic;
        }

        private void OnReceivedLogin(EventArgs args)
        {
            bool success = args.Get<bool>("status");

            if (success)
            {
                GlobalVariables.Set(GlobalKey.LOGIN_ACCOUNT, account);

                if (remember)
                {
                    GlobalVariables.Set(GlobalKey.LOGIN_PASSWORD, password);
                }
            }
            else
            {
                string error = args.Get<string>("message");

                UIQuickEntry.OpenNoticeView(error);
            }
        }

        private void OnValueChangedAccount(string value)
        {
            input_account.textComponent.color = Color.red;

            if (regexAccount.IsMatch(value))
            {
                input_account.textComponent.color = Color.black;
            }
            account = value;
        }

        private void OnValueChangedPassword(string value)
        {
            input_password.textComponent.color = Color.red;

            if (regexPassword.IsMatch(value))
            {
                input_password.textComponent.color = Color.black;
            }
            password = value;
        }

        private void OnValueChangedRemember(bool isOn)
        {
            remember = isOn;

            if (!remember)
            {
                GlobalVariables.Set(GlobalKey.LOGIN_PASSWORD, string.Empty);

                if (automatic)
                {
                    tog_automatic.isOn = false;
                }
            }
            GlobalVariables.Set(GlobalKey.LOGIN_PASSWORD_REMEMBER, remember);
        }

        private void OnValueChangedAutomatic(bool isOn)
        {
            automatic = isOn;

            GlobalVariables.Set(GlobalKey.LOGIN_AUTOMATIC, automatic);
        }

        private void OnClickLogin()
        {
            if (string.IsNullOrEmpty(account))
            {
                UIQuickEntry.OpenNoticeView("账号不能为空");
            }
            else if (string.IsNullOrEmpty(password))
            {
                UIQuickEntry.OpenNoticeView("密码不能为空");
            }
            else
            {

                TextManager.Instance.Translate("正在登录...", (result) =>
                {
                    UIQuickEntry.OpenHorseLampView(result);
                });

                //LoginLogic.Instance.RequestInformation(account, password);
            }
        }
    }
}