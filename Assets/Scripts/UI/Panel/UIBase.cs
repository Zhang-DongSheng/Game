using UnityEngine;

namespace Game.UI
{
    [DisallowMultipleComponent]
    public abstract class UIBase : MonoBehaviour
    {
        public UILayer layer = UILayer.Base;

        public int order;

        public virtual void Init() { }

        public virtual void Reopen() { }

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
            SetActive(gameObject, active);
        }

        public void SetActive(GameObject go, bool active)
        {
            if (go != null && go.activeSelf != active)
            {
                go.SetActive(active);
            }
        }

        protected virtual void Relevance()
        {
            Extension.Relevance(this);
        }
        [ContextMenu("Relevance")]
        protected void MenuRelevance()
        {
            Relevance();
        }
    }
}