using System;

namespace UnityEngine.UI
{
    public class RenewableAsset : RenewableBase
    {
        enum RenewableCompontentType
        {
            None,
            Image,
        }

        public Action<Object> callBack;

        [SerializeField] private RenewableCompontentType type;

        [SerializeField] private Transform parent;

        private string parameter;

        private Object asset;

        protected override DownloadFileType fileType { get { return DownloadFileType.None; } }

        public void CreateAsset(string key, string url = "", string parameter = "", Action<Object> callBack = null)
        {
            this.current = key;

            this.parameter = parameter;

            this.callBack = callBack;

            if (this.key == key && this.asset != null)
            {
                Refresh(this.asset); this.callBack?.Invoke(this.asset); return;
            }
            this.key = string.Empty;

            if (RenewablePool.Instance.Exist(cache, key + parameter, string.Empty))
            {
                this.asset = RenewablePool.Instance.Pop<Object>(cache, key + parameter);

                this.key = key; Refresh(this.asset); this.callBack?.Invoke(this.asset);

                if (!RenewablePool.Instance.Recent(cache, key + parameter))
                {
                    this.key = string.Empty; Get(key, url, parameter);
                }
            }
            else
            {
                Get(key, url, parameter);
            }
        }

        protected override void Create(RenewableDownloadHandler handle)
        {
            AssetBundle bundle = null;

            if (handle.source != null)
            {
                bundle = handle.source as AssetBundle;
            }
            else
            {
                try
                {
                    bundle = AssetBundle.LoadFromMemory(handle.buffer);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            }

            Object _temp = null;

            if (RenewablePool.Instance.Exist(cache, handle.key + handle.parameter, handle.secret))
            {
                _temp = RenewablePool.Instance.Pop<Object>(cache, handle.key + handle.parameter);

                bundle.Unload(true);
            }
            else
            {
                if (bundle.Contains(handle.parameter))
                {
                    _temp = bundle.LoadAsset<Object>(handle.parameter);

                    RenewablePool.Instance.Push(cache, handle.key + handle.parameter, handle.secret, handle.recent, _temp);
                }
                bundle.Unload(false);
            }

            if (current == handle.key && parameter == handle.parameter && _temp != null)
            {
                this.asset = _temp;

                Refresh(this.asset);

                this.callBack?.Invoke(this.asset);
            }
        }

        private void Refresh(Object asset)
        {
            switch (type)
            {
                case RenewableCompontentType.Image:
                    RenewableImageCompontent compontent = GetComponent<RenewableImageCompontent>();
                    if (compontent == null)
                        compontent = gameObject.AddComponent<RenewableImageCompontent>();
                    compontent.SetTexture(asset);
                    break;
            }
        }
    }
}