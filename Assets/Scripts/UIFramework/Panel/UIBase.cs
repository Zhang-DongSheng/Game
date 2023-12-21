using UnityEngine;

namespace Game.UI
{
    [DisallowMultipleComponent]
    public abstract class UIBase : RuntimeBehaviour
    {
        protected int panel;

        public UILayer layer = UILayer.Window;

        public UIType type = UIType.Panel;

        public uint order = 0;

        public virtual void Init(int panel)
        {
            this.panel = panel;
        }

        public virtual void Enter()
        {
            if (type == UIType.Panel)
            {
                UIQuickEntry.Open(UIPanel.UITitle, new UIParameter()
                {
                    ["panel"] = panel,
                });
            }
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

        protected virtual void SetActive(bool active)
        {
            SetActive(this.gameObject, active);
        }

        protected virtual void SetActive(Component component, bool active)
        {
            if (component != null)
            {
                SetActive(component.gameObject, active);
            }
        }

        protected virtual void SetActive(GameObject go, bool active)
        {
            if (go != null && go.activeSelf != active)
            {
                go.SetActive(active);
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