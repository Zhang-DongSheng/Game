using UnityEngine;

namespace Game
{
    public static partial class Extension
    {
        public static T AddOrReplaceComponent<T>(this Transform target) where T : Component
        {
            if (target != null && target.gameObject != null)
            {
                if (!target.TryGetComponent(out T compontent))
                {
                    compontent = target.gameObject.AddComponent<T>();
                }
                return compontent;
            }
            return null;
        }

        public static T FindComponentInParent<T>(this Transform target) where T : Component
        {
            T component = null; Transform root = target;

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

        public static void RemoveComponent<T>(this Transform target) where T : Component
        {
            if (target != null && target.gameObject != null)
            {
                if (target.TryGetComponent<T>(out T compontent))
                {
                    Object.Destroy(compontent);
                }
            }
        }

        public static void Clear(this Transform target)
        {
            if (target != null && target.childCount > 0)
            {
                for (int i = target.childCount - 1; i > -1; i--)
                {
                    GameObject.Destroy(target.GetChild(i).gameObject);
                }
            }
        }
    }
}