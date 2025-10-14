namespace UnityEngine
{
    /// <summary>
    /// 预制体模板组件
    /// </summary>
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    public class PrefabTemplateComponent : MonoBehaviour
    {
        public PrefabTemplate template;

        private void Awake()
        {
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
            var target = Create();

            if (!target.TryGetComponent(out T component))
            {
                component = target.AddComponent<T>();
            }
            return component;
        }
    }
}