namespace UnityEngine.Renewable.Compontent
{
    public class RenewablePrefabCompontent : MonoBehaviour
    {
        [SerializeField] private Transform parent;

        public void Create(GameObject prefab)
        {
            if (prefab == null) return;

            if (parent == null) parent = transform;

            for (int i = 0; i < parent.childCount; i++)
            {
                Destroy(parent.GetChild(0));
            }
            Instantiate(prefab, parent);
        }
    }
}
