namespace UnityEngine.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class UnregularScrollLayout : MonoBehaviour
    {
        public GameObject prefab;

        public Vector2 space = Vector2.zero;

        protected RectTransform rect;

        public void Initialize()
        {
            if (TryGetComponent(out rect))
            {
                Vector2 size = rect.rect.size;

                rect.anchorMin = new Vector2(0, 1);

                rect.anchorMax = new Vector2(0, 1);

                rect.pivot = new Vector2(0, 1);

                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);

                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
            }
        }

        public void SetPosition(Vector2 position)
        {
            rect.anchoredPosition = position;
        }

        public void SetSize(Vector2 size)
        {
            SetHorizontalSize(size.x); SetVerticalSize(size.y);
        }

        public void SetHorizontalSize(float value)
        {
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, value);
        }

        public void SetVerticalSize(float value)
        {
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, value);
        }

        public UnregularScrollItem Create()
        {
            GameObject go = GameObject.Instantiate(prefab, rect);

            if (go.TryGetComponent(out UnregularScrollItem item))
            {
                item.Init();
            }
            return item;
        }
    }
}