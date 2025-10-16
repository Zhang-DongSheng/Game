using Game.Attribute;
using UnityEngine;

namespace Game.UI
{
    public abstract class SubviewBase : RuntimeBehaviour
    {
        [FieldName("Ò³ÃæID")]
        public int subviewID;
        [FieldName("Ò³Ãæ×´Ì¬")]
        public bool active = true;

        public virtual void Initialise()
        {
            
        }

        public virtual void Refresh()
        {

        }

        public virtual void Switch(int ID)
        {
            var active = subviewID == ID;

            SetActive(active);
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