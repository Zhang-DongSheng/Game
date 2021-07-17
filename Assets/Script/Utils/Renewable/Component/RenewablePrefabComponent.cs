namespace UnityEngine.Renewable
{
    public class RenewablePrefabComponent : RenewableComponent
    {
        [SerializeField] private Transform parent;

        private GameObject model;

        protected override void Awake()
        {
            if (parent == null)
            {
                parent = transform;
            }
            base.Awake();
        }

        public override void Refresh(Object source)
        {
            Create(source as GameObject);
        }

        private void Create(GameObject prefab)
        {
            if (model != null)
            {
                GameObject.Destroy(model);
            }
            model = Instantiate(prefab, parent);
        }
    }
}