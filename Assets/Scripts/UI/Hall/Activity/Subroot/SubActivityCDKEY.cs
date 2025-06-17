using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    /// <summary>
    /// 兑换码
    /// </summary>
    public class SubActivityCDKEY : SubActivityBase
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

            string tips = string.Format("兑换码<color=blue><b>{0}</b></color>使用成功！", content);

            UIQuickEntry.OpenNoticeView(tips);
        }
    }
}
