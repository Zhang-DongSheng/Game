namespace UnityEngine.Renewable
{
    public class RenewablePrefabComponent : RenewableComponent
    {
        [SerializeField] private Transform parent;

        private GameObject target;

        public override void Refresh(Object source)
        {
            Create(source as GameObject);
        }

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
