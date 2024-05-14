using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    /// <summary>
    /// »∑»œµØ¥∞
    /// </summary>
    public class ConfirmView : ViewBase
    {
        [SerializeField] private RectTransform content;

        [SerializeField] private Text txt_title;

        [SerializeField] private Text txt_message;

        [SerializeField] private Button btn_confirm;

        [SerializeField] private Button btn_cancel;

        [SerializeField] private Button btn_close;

        private Action confirm, cancel;

        protected override void OnAwake()
        {
            btn_confirm.onClick.AddListener(OnClickConfirm);

            btn_cancel.onClick.AddListener(OnClickCancel);

            btn_close.onClick.AddListener(OnClickCancel);
        }

        public override void Refresh(UIParameter paramter)
        {
            if (paramter == null) return;

            confirm = paramter.Get<Action>("confirm");

            cancel = paramter.Get<Action>("cancel");

            txt_title.text = paramter.Get<string>("title");

            txt_message.text = paramter.Get<string>("message");

            float height = 320f;

            height += txt_message.preferredHeight;

            content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }

        private void OnClickConfirm()
        {
            confirm?.Invoke();

            OnClickClose();
        }

        private void OnClickCancel()
        {
            cancel?.Invoke();

            OnClickClose();
        }
    }
}