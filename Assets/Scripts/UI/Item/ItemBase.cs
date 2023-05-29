using Game.UI;
using UnityEngine;

namespace Game
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

        protected virtual void SetSize(RectTransform rect, float width = UIConfig.MinusOne, float height = UIConfig.MinusOne)
        {
            if (rect == null) return;

            if (width != UIConfig.MinusOne)
            {
                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            }
            // if the value is default. don't modify!
            if (height != UIConfig.MinusOne)
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

        protected virtual void ShowOrHide(bool state)
        {
            if (TryGetComponent(out CanvasGroup canvas))
            {
                canvas.alpha = state ? 1 : 0;

                canvas.interactable = state;

                canvas.blocksRaycasts = state;
            }
        }
    }
}