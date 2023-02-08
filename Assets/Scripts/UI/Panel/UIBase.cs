using UnityEngine;

namespace Game.UI
{
    [DisallowMultipleComponent]
    public abstract class UIBase : RuntimeBehaviour
    {
        protected UIPanel panel;

        public UILayer layer = UILayer.Window;

        public UIType type = UIType.Panel;

        public int order = 0;

        public virtual void Init(UIPanel panel)
        {
            this.panel = panel;
        }

        public virtual void Enter()
        {
            SetActive(true);
        }

        public virtual void Exit()
        {
            SetActive(false);
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
            SetActive(this.gameObject, active);
        }

        public void SetActive(GameObject go, bool active)
        {
            if (go != null && go.activeSelf != active)
            {
                go.SetActive(active);
            }
        }

        public void SetActive(Component component, bool active)
        {
            if (component != null)
            {
                SetActive(component.gameObject, active);
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