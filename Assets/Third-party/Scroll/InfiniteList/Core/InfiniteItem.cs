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

        public void Refresh(int index, object source)
        {
            Index = index;

            Source = source;
        }

        public abstract Vector2 Compute();

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
            set
            {
                self.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, value.x);

                self.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, value.y);
            }
        }

        public void SetActive(bool active)
        {
            if (gameObject != null && gameObject.activeSelf != active)
            {
                gameObject.SetActive(active);
            }
        }

        protected virtual void Refresh() { }
    }
}