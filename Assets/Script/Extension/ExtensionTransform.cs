using System.Collections.Generic;
using UnityEngine;

public static partial class Extension
{
    public static T AddComponent<T>(this Transform target) where T : UnityEngine.Component
    {
        if (target != null && target.gameObject != null)
        {
            return target.gameObject.AddComponent<T>();
        }
        return null;
    }

    public static T GetOrAddCompontent<T>(this Transform target) where T : UnityEngine.Component
    {
        if (target != null && target.gameObject != null)
        {
            if (!target.TryGetComponent<T>(out T compontent))
            {
                compontent = target.gameObject.AddComponent<T>();
            }
            return compontent;
        }
        return null;
    }

    public static T GetComponentInChildren<T>(this Transform target, string name) where T : UnityEngine.Component
    {
        if (target != null)
        {
            Transform child = target.Find(name);
            if (child != null)
                return child.GetComponent<T>();
        }
        return null;
    }

    public static void RemoveComponent<T>(this Transform target) where T : UnityEngine.Component
    {
        if (target != null && target.gameObject != null)
        {
            if (target.TryGetComponent<T>(out T compontent))
            {
                UnityEngine.Object.Destroy(compontent);
            }
        }
    }

    public static Transform Root<T>(this Transform target) where T : UnityEngine.Component
    {
        if (target != null)
        {
            Transform node = target;

            while (node != null)
            {
                if (node.TryGetComponent<T>(out _))
                {
                    break;
                }
                node = node.parent;
            }
            return node;
        }
        return null;
    }

    public static void SetPositionX(this Transform target, float value, bool local = false)
    {
        if (target != null)
        {
            if (local)
            {
                Vector3 position = target.localPosition;
                position.x = value;
                target.localPosition = position;
            }
            else
            {
                Vector3 position = target.position;
                position.x = value;
                target.position = position;
            }
        }
    }

    public static void SetPositionY(this Transform target, float value, bool local = false)
    {
        if (target != null)
        {
            if (local)
            {
                Vector3 position = target.localPosition;
                position.y = value;
                target.localPosition = position;
            }
            else
            {
                Vector3 position = target.position;
                position.y = value;
                target.position = position;
            }
        }
    }

    public static void SetPositionZ(this Transform target, float value, bool local = false)
    {
        if (target != null)
        {
            if (local)
            {
                Vector3 position = target.localPosition;
                position.z = value;
                target.localPosition = position;
            }
            else
            {
                Vector3 position = target.position;
                position.z = value;
                target.position = position;
            }
        }
    }

    public static void Reset(this RectTransform target)
    {
        target.localPosition = Vector3.zero;

        target.localRotation = Quaternion.identity;

        target.localScale = Vector3.one;
    }

    public static void SetSize(this RectTransform target, Vector2 size)
    {
        target.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);

        target.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
    }

    public static void Full(this RectTransform target)
    {
        target.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);

        target.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);

        target.anchorMin = Vector2.zero;

        target.anchorMax = Vector2.one;
    }

    public static List<T> Children<T>(this Transform target)
    {
        List<T> list = new List<T>();

        for (int i = 0; i < target.childCount; i++)
        {
            if (target.GetChild(i).TryGetComponent(out T compontent))
            {
                list.Add(compontent);
            }
        }

        return list;
    }

    public static void ClearChildren(this Transform target)
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
