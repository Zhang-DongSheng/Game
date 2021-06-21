using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIConfirm : UIBase
    {
        [SerializeField] private RectTransform content;

        [SerializeField] private Text txt_title;

        [SerializeField] private Text txt_message;

        [SerializeField] private Button btn_confirm;

        [SerializeField] private Button btn_cancel;

        [SerializeField] private Button btn_close;

        private Action confirm, cancel;

        private void Awake()
        {
            btn_confirm.onClick.AddListener(OnClickConfirm);

            btn_cancel.onClick.AddListener(OnClickCancel);

            btn_close.onClick.AddListener(OnClickClose);
        }

        public override void Refresh(Paramter paramter)
        {
            if (paramter == null) return;

            txt_title.text = paramter.Get<string>("title");

            txt_message.text = paramter.Get<string>("message");

            confirm = paramter.Get<Action>("confirm");

            cancel = paramter.Get<Action>("cancel");
        }

        private void OnClickConfirm()
        {
            confirm?.Invoke();

            confirm = null;

            UIManager.Instance.Close(UIPanel.UIConfirm);
        }

        private void OnClickCancel()
        {
            cancel?.Invoke();

            cancel = null;

            UIManager.Instance.Close(UIPanel.UIConfirm);
        }

        private void OnClickClose()
        {
            OnClickCancel();
        }
    }
}