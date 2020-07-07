using System;
using System.Text;

namespace UnityEngine.UI
{
    public class RenewableText : RenewableBase
    {
        [SerializeField] private Text text;

        [SerializeField] private TMPro.TextMeshProUGUI textmesh;

        protected override DownloadFileType fileType { get { return DownloadFileType.None; } }

        public void SetText(string key, string url = "", string extra = "", Action callBack = null)
        {
            Get(key, url, extra, callBack);
        }

        protected override void Create(byte[] buffer, Object content)
        {
            SetText(Encoding.Default.GetString(buffer));
        }

        private void SetText(string value)
        {
            if (text == null)
                text = GetComponentInChildren<Text>();
            if (text != null)
                text.text = value;

            if (textmesh == null)
                textmesh = GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (textmesh != null)
                textmesh.text = value;
        }
    }
}