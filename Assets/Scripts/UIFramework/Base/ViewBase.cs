using Game.Attribute;
using Game.Data;
using UnityEngine;

namespace Game.UI
{
    [DisallowMultipleComponent]
    public abstract class ViewBase : RuntimeBehaviour
    {
        [FieldName("层级")]
        public UILayer layer = UILayer.Window;
        [FieldName("类型")]
        public UIType type = UIType.Panel;
        [FieldName("序列")]
        public uint order = 0;
        [HideInInspector]
        public bool active;

        protected UIInformation information;

        public virtual void Init(UIInformation information)
        {
            this.information = information;
        }

        public virtual void Enter()
        {
            active = true;

            if (type == UIType.Panel)
            {
                UIQuickEntry.Open(UIPanel.Title, new UIParameter()
                {
                    ["panel"] = information.panel,
                });
            }
            SetActive(active);
        }

        public virtual bool Back()
        {
            if (gameObject.activeSelf)
            {
                UIManager.Instance.Close(information.panel);
            }
            return true;
        }

        public virtual void Exit()
        {
            active = false;

            SetActive(active);
        }

        public virtual void Reopen()
        {
        
        }

        public virtual void Refresh(UIParameter parameter)
        {
        
        }

        public virtual void Release()
        {
            if (gameObject != null)
            {
                GameObject.Destroy(gameObject);
            }
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
            UIManager.Instance.Close(information.panel);
        }

        protected virtual void Relevance()
        {
            Extension.Relevance(this);
        }
        [ContextMenu("Relevance")]
        protected void EditorRelevance()
        {
            Relevance();
        }
    }
}