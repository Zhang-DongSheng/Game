using System;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(RenewableImageCompontent))]
    public class RenewableImage : RenewableBase
    {
        private RenewableImageCompontent compontent;

        private Texture2D m_texture;

        protected override DownloadFileType fileType { get { return DownloadFileType.Image; } }

        public void SetImage(string key, string url = "", Action callBack = null)
        {
            if (string.IsNullOrEmpty(key)) return;

            this.current = key;

            if (compontent == null)
                compontent = GetComponent<RenewableImageCompontent>();

            if (RenewablePool.Instance.Exist(cache, key, string.Empty))
            {
                m_texture = RenewablePool.Instance.Pop<Texture2D>(cache, key);

                this.key = key; callBack?.Invoke();

                compontent.SetTexture(m_texture);
            }
            else
            {
                Get(key, url, callBack);
            }
        }

        public void SetImageSpecial(string key, string url = "", Action callBack = null)
        {
            if (string.IsNullOrEmpty(key)) return;

            this.current = key;

            if (RenewablePool.Instance.Exist(cache, key, string.Empty))
            {
                m_texture = RenewablePool.Instance.Pop<Texture2D>(cache, key);

                this.key = key; callBack?.Invoke();

                compontent.SetTexture(m_texture);
            }
            else
            {
                string path = string.Format("{0}/{1}", Application.persistentDataPath, key);

                if (RenewableFile.Exists(path))
                {
                    byte[] buffer = RenewableFile.Read(path);

                    if (RenewableResourceUpdate.Instance.Validation(key, buffer))
                    {
                        this.key = key; callBack?.Invoke();

                        Create(key, buffer, null, string.Empty);
                    }
                    else
                    {
                        callBack?.Invoke();

                        Create(key, buffer, null, string.Empty);

                        Get(key, url, callBack);
                    }
                }
                else
                {
                    path = key;

                    Texture2D source = Resources.Load<Texture2D>(path);

                    if (source != null)
                    {
                        if (RenewableResourceUpdate.Instance.Validation(key, null))
                        {
                            this.key = key;
                        }
                        else
                        {
                            Get(key, url, callBack);
                        }

                        callBack?.Invoke();

                        Create(key, null, Instantiate(source), string.Empty);

                        Resources.UnloadAsset(source);
                    }
                    else
                    {
                        Get(key, url, callBack);
                    }
                }
            }
        }

        public bool Exist(string key)
        {
            bool mask = true;

            if (RenewablePool.Instance.Exist(cache, key, string.Empty))
            {
                mask = false;
            }
            else
            {
                string path = string.Format("{0}/{1}", Application.persistentDataPath, key);

                if (RenewableFile.Exists(path))
                {
                    mask = false;
                }
                else
                {
                    path = key;

                    mask = Resources.Load<Texture2D>(path) == null;
                }
            }

            return mask;
        }

        protected override void Create(string key, byte[] buffer, Object content, string secret)
        {
            Texture2D _texture;

            if (RenewablePool.Instance.Exist(cache, key, secret))
            {
                _texture = RenewablePool.Instance.Pop<Texture2D>(cache, key);
            }
            else
            {
                if (content != null)
                {
                    _texture = content as Texture2D;
                }
                else
                {
                    _texture = new Texture2D(10, 10, TextureFormat.RGBA32, false);

                    _texture.LoadImage(buffer);
                }
                RenewablePool.Instance.Push(cache, key, secret, _texture);
            }

            //Debug.LogErrorFormat("<color=green>[{0}]</color> ## <color=blue>[{1}]</color>", current, key);

            if (current != key) return;

            compontent.SetTexture(_texture);
        }
    }
}