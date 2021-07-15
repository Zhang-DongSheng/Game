using UnityEngine;

namespace Game
{
    public static partial class Extension
    {
        public static T AddOrReplaceComponent<T>(this Component self) where T : Component
        {
            if (self != null && self.gameObject is GameObject go)
            {
                if (!go.TryGetComponent(out T component))
                {
                    component = go.AddComponent<T>();
                }
                return component;
            }
            return null;
        }

        public static void RemoveComponent<T>(this Component self) where T : Component
        {
            if (self != null && self.gameObject is GameObject go)
            {
                if (go.TryGetComponent(out T compontent))
                {
                    Object.Destroy(compontent);
                }
            }
        }

        public static T FindComponentInParent<T>(this Component self) where T : Component
        {
            T component = null; Transform root = self.transform;

            while (root != null)
            {
                if (root.TryGetComponent(out component))
                {
                    break;
                }
                root = root.parent;
            }
            return component;
        }

        public static void SetActive(this Component component, bool active)
        {
            if (component != null && component.gameObject is GameObject go)
            {
                if (go != null && go.activeSelf != active)
                {
                    go.SetActive(active);
                }
            }
        }
    }
}