using UnityEngine.UI;

namespace UnityEngine.Renewable
{
    public class RenewableImageComponent : RenewableComponent
    {
        enum RenewableImageType
        {
            Image,
            RawImage,
            SpriteRenderer,
        }

        [SerializeField] private RenewableImageType type;

        [SerializeField] private Image image;

        [SerializeField] private RawImage rawImage;

        [SerializeField] private SpriteRenderer render;

        [SerializeField] private Vector4 border;

        [SerializeField] private bool nativeSize = false;

        private Sprite m_sprite;

        public override void Refresh(Object source)
        {
            if (source != null && source is Texture2D)
            {
                SetTexture(source as Texture2D);
            }
            else
            {
                Debug.LogWarningFormat("The source Type is unknow!");
            }
        }

        public void SetTexture(Texture2D texture)
        {
            if (texture == null) return;

            switch (type)
            {
                case RenewableImageType.Image:
                    if (m_sprite != null) { Destroy(m_sprite); }
                    m_sprite = Create(texture);
                    SetImage(m_sprite);
                    break;
                case RenewableImageType.RawImage:
                    SetRawImage(texture);
                    break;
                case RenewableImageType.SpriteRenderer:
                    if (m_sprite != null) { Destroy(m_sprite); }
                    m_sprite = Create(texture);
                    SetSpriteRenderer(m_sprite);
                    break;
            }
        }

        public void SetTexture(Sprite sprite)
        {
            switch (type)
            {
                case RenewableImageType.Image:
                    SetImage(sprite);
                    break;
                case RenewableImageType.RawImage:
                    SetRawImage(sprite.texture);
                    break;
                case RenewableImageType.SpriteRenderer:
                    SetSpriteRenderer(sprite);
                    break;
            }
        }

        private void SetImage(Sprite sprite)
        {
            if (image == null)
                image = GetComponentInChildren<Image>();
            if (image != null)
                image.sprite = sprite;

            if (image != null && nativeSize)
                image.SetNativeSize();
        }

        private void SetRawImage(Texture2D texture)
        {
            if (rawImage == null)
                rawImage = GetComponent<RawImage>();
            if (rawImage != null)
                rawImage.texture = texture;
        }

        private void SetSpriteRenderer(Sprite sprite)
        {
            if (render == null)
                render = GetComponentInChildren<SpriteRenderer>();
            if (render != null)
                render.sprite = sprite;
        }

        public void SetColor(Color color)
        {
            if (image == null)
                image = GetComponent<Image>();
            if (image != null)
                image.color = color;

            if (rawImage == null)
                rawImage = GetComponent<RawImage>();
            if (rawImage != null)
                rawImage.color = color;

            if (render == null)
                render = GetComponent<SpriteRenderer>();
            if (render != null)
                render.color = color;
        }

        private Sprite Create(Texture2D texture)
        {
            if (border != Vector4.zero)
            {
                return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f, 100f, 0, SpriteMeshType.FullRect, border);
            }
            else
            {
                return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            }
        }

        private void OnDestroy()
        {
            if (m_sprite != null) { Destroy(m_sprite); }
        }
    }
}