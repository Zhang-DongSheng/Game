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

        public void SetText(string key, string url = null, string parameter = null, Action callBack = null)
        {
            current = key;

            Get(key, url, parameter, callBack);
        }

        protected override void Create(RenewableDownloadHandler handle)
        {
            if (this == null) return;

            if (current != handle.key) return;

            SetText(Encoding.Default.GetString(handle.buffer));
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