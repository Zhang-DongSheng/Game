using UnityEngine;

namespace Game.UI
{
    [DisallowMultipleComponent]
    public abstract class UIBase : RuntimeBehaviour
    {
        protected UIPanel panel;

        public UILayer layer = UILayer.Base;

        public UIType type = UIType.Panel;

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