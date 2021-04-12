using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    public abstract class InfiniteItem : MonoBehaviour
    {
        public int Index { get; private set; }

        public object Source { get; private set; }

        public Vector2 Position { get; private set; }

        public Vector2 Size { get; private set; }

        public void Refresh(int index, object source)
        {
            Index = index;

            Source = source;
        }

        public abstract Vector2 Compute();

        public virtual void SetRect(Vector2 position, Vector2 size)
        {
            Position = position;

            Size = size;

            if (TryGetComponent(out RectTransform self))
            {
                self.anchoredPosition = position;

                self.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);

                self.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
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