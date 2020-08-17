using System;
using System.Text;
using TMPro;

namespace UnityEngine.UI
{
    public class RenewableText : RenewableBase
    {
        [SerializeField] private Text text;

        [SerializeField] private TextMeshProUGUI textMesh;

        protected override DownloadFileType fileType { get { return DownloadFileType.None; } }

        public void SetText(string key, string url = "", Action callBack = null)
        {
            current = key;

            Get(key, url, callBack);
        }

        protected override void Create(string key, byte[] buffer, Object content)
        {
            if (current != key) return;

            SetText(Encoding.Default.GetString(buffer));
        }

        private void SetText(string value)
        {
            if (text == null)
                text = GetComponentInChildren<Text>();
            if (text != null)
                text.text = value;

            if (textMesh == null)
                textMesh = GetComponentInChildren<TextMeshProUGUI>();
            if (textMesh != null)
                textMesh.text = value;
        }
    }
}