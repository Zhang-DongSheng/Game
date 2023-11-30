namespace UnityEngine.UI
{
    /// <summary>
    /// 碰到了内存崩溃的问题不太确定是那块导致的
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class ImageBackground : MonoBehaviour
    {
        [SerializeField] private Suit suit;

        private Image background;

        private RectTransform target;

        private void Awake()
        {
            target = GetComponent<RectTransform>();

            background = GetComponent<Image>();

            background.RegisterDirtyLayoutCallback(Execute);

            Execute();
        }

        private void OnValidate()
        {
            Execute();
        }

        private void OnDestroy() 
        {
            background.UnregisterDirtyLayoutCallback(Execute);
        }

        private void Execute()
        {
            if (background == null || background.sprite == null) return;

            float width = background.sprite.rect.width;

            float height = background.sprite.rect.height;

            background.SetNativeSize();

            switch (suit)
            {
                case Suit.None:
                    break;
                case Suit.Image:
                    {
                        target.anchorMin = new Vector2(0.5f, 0.5f);

                        target.anchorMax = new Vector2(0.5f, 0.5f);

                        target.pivot = new Vector2(0.5f, 0.5f);

                        target.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);

                        target.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);

                        target.anchoredPosition = new Vector2(0, 0);
                    }
                    break;
                case Suit.Full:
                    {
                        target.anchorMin = new Vector2(0, 0);

                        target.anchorMax = new Vector2(1, 1);

                        target.pivot = new Vector2(0.5f, 0.5f);

                        target.sizeDelta = new Vector2(0, 0);

                        target.anchoredPosition = new Vector2(0, 0);
                    }
                    break;
                case Suit.Scene:
                    {
                        Vector2 space = GetComponentInParent<Canvas>().GetComponent<RectTransform>().rect.size;

                        float x = space.x / width;

                        float y = space.y / height;

                        target.anchorMin = new Vector2(0.5f, 0.5f);

                        target.anchorMax = new Vector2(0.5f, 0.5f);

                        target.pivot = new Vector2(0.5f, 0.5f);

                        if (x > y)
                        {
                            target.sizeDelta = new Vector2(width * x, height * x);
                        }
                        else
                        {
                            target.sizeDelta = new Vector2(width * y, height * y);
                        }
                        target.anchoredPosition = new Vector2(0, 0);
                    }
                    break;
            }
        }

        enum Suit
        { 
            None,
            Full,
            Image,
            Scene,
        }
    }
}