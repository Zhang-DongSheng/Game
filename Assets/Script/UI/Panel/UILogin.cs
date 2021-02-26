using LitJson;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UILogin : UIBase
    {
        [SerializeField] private InputField input_account;

        [SerializeField] private InputField input_password;

        [SerializeField] private Button btn_login;

        private readonly Regex regexAccount = new Regex(@"^[A-Za-z0-9]{3,15}$");

        private readonly Regex regexPassword = new Regex(@"^.*(?=.{6,15})(?=.*\d)(?=.*[A-Z])(?=.*[a-z])(?=.*[!@#$%^&*? ]).*$");

        private string account, password;

        private void Awake()
        {
            input_account.onValueChanged.AddListener(OnValueChangedAccount);

            input_password.onValueChanged.AddListener(OnValueChangedPassword);

            btn_login.onClick.AddListener(OnClickLogin);
        }

        private void OnEnable()
        {
            EventManager.RegisterEvent(EventKey.Login, OnReceivedLogin);
        }

        private void OnDisable()
        {
            EventManager.UnregisterEvent(EventKey.Login, OnReceivedLogin);
        }

        private void Start()
        {
            input_account.text = LocalManager.GetString("ACCOUNT");

            input_password.text = LocalManager.GetString("PASSWORD");
        }

        private void OnReceivedLogin(EventMessageArgs args)
        {
            UIManager.Instance.Open(UIKey.UIMain);
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
            UserLogic.Instance.RequestLogin(new JsonData()
            {
                ["account"] = account,
                ["password"] = password,
            });
        }
    }
}