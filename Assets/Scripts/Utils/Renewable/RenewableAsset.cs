using System;
using UnityEngine.Renewable;

namespace UnityEngine
{
    public class RenewableAsset : RenewableBase
    {
        public RenewableComponent component;

        private string parameter;

        private Object asset;

        protected override DownloadFileType fileType { get { return DownloadFileType.None; } }

        public void CreateAsset(string key, string parameter, int order = 0, Action callBack = null)
        {
            if (string.IsNullOrEmpty(key)) return;

            this.current = key;

            this.key = string.Empty;

            this.parameter = parameter;

            if (RenewablePool.Instance.Exist(cache, key + parameter, string.Empty))
            {
                this.asset = RenewablePool.Instance.Pop<Object>(cache, key + parameter);

                this.key = key; Refresh(this.asset); callBack?.Invoke();

                if (!RenewablePool.Instance.Recent(cache, key + parameter))
                {
                    this.key = string.Empty; Get(key, parameter, order, callBack);
                }
            }
            else
            {
                Get(key, parameter, order, callBack);
            }
        }

        public void CreateAssetImmediate(string key, string parameter, string resource = null, int order = 0, Action callBack = null)
        {
            if (string.IsNullOrEmpty(key)) return;

            this.current = key;

            this.key = string.Empty;

            this.parameter = parameter;

            if (RenewablePool.Instance.Exist(cache, key + parameter, string.Empty))
            {
                this.asset = RenewablePool.Instance.Pop<Object>(cache, key + parameter);

                this.key = key; Refresh(this.asset); callBack?.Invoke();

                if (!RenewablePool.Instance.Recent(cache, key + parameter))
                {
                    this.key = string.Empty; Get(key, parameter, order, callBack);
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
                        Get(key, parameter, order, callBack);
                    }
                }
                else
                {
                    path = string.Format("{0}{1}", resource, parameter);

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
                            Get(key, parameter, order, callBack);
                        }
                    }
                    else
                    {
                        Get(key, parameter, order, callBack);
                    }
                }
            }
        }

        public bool Exist(string key, string parameter, string resource = null)
        {
            bool exist = false;

            if (RenewablePool.Instance.Exist(cache, key + parameter, string.Empty))
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
                    path = string.Format("{0}{1}", resource, parameter);

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
            if (RenewablePool.Instance.Exist(cache, handle.key + handle.parameter, handle.secret))
            {
                Object asset = RenewablePool.Instance.Pop<Object>(cache, handle.key + handle.parameter);

                if (current == handle.key && parameter == handle.parameter && asset != null)
                {
                    Refresh(asset);
                }
            }
            else
            {
                RenewableAssetBundle.Instance.LoadAsync(handle.key, handle.buffer, handle.parameter, (asset) =>
                {
                    if (asset != null)
                    {
                        if (current == handle.key && parameter == handle.parameter && asset != null)
                        {
                            Refresh(asset);
                        }
                        RenewablePool.Instance.Push(cache, handle.key + handle.parameter, handle.secret, handle.recent, asset);
                    }
                });
            }
        }

        private void Refresh(Object asset)
        {
            if (this == null || asset == null) return;

            if (component != null)
            {
                component.Refresh(asset);
            }
        }
    }
}