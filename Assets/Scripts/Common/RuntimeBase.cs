using UnityEngine;

namespace Game
{
    public abstract class RuntimeBase : MonoBehaviour
    {
        protected virtual void OnEnable()
        {
            RuntimeBehaviour.Instance.RegisterUpdate(OnUpdate);

            RuntimeBehaviour.Instance.RegisterFixedUpdate(OnFixedUpdate);
        }

        protected virtual void OnDisable()
        {
            RuntimeBehaviour.Instance.UnregisterUpdate(OnUpdate);

            RuntimeBehaviour.Instance.UnregisterUpdate(OnFixedUpdate);
        }

        protected virtual void OnUpdate(float delta)
        {

        }

        protected virtual void OnFixedUpdate(float delta)
        {

        }

        protected virtual void SetActive(bool active)
        {
            if (gameObject != null && gameObject.activeSelf != active)
            {
                gameObject.SetActive(active);
            }
        }
    }
}