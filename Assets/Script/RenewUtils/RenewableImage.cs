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

        public void SetImage(string key, string url = "", string extra = "", Action callBack = null)
        {
            if (string.IsNullOrEmpty(key)) return;

            string[] param = key.Split('|');

            key = param.Length > 0 ? param[0] : string.Empty;

            string md5 = param.Length > 1 ? param[1] : string.Empty;

            switch (imageType)
            {
                case RenewableImageType.RawImage:
                    if (RenewablePool.Instance.Exist("Texture:" + key))
                    {
                        m_texture = RenewablePool.Instance.Pop<Texture2D>("Texture:" + key);
                        this.key = key; SetRawImage(m_texture); callBack?.Invoke();
                    }
                    else
                    {
                        Get(key, url, extra, md5, callBack);
                    }
                    break;
                default:
                    if (RenewablePool.Instance.Exist("Sprite:" + key))
                    {
                        m_sprite = RenewablePool.Instance.Pop<Sprite>("Sprite:" + key);
                        this.key = key; SetImage(m_sprite); callBack?.Invoke();
                    }
                    else
                    {
                        Get(key, url, extra, md5, callBack);
                    }
                    break;
            }
        }

        protected override void Create(byte[] buffer, Object content)
        {
            if (RenewablePool.Instance.Exist("Texture:" + this.key))
            {
                m_texture = RenewablePool.Instance.Pop<Texture2D>("Texture:" + this.key);
            }
            else
            {
                if (content != null)
                {
                    m_texture = content as Texture2D;
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
                }
                RenewablePool.Instance.Push("Texture:" + this.key, m_texture);
            }
            SetImage(m_texture);
        }

        private void SetImage(Texture2D texture)
        {
            switch (imageType)
            {
                case RenewableImageType.RawImage:
                    SetRawImage(texture);
                    break;
                default:
                    m_sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                    RenewablePool.Instance.Push("Sprite:" + this.key, m_sprite);
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

        private void SetRawImage(Texture2D texture)
        {
            if (rawImage != null)
            {
                rawImage.texture = texture;
            }
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