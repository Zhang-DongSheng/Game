using System;

namespace UnityEngine
{
    /// <summary>
    /// Ԥ����ʵ������
    /// </summary>
    [Serializable]
    public class PrefabAndParent
    {
        public Transform parent;

        public GameObject prefab;

        public string name;

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