using Data;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemMail : ItemBase
    {
        public Action<Mail> callback;

        [SerializeField] private Text text;

        [SerializeField] private Button button;

        private Mail m_mail;

        protected override void OnRegister()
        {
            button.onClick.AddListener(OnClick);
        }

        public void Refresh(Mail mail)
        {
            m_mail = mail;

            text.text = mail.content;

            SetActive(true);
        }

        private void OnClick()
        {
            callback?.Invoke(m_mail);
        }
    }
}