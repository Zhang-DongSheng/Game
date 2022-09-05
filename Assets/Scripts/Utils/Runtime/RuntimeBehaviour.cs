using UnityEngine;

namespace Game
{
    public abstract class RuntimeBehaviour : MonoBehaviour
    {
        protected virtual void OnEnable()
        {
            RuntimeManager.Instance.Register(RuntimeEvent.Update, OnUpdate);
        }

        protected virtual void OnDisable()
        {
            RuntimeManager.Instance.Unregister(RuntimeEvent.Update, OnUpdate);
        }

        protected virtual void OnUpdate(float delta)
        {

        }

        protected virtual void OnFixedUpdate(float delta)
        {

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