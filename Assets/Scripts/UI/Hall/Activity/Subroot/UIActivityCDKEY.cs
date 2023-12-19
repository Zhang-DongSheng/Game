using System.Text.RegularExpressions;
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

        private readonly Regex regex = new Regex(@"A-Z 0-9 6");

        private string content;

        protected override void OnAwake()
        {
            input.onSubmit.AddListener(OnSubmit);

            button.onClick.AddListener(OnClick);
        }

        private void OnSubmit(string value)
        {
            content = value;
        }

        private void OnClick()
        {
            string tips = string.Format("�һ��롾{0}��ʹ�óɹ���", content);

            UIQuickEntry.OpenUINotice(tips);
        }
    }
}
