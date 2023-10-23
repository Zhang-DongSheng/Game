using UnityEngine;

namespace Game
{
    public static partial class Utility
    {
        /// <summary>
        /// Unity GameObject
        /// </summary>
        public static class GameObject
        {
            public static bool IsNullOrEmpty(UnityEngine.GameObject target)
            {
                return System.Object.ReferenceEquals(target, null);
            }

            public static UnityEngine.GameObject Create(UnityEngine.GameObject prefab, Transform parent = null)
            {
                UnityEngine.GameObject go = UnityEngine.GameObject.Instantiate(prefab, parent);

                go.transform.Reset();

                return go;
            }

            public static T Create<T>(UnityEngine.GameObject prefab, Transform parent = null) where T : Component
            {
                UnityEngine.GameObject go = UnityEngine.GameObject.Instantiate(prefab, parent);

                go.transform.Reset();

                if (!go.TryGetComponent(out T component))
                {
                    component = go.AddComponent<T>();
                }
                return component;
            }
        }
    }
}