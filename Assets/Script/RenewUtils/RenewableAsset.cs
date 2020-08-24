using System;
using UnityEngine.UIElements;

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
            if (string.IsNullOrEmpty(key)) return;

            this.current = key;

            this.key = string.Empty;

            this.parameter = parameter;

            this.callBack = callBack;

            if (RenewablePool.Instance.Exist(cache, key + parameter, string.Empty))
            {
                this.asset = RenewablePool.Instance.Pop<Object>(cache, key + parameter);

                this.key = key; Refresh(this.asset);

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

        public void CreateAssetImmediate(string key, string url = "", string parameter = "", Action<Object> callBack = null)
        {
            if (string.IsNullOrEmpty(key)) return;

            this.current = key;

            this.key = string.Empty;

            this.parameter = parameter;

            this.callBack = callBack;

            if (RenewablePool.Instance.Exist(cache, key + parameter, string.Empty))
            {
                this.asset = RenewablePool.Instance.Pop<Object>(cache, key + parameter);

                this.key = key; Refresh(this.asset);

                if (!RenewablePool.Instance.Recent(cache, key + parameter))
                {
                    this.key = string.Empty; Get(key, url, parameter);
                }
            }
            else
            {
                string path = Application.persistentDataPath + "/" + key;

                if (RenewableFile.Exists(path))
                {
                    byte[] buffer = RenewableFile.Read(path);

                    bool recent = RenewableResourceUpdate.Instance.Validation(key, buffer);

                    Create(new RenewableDownloadHandler(key, parameter, string.Empty, recent, buffer, null));

                    if (recent)
                    {
                        this.key = key; RenewableResourceUpdate.Instance.Remove(key);
                    }
                    else
                    {
                        Get(key, url, parameter);
                    }
                }
                else
                {
                    if (true)
                    {
                        path = string.Format("Texture/{0}", parameter);

                        if (TryLoad<Texture2D>(path, out Texture2D source))
                        {
                            bool recent = RenewableResourceUpdate.Instance.Validation(key, null);

                            Object _temp;

                            if (RenewablePool.Instance.Exist(cache, key + parameter, string.Empty))
                            {
                                _temp = RenewablePool.Instance.Pop<Object>(cache, key + parameter);
                            }
                            else
                            {
                                _temp = Instantiate(source);

                                RenewablePool.Instance.Push(cache, key + parameter, string.Empty, recent, _temp);
                            }
                            Refresh(_temp);

                            Resources.UnloadAsset(source);

                            if (recent)
                            {
                                this.key = key; RenewableResourceUpdate.Instance.Remove(key);
                            }
                            else
                            {
                                Get(key, url, parameter);
                            }

                            Debug.LogError("??????");
                        }
                        else
                        {
                            Get(key, url, parameter);
                        }
                    }
                    else
                    {
                        Get(key, url, parameter);
                    }
                }
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
                Refresh(_temp);
            }
        }

        private void Refresh(Object asset)
        {
            if (asset == null) return;

            switch (type)
            {
                case RenewableCompontentType.Image:
                    RenewableImageCompontent compontent = GetComponent<RenewableImageCompontent>();
                    if (compontent == null)
                        compontent = gameObject.AddComponent<RenewableImageCompontent>();
                    compontent.SetTexture(asset);
                    break;
            }
            callBack?.Invoke(asset);
        }
    }
}