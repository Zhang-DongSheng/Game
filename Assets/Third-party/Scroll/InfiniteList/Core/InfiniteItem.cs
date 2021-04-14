using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    public abstract class InfiniteItem : MonoBehaviour
    {
        protected RectTransform self;

        public int Index { get; private set; }

        public object Source { get; private set; }

        public void Init()
        {
            self = GetComponent<RectTransform>();

            self.anchorMin = new Vector2(0.5f, 1);

            self.anchorMax = new Vector2(0.5f, 1);

            self.pivot = new Vector2(0.5f, 1);
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

            self.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, value.x);

            self.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, value.y);
        }

        public Vector2 Position 
        {
            get
            {
                return self.anchoredPosition;
            }
            set
            {
                self.anchoredPosition = value;
            }
        }

        public Vector2 Size
        {
            get
            {
                return new Vector2(self.rect.width, self.rect.height);
            }
        }

        internal bool Exist(Vector2 point)
        {
            if (point.y < Position.y && point.y > Position.y - Size.y)
            {
                return true;
            }
            return false;
        }

        public void SetActive(bool active)
        {
            if (gameObject != null && gameObject.activeSelf != active)
            {
                gameObject.SetActive(active);
            }
        }
    }
}