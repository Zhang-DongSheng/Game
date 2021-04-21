namespace UnityEngine.Renewable
{
    public class RenewablePrefabComponent : RenewableComponent
    {
        [SerializeField] private Transform parent;

        private GameObject model;

        public override void Refresh(Object source)
        {
            Create(source as GameObject);
        }

        public void Create(GameObject prefab)
        {
            if (model != null)
            {
                GameObject.Destroy(model);
            }
            model = Instantiate(prefab, parent);
        }
    }
}