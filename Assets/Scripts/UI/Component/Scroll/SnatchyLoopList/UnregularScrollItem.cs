namespace UnityEngine.UI
{
    public abstract class UnregularScrollItem : MonoBehaviour
    {
        protected GameObject self;

        protected RectTransform rect;

        public int Index { get; private set; }

        public object Source { get; private set; }

        public virtual void Init()
        {
            if (TryGetComponent(out rect))
            {
                rect.anchorMin = new Vector2(0, 1);

                rect.anchorMax = new Vector2(0, 1);

                rect.pivot = new Vector2(0, 1);
            }
            Index = -1;
        }

        public virtual void Refresh(int index, object source)
        {
            Index = index;

            Source = source;

            Refresh();

            SetActive(true);
        }

        protected abstract void Refresh();

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

        public void SetActive(bool active)
        {
            if (gameObject != null && gameObject.activeSelf != active)
            {
                gameObject.SetActive(active);
            }
        }

        public void Destroy()
        {
            if (this.gameObject != null)
            {
                GameObject.Destroy(this.gameObject);
            }
        }
    }
}