using System;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(RenewableImageCompontent))]
    public class RenewableImage : RenewableBase
    {
        private RenewableImageCompontent compontent;

        private Texture2D m_texture;

        protected override DownloadFileType fileType { get { return DownloadFileType.Image; } }

        public void SetImage(string key, string url = null, string parameter = null, Action callBack = null)
        {
            if (string.IsNullOrEmpty(key)) return;

            this.current = key;

            if (RenewablePool.Instance.Exist(cache, key, string.Empty))
            {
                m_texture = RenewablePool.Instance.Pop<Texture2D>(cache, key);

                this.key = key; callBack?.Invoke();

                SetTexture(m_texture);

                if (!RenewablePool.Instance.Recent(cache, key))
                {
                    this.key = string.Empty; Get(key, url, parameter, callBack);
                }
            }
            else
            {
                Get(key, url, parameter, callBack);
            }
        }

        public void SetImageSpecial(string key, string url = null, string parameter = null, Action callBack = null)
        {
            if (string.IsNullOrEmpty(key)) return;

            this.current = key;

            if (RenewablePool.Instance.Exist(cache, key, string.Empty))
            {
                m_texture = RenewablePool.Instance.Pop<Texture2D>(cache, key);

                this.key = key; callBack?.Invoke();

                SetTexture(m_texture);

                if (!RenewablePool.Instance.Recent(cache, key))
                {
                    this.key = string.Empty; Get(key, url, parameter, callBack);
                }
            }
            else
            {
                string path = string.Format("{0}/{1}", Application.persistentDataPath, key);

                if (RenewableFile.Exists(path))
                {
                    byte[] buffer = RenewableFile.Read(path);

                    bool recent = RenewableResourceUpdate.Instance.Validation(key, buffer);

                    this.key = recent ? key : string.Empty; callBack?.Invoke();

                    Create(new RenewableDownloadHandler(key, string.Empty, string.Empty, recent, buffer, null));

                    if (recent)
                    {
                        RenewableResourceUpdate.Instance.Remove(key);
                    }
                    else
                    {
                        Get(key, url, parameter, callBack);
                    }
                }
                else
                {
                    path = key;

                    Texture2D source = Resources.Load<Texture2D>(path);

                    if (source != null)
                    {
                        bool recent = RenewableResourceUpdate.Instance.Validation(key, null);

                        this.key = recent ? key : string.Empty; callBack?.Invoke();

                        Create(new RenewableDownloadHandler(key, string.Empty, string.Empty, recent, null, Instantiate(source)));

                        Resources.UnloadAsset(source);

                        if (recent)
                        {
                            RenewableResourceUpdate.Instance.Remove(key);
                        }
                        else
                        {
                            Get(key, url, parameter, callBack);
                        }
                    }
                    else
                    {
                        Get(key, url, parameter, callBack);
                    }
                }
            }
        }

        public bool Exist(string key)
        {
            bool exist;

            if (RenewablePool.Instance.Exist(cache, key, string.Empty))
            {
                exist = false;
            }
            else
            {
                string path = string.Format("{0}/{1}", Application.persistentDataPath, key);

                if (RenewableFile.Exists(path))
                {
                    exist = false;
                }
                else
                {
                    path = key;

                    Object asset = Resources.Load<Object>(path);

                    exist = asset != null;

                    if (exist)
                    {
                        Resources.UnloadAsset(asset);
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

            if (current != handle.key) return;

            SetTexture(_texture);
        }

        private void SetTexture(Texture2D texture)
        {
            if (compontent == null)
                compontent = GetComponent<RenewableImageCompontent>();
            compontent.SetTexture(texture);
        }
    }
}