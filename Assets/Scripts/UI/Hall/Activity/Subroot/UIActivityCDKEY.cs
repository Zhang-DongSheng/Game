using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    /// <summary>
    /// �һ���
    /// </summary>
    public class UIActivityCDKEY : UIActivityBase
    {
        [SerializeField] private InputField input;

        [SerializeField] private Button button;

        private string content;

        protected override void OnAwake()
        {
            input.onValueChanged.AddListener(OnValueChanged);

            input.onSubmit.AddListener(OnSubmit);

            button.onClick.AddListener(OnClick);
        }

        private void OnValueChanged(string value)
        {
            content = value;
        }

        private void OnSubmit(string value)
        {
            content = value;
        }

        private void OnClick()
        {
            if (string.IsNullOrEmpty(content)) return;

            string tips = string.Format("�һ���<color=blue><b>{0}</b></color>ʹ�óɹ���", content);

            UIQuickEntry.OpenUINotice(tips);
        }
    }
}
