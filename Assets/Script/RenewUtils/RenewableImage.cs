using System;

namespace UnityEngine.UI
{
    public class RenewableImage : RenewableBase
    {
        [SerializeField] private RenewableImageType imageType;

        [SerializeField] private Image image;

        [SerializeField] private RawImage rawImage;

        [SerializeField] private SpriteRenderer render;

        [SerializeField] private bool highQuality = false;

        [SerializeField] private bool nativeSize = false;

        private Texture2D m_texture;

        private Sprite m_sprite;

        protected override DownloadFileType fileType { get { return DownloadFileType.Image; } }

        public void SetImage(string key, string url = "", string extra = "", Action callBack = null)
        {
            if (string.IsNullOrEmpty(key)) return;

            if (RenewablePool.Instance.Exist(cache, key))
            {
                m_texture = RenewablePool.Instance.Pop<Texture2D>(cache, key);

                this.key = key; callBack?.Invoke();

                SetImage(m_texture);
            }
            else
            {
                Get(key, url, extra, callBack);
            }
        }

        protected override void Create(byte[] buffer, Object content)
        {
            if (RenewablePool.Instance.Exist(cache, this.key))
            {
                m_texture = RenewablePool.Instance.Pop<Texture2D>(cache, this.key);
            }
            else
            {
                if (content != null)
                {
                    m_texture = content as Texture2D;
                    m_texture.Compress(highQuality);
                }
                else
                {
                    string[] param = this.key.Split('.');

                    string suffix = param.Length > 1 ? param[param.Length - 1] : string.Empty;

                    switch (suffix.ToLower())
                    {
                        case "jpg":
                        case "jpeg":
                            m_texture = new Texture2D(10, 10, TextureFormat.PVRTC_RGB4, true);
                            break;
                        case "png":
                            m_texture = new Texture2D(10, 10, TextureFormat.PVRTC_RGBA4, true);
                            break;
                        default:
                            m_texture = new Texture2D(10, 10, TextureFormat.PVRTC_RGBA4, true);
                            break;
                    }
                    m_texture.LoadImage(buffer);
                    m_texture.Compress(highQuality);
                }
                RenewablePool.Instance.Push(cache, this.key, m_texture);
            }
            SetImage(m_texture);
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
                    if (m_sprite != null)
                    {
                        Destroy(m_sprite);
                    }
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