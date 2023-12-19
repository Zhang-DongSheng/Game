using Data;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemMail : ItemBase
    {
        [SerializeField] private Text title;

        [SerializeField] private GameObject m_reddot;

        [SerializeField] private GameObject m_select;

        [SerializeField] private Button button;

        private Action<Mail> _callback;

        private Mail _mail;

        protected override void OnRegister()
        {
            button.onClick.AddListener(OnClick);
        }

        public void Refresh(Mail mail, uint select, Action<Mail> callback)
        {
            _mail = mail;

            _callback = callback;

            title.text = mail.content;

            SetActive(m_select, mail.ID == select);
        }

        private void OnClick()
        {
            _callback?.Invoke(_mail);
        }
    }
}