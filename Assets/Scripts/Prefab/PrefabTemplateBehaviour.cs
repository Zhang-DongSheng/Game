namespace UnityEngine
{
    /// <summary>
    /// 预制体实例组件
    /// </summary>
    [ExecuteInEditMode, DisallowMultipleComponent]
    public class PrefabTemplateBehaviour : MonoBehaviour
    {
        public PrefabTemplate template;

        private void Awake()
        {
            if (template == null)
                template = new PrefabTemplate();
            if (template.parent == null)
                template.parent = transform;
        }

        public GameObject Create()
        {
            return template.Create();
        }

        public T Create<T>() where T : Component
        {
            return template.Create<T>();
        }
    }
    [System.Serializable]
    public class PrefabTemplate
    {
        public Transform parent;

        public GameObject prefab;

        public GameObject Create()
        {
            return GameObject.Instantiate(prefab, parent);
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