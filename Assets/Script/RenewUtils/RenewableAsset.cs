using System;

namespace UnityEngine.UI
{
    public class RenewableAsset : RenewableBase
    {
        public Action<Object> callBack;

        [SerializeField] private RenewableAssetType type;

        [SerializeField] private Transform parent;

        private string index;

        private Object asset;

        protected override DownloadFileType fileType { get { return DownloadFileType.None; } }

        public void CreateAsset(string key, string index, string url = "", Action<Object> callBack = null)
        {
            this.current = key;

            this.index = index;

            this.callBack = callBack;

            if (this.key == key && asset != null)
            {
                this.callBack?.Invoke(asset); return;
            }
            this.key = string.Empty;

            if (RenewablePool.Instance.Exist(cache, key + index, string.Empty))
            {
                this.key = key;

                this.asset = RenewablePool.Instance.Pop<Object>(cache, key + index);

                this.callBack?.Invoke(this.asset);
            }
            else
            {
                Get(key, url);
            }
        }

        protected override void Create(string key, byte[] buffer, Object content, string secret)
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
            buffer = null;

            Object _temp;

            if (RenewablePool.Instance.Exist(cache, key + index, secret))
            {
                _temp = RenewablePool.Instance.Pop<Object>(cache, key + index);

                bundle.Unload(true);
            }
            else
            {
                _temp = bundle.LoadAsset<Object>(index);

                bundle.Unload(false);

                RenewablePool.Instance.Push(cache, key + index, secret, _temp);
            }

            if (current == key)
            {
                this.asset = _temp;

                switch (type)
                {
                    case RenewableAssetType.Image:
                        RenewableImageCompontent compontent = GetComponent<RenewableImageCompontent>();
                        if (compontent == null)
                            compontent = gameObject.AddComponent<RenewableImageCompontent>();
                        compontent.SetTexture(this.asset);
                        break;
                }
                this.callBack?.Invoke(this.asset);
            }
            else
            {
                bundle.Unload(true);
            }
        }

        enum RenewableAssetType
        {
            None,
            Image,
            Text,
            Prefab,
        }
    }
}