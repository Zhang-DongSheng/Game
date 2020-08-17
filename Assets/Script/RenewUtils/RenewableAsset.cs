using System;

namespace UnityEngine.UI
{
    public class RenewableAsset : RenewableBase
    {
        public Action<AssetBundle> callBack;

        [SerializeField] private Transform parent;

        private AssetBundle bundle;

        protected override DownloadFileType fileType { get { return DownloadFileType.None; } }

        public void CreateAsset(string key, string url = "", Action<AssetBundle> callBack = null)
        {
            this.current = key;

            this.callBack = callBack;

            if (this.key == key && bundle != null)
            {
                this.callBack?.Invoke(bundle); return;
            }
            this.key = string.Empty;

            //Release AssetBundle ..
            //if (this.bundle != null)
            //{
            //    this.bundle.Unload(true);
            //    this.bundle = null;
            //}

            Get(key, url);
        }

        protected override void Create(string key, byte[] buffer, Object content)
        {
            AssetBundle bundle = null;

            if (content != null)
            {
                bundle = content as AssetBundle;
            }
            else
            {
                try
                {
                    bundle = AssetBundle.LoadFromMemory(buffer);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            }

            if (current != key) return;

            this.bundle = bundle;

            this.callBack?.Invoke(this.bundle);
        }
    }
}