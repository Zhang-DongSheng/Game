using System;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(RenewableImageCompontent))]
    public class RenewableImage : RenewableBase
    {
        private Texture2D m_texture;

        protected override DownloadFileType fileType { get { return DownloadFileType.Image; } }

        public void SetImage(string key, string parameter = null, int order = 0, Action callBack = null)
        {
            if (string.IsNullOrEmpty(key)) return;

            this.current = key;

            this.key = string.Empty;

            if (RenewablePool.Instance.Exist(cache, key, string.Empty))
            {
                m_texture = RenewablePool.Instance.Pop<Texture2D>(cache, key);

                this.key = key; callBack?.Invoke();

                SetTexture(m_texture);

                if (!RenewablePool.Instance.Recent(cache, key))
                {
                    this.key = string.Empty; Get(key, parameter, order, callBack);
                }
            }
            else
            {
                Get(key, parameter, order, callBack);
            }
        }

        public void SetImageImmediate(string key, string parameter = null, int order = 0, Action callBack = null)
        {
            if (string.IsNullOrEmpty(key)) return;

            this.current = key;

            this.key = string.Empty;

            if (RenewablePool.Instance.Exist(cache, key, string.Empty))
            {
                m_texture = RenewablePool.Instance.Pop<Texture2D>(cache, key);

                this.key = key; callBack?.Invoke();

                SetTexture(m_texture);

                if (!RenewablePool.Instance.Recent(cache, key))
                {
                    this.key = string.Empty; Get(key, parameter, order, callBack);
                }
            }
            else
            {
                string path = string.Format("{0}/{1}", Application.persistentDataPath, key);

                if (RenewableFile.Exists(path))
                {
                    byte[] buffer = RenewableFile.Read(path);

                    bool recent = RenewableResourceUpdate.Instance.Validation(key, buffer);

                    Create(new RenewableDownloadHandler(key, string.Empty, string.Empty, recent, buffer, null));

                    if (recent)
                    {
                        this.key = key;

                        RenewableResourceUpdate.Instance.Remove(key);
                    }
                    else
                    {
                        Get(key, parameter, order, callBack);
                    }
                }
                else
                {
                    path = key;

                    if (TryLoad<Texture2D>(path, out Texture2D source))
                    {
                        bool recent = RenewableResourceUpdate.Instance.Validation(key, null);

                        Create(new RenewableDownloadHandler(key, string.Empty, string.Empty, recent, null, Instantiate(source)));

                        Resources.UnloadAsset(source);

                        if (recent)
                        {
                            this.key = key;

                            RenewableResourceUpdate.Instance.Remove(key);
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

        public bool Exist(string key, string resource)
        {
            bool exist = false;

            if (RenewablePool.Instance.Exist(cache, key, string.Empty))
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
                    path = string.Format("{0}/{1}", resource, key);

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
            Texture2D _texture;

            if (RenewablePool.Instance.Exist(cache, handle.key, handle.secret))
            {
                _texture = RenewablePool.Instance.Pop<Texture2D>(cache, handle.key);
            }
            else
            {
                if (handle.source != null)
                {
                    _texture = handle.Get<Texture2D>();
                }
                else
                {
                    _texture = new Texture2D(10, 10, TextureFormat.RGBA32, false);

                    _texture.LoadImage(handle.buffer);
                }
                RenewablePool.Instance.Push(cache, handle.key, handle.secret, handle.recent, _texture);
            }
            //Debug.LogErrorFormat("<color=green>[{0}]</color> ## <color=blue>[{1}]</color>", current, handle.key);

            if (this == null) return;

            if (current != handle.key) return;

            SetTexture(_texture);
        }

        private void SetTexture(Texture2D texture)
        {
            if (!TryGetComponent<RenewableImageCompontent>(out RenewableImageCompontent compontent))
            {
                compontent = gameObject.AddComponent<RenewableImageCompontent>();
            }
            compontent.SetTexture(texture);
        }
    }
}