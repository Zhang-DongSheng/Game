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

        protected virtual void SetRotation(Vector3 rotation)
        {
            transform.localRotation = Quaternion.Euler(rotation);
        }

        protected virtual void SetScale(float scale)
        {
            transform.localScale = Vector3.one * scale;
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