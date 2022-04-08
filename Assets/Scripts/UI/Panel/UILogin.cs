using Data;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UILogin : UIBase
    {
        private const string ACCOUNT = "ACCOUNT";

        private const string PASSWORD = "PASSWORD";

        private readonly Regex regexAccount = new Regex(@"^[A-Za-z0-9]{3,15}$");

        private readonly Regex regexPassword = new Regex(@"^.*(?=.{6,15})(?=.*\d)(?=.*[A-Z])(?=.*[a-z])(?=.*[!@#$%^&*? ]).*$");

        [SerializeField] private InputField input_account;

        [SerializeField] private InputField input_password;

        [SerializeField] private Button btn_login;

        private string account, password;

        private void Awake()
        {
            input_account.onValueChanged.AddListener(OnValueChangedAccount);

            input_password.onValueChanged.AddListener(OnValueChangedPassword);

            btn_login.onClick.AddListener(OnClickLogin);
        }

        private void OnEnable()
        {
            EventManager.Register(EventKey.Login, OnReceivedLogin);
        }

        private void OnDisable()
        {
            EventManager.Unregister(EventKey.Login, OnReceivedLogin);
        }

        private void Start()
        {
            input_account.text = GlobalVariables.Get<string>(ACCOUNT);

            input_password.text = GlobalVariables.Get<string>(PASSWORD);
        }

        private void OnReceivedLogin(EventMessageArgs args)
        {
            bool status = args.Get<bool>("status");

            if (status)
            {
                GlobalVariables.Set(ACCOUNT, account);

                GlobalVariables.Set(PASSWORD, password);

                UIManager.Instance.Open(UIPanel.UIMain, record: false);

                UIManager.Instance.Close(UIPanel.UILogin);
            }
            else
            {
                string error = args.Get<string>("message");

                UIQuickEntry.OpenUIHorseLamp(error);
            }
        }

        private void OnValueChangedAccount(string value)
        {
            input_account.textComponent.color = Color.red;

            if (regexAccount.IsMatch(value))
            {
                input_account.textComponent.color = Color.black;

                account = value;
            }
        }

        private void OnValueChangedPassword(string value)
        {
            input_password.textComponent.color = Color.red;

            if (regexPassword.IsMatch(value))
            {
                input_password.textComponent.color = Color.black;

                password = value;
            }
        }

        private void OnClickLogin()
        {
            //UserLogic.Instance.RequestInformation();

            UIManager.Instance.Close(UIPanel.UILogin);

            UIManager.Instance.Open(UIPanel.UIMain);
        }
    }
}