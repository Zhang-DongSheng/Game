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

        [SerializeField] private RenewableCompontentType type;

        [SerializeField] private Transform parent;

        private Action<Object> create;

        private string resource;

        private Object asset;

        protected override DownloadFileType fileType { get { return DownloadFileType.None; } }

        public void CreateAsset(string key, string resource, string url = "", Action callBack = null, Action<Object> create = null)
        {
            if (string.IsNullOrEmpty(key)) return;

            this.current = key;

            this.key = string.Empty;

            this.resource = resource;

            this.create = create;

            if (RenewablePool.Instance.Exist(cache, key + resource, string.Empty))
            {
                this.asset = RenewablePool.Instance.Pop<Object>(cache, key + resource);

                this.key = key; Refresh(this.asset); callBack?.Invoke();

                if (!RenewablePool.Instance.Recent(cache, key + resource))
                {
                    this.key = string.Empty; Get(key, url, resource, callBack);
                }
            }
            else
            {
                Get(key, url, resource, callBack);
            }
        }

        public void CreateAssetImmediate(string key, string resource, string url = "", string local = "", Action callBack = null, Action<Object> create = null)
        {
            if (string.IsNullOrEmpty(key)) return;

            this.current = key;

            this.key = string.Empty;

            this.resource = resource;

            this.create = create;

            if (RenewablePool.Instance.Exist(cache, key + resource, string.Empty))
            {
                this.asset = RenewablePool.Instance.Pop<Object>(cache, key + resource);

                this.key = key; Refresh(this.asset); callBack?.Invoke();

                if (!RenewablePool.Instance.Recent(cache, key + resource))
                {
                    this.key = string.Empty; Get(key, url, resource, callBack);
                }
            }
            else
            {
                string path = Application.persistentDataPath + "/" + key;

                if (RenewableFile.Exists(path))
                {
                    byte[] buffer = RenewableFile.Read(path);

                    bool recent = RenewableResourceUpdate.Instance.Validation(key, buffer);

                    Create(new RenewableDownloadHandler(key, resource, string.Empty, recent, buffer, null));

                    if (recent)
                    {
                        this.key = key; RenewableResourceUpdate.Instance.Remove(key);
                    }
                    else
                    {
                        Get(key, url, resource, callBack);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(local))
                    {
                        path = local + resource;

                        if (TryLoad<Texture2D>(path, out Texture2D source))
                        {
                            bool recent = RenewableResourceUpdate.Instance.Validation(key, null);

                            Object _temp;

                            if (RenewablePool.Instance.Exist(cache, key + resource, string.Empty))
                            {
                                _temp = RenewablePool.Instance.Pop<Object>(cache, key + resource);
                            }
                            else
                            {
                                _temp = Instantiate(source);

                                RenewablePool.Instance.Push(cache, key + resource, string.Empty, recent, _temp);
                            }
                            Refresh(_temp);

                            Resources.UnloadAsset(source);

                            if (recent)
                            {
                                this.key = key; RenewableResourceUpdate.Instance.Remove(key);
                            }
                            else
                            {
                                Get(key, url, resource, callBack);
                            }
                        }
                        else
                        {
                            Get(key, url, resource, callBack);
                        }
                    }
                    else
                    {
                        Get(key, url, resource, callBack);
                    }
                }
            }
        }

        public bool Exist(string key, string resource, string local)
        {
            bool exist = false;

            if (RenewablePool.Instance.Exist(cache, key + resource, string.Empty))
            {
                exist = true;
            }
            else
            {
                string path = Application.persistentDataPath + "/" + key;

                if (RenewableFile.Exists(path))
                {
                    exist = true;
                }
                else
                {
                    path = local + resource;

                    if (TryLoad<Texture2D>(path, out Texture2D source))
                    {
                        exist = true; Resources.UnloadAsset(source);
                    }
                }
            }
            return exist;
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

            if (current == handle.key && resource == handle.parameter && _temp != null)
            {
                Refresh(_temp);
            }
        }

        private void Refresh(Object asset)
        {
            if (asset == null) return;

            switch (type)
            {
                case RenewableCompontentType.Image:
                    if (!TryGetComponent<RenewableImageCompontent>(out RenewableImageCompontent compontent))
                    {
                        compontent = gameObject.AddComponent<RenewableImageCompontent>();
                    }
                    compontent.SetTexture(asset);
                    break;
            }
            create?.Invoke(asset);
        }
    }
}