namespace UnityEngine
{
    /// <summary>
    /// 预制体实例组件
    /// </summary>
    [DisallowMultipleComponent]
    public class PrefabTemplate : MonoBehaviour
    {
        public Transform parent;

        public GameObject prefab;

        public GameObject Create()
        {
            return GameObject.Instantiate(prefab, parent);
        }

        public Transform CreateTransform()
        {
            return GameObject.Instantiate(prefab, parent).transform;
        }

        public T Create<T>() where T : Component
        {
            GameObject go = GameObject.Instantiate(prefab, parent);

            if (!go.TryGetComponent(out T component))
            {
                component = go.AddComponent<T>();
            }
            return component;
        }
    }
}