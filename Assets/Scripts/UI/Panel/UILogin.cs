using Data;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UILogin : UIBase
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

        private void Awake()
        {
            Initialize();

            input_account.onValueChanged.AddListener(OnValueChangedAccount);

            input_password.onValueChanged.AddListener(OnValueChangedPassword);

            tog_remember.onValueChanged.AddListener(OnValueChangedRemember);

            tog_automatic.onValueChanged.AddListener(OnValueChangedAutomatic);

            btn_login.onClick.AddListener(OnClickLogin);
        }

        protected override void OnEnable()
        {
            EventManager.Register(EventKey.Login, OnReceivedLogin);
        }

        protected override void OnDisable()
        {
            EventManager.Unregister(EventKey.Login, OnReceivedLogin);
        }

        private void Initialize()
        {
            remember = GlobalVariables.Get<bool>(Const.REMEMBERPASSWORD);

            automatic = GlobalVariables.Get<bool>(Const.AUTOMATICLOGIN);

            account = GlobalVariables.Get<string>(Const.ACCOUNT);

            password = GlobalVariables.Get<string>(Const.PASSWORD);

            input_account.text = account;

            input_password.text = password;

            tog_remember.isOn = remember;

            tog_automatic.isOn = automatic;
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
                GlobalVariables.Set(Const.PASSWORD, string.Empty);

                if (automatic)
                {
                    tog_automatic.isOn = false;
                }
            }
            GlobalVariables.Set(Const.REMEMBERPASSWORD, remember);
        }

        private void OnValueChangedAutomatic(bool isOn)
        {
            automatic = isOn;

            GlobalVariables.Set(Const.AUTOMATICLOGIN, automatic);
        }

        private void OnClickLogin()
        {
            if (string.IsNullOrEmpty(account))
            {
                UIQuickEntry.OpenUITips("账号不能为空");
            }
            else if (string.IsNullOrEmpty(password))
            {
                UIQuickEntry.OpenUITips("密码不能为空");
            }
            else
            {
                LoginLogic.Instance.RequestInformation(account, password);
            }
        }

        private void OnReceivedLogin(EventMessageArgs args)
        {
            bool success = args.Get<bool>("status");

            if (success)
            {
                GlobalVariables.Set(Const.ACCOUNT, account);

                if (remember)
                {
                    GlobalVariables.Set(Const.PASSWORD, password);
                }
                UILoading.Instance.Open();

                ScheduleLogic.Instance.callback = () =>
                {
                    UIQuickEntry.OpenSingle(UIPanel.UIMain);
                };
                ScheduleLogic.Instance.Enter();
            }
            else
            {
                string error = args.Get<string>("message");

                UIQuickEntry.OpenUIHorseLamp(error);
            }
        }
    }
}