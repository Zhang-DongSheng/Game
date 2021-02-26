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

        private readonly Regex regexAccount = new Regex("");

        private readonly Regex regexPassword = new Regex("");

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
            if (regexAccount.IsMatch(value))
            {
                account = value;
            }
        }

        private void OnValueChangedPassword(string value)
        {
            if (regexPassword.IsMatch(value))
            {
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