using System;
using System.Text;
using UnityEngine.Renewable;
using UnityEngine.Renewable.Compontent;

namespace UnityEngine
{
    public class RenewableText : RenewableBase
    {
        protected override DownloadFileType fileType { get { return DownloadFileType.None; } }

        public void SetText(string key, int order = 0, Action callBack = null)
        {
            current = key;

            Get(key, null, order, callBack);
        }

        protected override void Create(RenewableDownloadHandler handle)
        {
            if (this == null) return;

            if (current != handle.key) return;

            SetText(Encoding.Default.GetString(handle.buffer));
        }

        private void SetText(string value)
        {
            if (!TryGetComponent<RenewableTextComponent>(out RenewableTextComponent compontent))
            {
                compontent = gameObject.AddComponent<RenewableTextComponent>();
            }
            compontent.SetContent(value);
        }
    }
}