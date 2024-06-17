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

        public bool reset;

        public GameObject Create()
        {
            var target = GameObject.Instantiate(prefab, parent);

            if (target != null && reset)
            {
                Reset(target.transform);
            }
            return target;
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

        public void Reset(Transform transform)
        {
            transform.localPosition = Vector3.zero;

            transform.localRotation = Quaternion.identity;

            transform.localScale = Vector3.one;
        }
    }
}