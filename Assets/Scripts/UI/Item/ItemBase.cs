using UnityEngine;

namespace Game.UI
{
    public abstract class ItemBase : RuntimeBehaviour
    {
        protected virtual void SetPosition(Vector3 position)
        {
            if (transform is RectTransform rect)
            {
                rect.anchoredPosition = position;
            }
            else
            {
                transform.localPosition = position;
            }
        }

        protected virtual void SetSize(RectTransform rect, float width = Config.MinusOne, float height = Config.MinusOne)
        {
            if (rect == null) return;

            if (width != Config.MinusOne)
            {
                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            }
            // if the value is default. don't modify!
            if (height != Config.MinusOne)
            {
                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            }
        }

        protected virtual void SetOrder(int index)
        {
            transform.SetSiblingIndex(index);
        }

        protected virtual void SetActive(bool active)
        {
            SetActive(this.gameObject, active);
        }

        protected virtual void SetActive(GameObject go, bool active)
        {
            if (go != null && go.activeSelf != active)
            {
                go.SetActive(active);
            }
        }

        protected virtual void SetActive(Component component, bool active)
        {
            if (component != null)
            {
                SetActive(component.gameObject, active);
            }
        }
    }
}