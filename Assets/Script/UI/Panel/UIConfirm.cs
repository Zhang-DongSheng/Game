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

        [SerializeField] private Button btn_background;

        private Action confirm, cancel;

        private void Awake()
        {
            btn_confirm.onClick.AddListener(OnClickConfirm);

            btn_cancel.onClick.AddListener(OnClickCancel);

            btn_background.onClick.AddListener(OnClickCancel);
        }

        public override void Refresh(params object[] paramter)
        {
            if (paramter == null) return;

            this.confirm = paramter[2] as Action;

            this.cancel = paramter[3] as Action;

            txt_title.text = paramter[0].ToString();

            txt_message.text = paramter[1].ToString();
        }

        private void OnClickConfirm()
        {
            confirm?.Invoke();

            UIManager.Instance.Close(UIKey.UIConfirm);
        }

        private void OnClickCancel()
        {
            cancel?.Invoke();

            UIManager.Instance.Close(UIKey.UIConfirm);
        }
    }
}