namespace UnityEngine.Renewable
{
    [DisallowMultipleComponent, ExecuteInEditMode]
    public abstract class RenewableComponent : MonoBehaviour
    {
        protected virtual void Awake()
        {
            if (TryGetComponent<RenewableAsset>(out RenewableAsset asset))
            {
                asset.component = this;
            }
        }

        public abstract void Refresh(Object source);
    }
}