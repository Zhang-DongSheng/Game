using UnityEngine;

namespace Game.UI
{
    public abstract class ItemBase : MonoBehaviour
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

        protected virtual void SetSize(RectTransform rect, float width = -1f, float height = -1f)
        {
            if (rect == null) return;

            if (width != -1f)
            {
                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            }
            if (height != -1f)
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
            if (gameObject != null && gameObject.gameObject.activeSelf != active)
            {
                gameObject.gameObject.SetActive(active);
            }
        }

        protected virtual void SetActive(Component component, bool active)
        {
            if (component != null && component.gameObject.activeSelf != active)
            {
                component.gameObject.SetActive(active);
            }
        }

        public void Destroy()
        {
            if (gameObject != null)
            {
                GameObject.Destroy(gameObject);
            }
        }
    }
}