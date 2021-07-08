namespace UnityEngine.UI
{
    public abstract class UnregularLoopItem : MonoBehaviour
    {
        protected RectTransform target;

        public int Index { get; private set; }

        public object Source { get; private set; }

        public void Init()
        {
            if (TryGetComponent(out target))
            {
                target.anchorMin = new Vector2(0.5f, 1);

                target.anchorMax = new Vector2(0.5f, 1);

                target.pivot = new Vector2(0.5f, 1);
            }
        }

        public void Refresh(int index, object source)
        {
            Index = index;

            Source = source;

            Refresh();
        }

        protected virtual void Refresh()
        {
            Vector2 value = new Vector2(500, Index % 2 == 0 ? 100 : 200);

            Size = value;
        }

        public Vector2 Position 
        {
            get
            {
                return target.anchoredPosition;
            }
            set
            {
                target.anchoredPosition = value;
            }
        }

        public Vector2 Size
        {
            get
            {
                return new Vector2(target.rect.width, target.rect.height);
            }
            set
            {
                target.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, value.x);

                target.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, value.y);
            }
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