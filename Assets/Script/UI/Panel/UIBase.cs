using UnityEngine;

namespace Game.UI
{
    [DisallowMultipleComponent]
    public abstract class UIBase : MonoBehaviour
    {
        public UILayer layer = UILayer.Base;

        public int order;

        public virtual void Init() { }

        public virtual void Reopen()
        {
            SetActive(true);
        }

        public virtual void Refresh(Paramter paramter) { }

        public void SetName(string name)
        {
            transform.name = name;
        }

        public void SetParent(Transform parent)
        {
            if (transform.parent != parent)
            {
                transform.SetParent(parent);
            }
        }

        public void SetActive(bool active)
        {
            if (gameObject.activeSelf != active)
            {
                gameObject.SetActive(active);
            }
        }

        public void SetActive(Component component, bool active)
        {
            if (component != null && component.gameObject.activeSelf != active)
            {
                component.gameObject.SetActive(active);
            }
        }
    }
}