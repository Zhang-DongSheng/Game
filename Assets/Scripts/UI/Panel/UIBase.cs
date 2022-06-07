using UnityEngine;

namespace Game.UI
{
    [DisallowMultipleComponent]
    public abstract class UIBase : MonoBehaviour
    {
        protected UIPanel panel;

        public UILayer layer = UILayer.Base;

        public int order;

        public virtual void Init(UIPanel panel)
        {
            this.panel = panel;
        }

        public virtual void Reopen() { }

        public virtual void Refresh(Paramter paramter) { }

        public virtual void OnClickClose()
        {
            UIManager.Instance.Close(panel);
        }

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