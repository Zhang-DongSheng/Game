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

        public virtual void Refresh(UIParameter parameter) { }

        public virtual bool Back()
        {
            if (isActiveAndEnabled)
            {
                UIManager.Instance.Close(panel);
            }
            return true;
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
            if (gameObject != null && gameObject.activeSelf != active)
            {
                gameObject.SetActive(active);
            }
        }

        protected virtual void OnClickClose()
        {
            UIManager.Instance.Close(panel);
        }

        protected virtual void Relevance()
        {
            Extension.Relevance(this);
        }
        [ContextMenu("Relevance")]
        protected void RelevanceMenu()
        {
            Relevance();
        }
    }
}