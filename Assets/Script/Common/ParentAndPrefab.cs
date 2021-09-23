namespace UnityEngine
{
    [System.Serializable]
    public class ParentAndPrefab
    {
        [SerializeField] private Transform parent;

        [SerializeField] private GameObject prefab;

        public GameObject Create()
        {
            return GameObject.Instantiate(prefab, parent);
        }

        public T Create<T>() where T : Component
        {
            GameObject go = GameObject.Instantiate(prefab, parent);

            if (go.TryGetComponent(out T component))
            {
                component = go.AddComponent<T>();
            }
            return component;
        }
    }
}