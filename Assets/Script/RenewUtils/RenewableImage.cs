using System;

namespace UnityEngine.UI
{
    public class RenewableImage : RenewableBase
    {
        [SerializeField] private RenewableImageType imageType;

        [SerializeField] private Image image;

        [SerializeField] private RawImage rawImage;

        [SerializeField] private SpriteRenderer render;

        [SerializeField] private bool nativeSize = false;

        private Texture2D m_texture;

        private Sprite m_sprite;

        protected override DownloadFileType fileType { get { return DownloadFileType.Image; } }

        public void SetImage(string key, string url = "", Action callBack = null)
        {
            if (string.IsNullOrEmpty(key)) return;

            this.current = key;

            if (RenewablePool.Instance.Exist(cache, key))
            {
                m_texture = RenewablePool.Instance.Pop<Texture2D>(cache, key);

                this.key = key; callBack?.Invoke();

                SetImage(m_texture);
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

            if (RenewablePool.Instance.Exist(cache, key))
            {
                m_texture = RenewablePool.Instance.Pop<Texture2D>(cache, key);

                this.key = key; callBack?.Invoke();

                SetImage(m_texture);
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

                        Create(key, buffer, null);
                    }
                    else
                    {
                        callBack?.Invoke();

                        Create(key, buffer, null);

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

                        Create(key, null, Instantiate(source));

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

            if (RenewablePool.Instance.Exist(cache, key))
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

        protected override void Create(string key, byte[] buffer, Object content)
        {
            Texture2D _texture;

            if (content != null)
            {
                _texture = content as Texture2D;
            }
            else
            {
                _texture = new Texture2D(10, 10, TextureFormat.RGBA32, false);

                _texture.LoadImage(buffer);
            }
            RenewablePool.Instance.Push(cache, key, _texture);

            //Debug.LogErrorFormat("<color=green>[{0}]</color> ## <color=blue>[{1}]</color>", current, key);

            if (current != key) return;

            SetImage(_texture);
        }

        private void SetImage(Texture2D texture)
        {
            switch (imageType)
            {
                case RenewableImageType.RawImage:
                    if (rawImage != null)
                        rawImage.texture = texture;
                    break;
                default:
                    if (m_sprite != null) { Destroy(m_sprite); }
                    m_sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                    SetImage(m_sprite);
                    break;
            }
        }

        private void SetImage(Sprite sprite)
        {
            if (image == null)
                image = GetComponentInChildren<Image>();
            if (image != null)
                image.sprite = sprite;

            if (render == null)
                render = GetComponentInChildren<SpriteRenderer>();
            if (render != null)
                render.sprite = sprite;

            if (image == null && nativeSize)
                image.SetNativeSize();
        }

        public void SetColor(Color color)
        {
            if (image == null)
                image = GetComponentInChildren<Image>();
            if (image != null)
                image.color = color;

            if (render == null)
                render = GetComponentInChildren<SpriteRenderer>();
            if (render != null)
                render.color = color;
        }

        private enum RenewableImageType
        {
            Image,
            RawImage,
            SpriteRenderer,
        }
    }
}