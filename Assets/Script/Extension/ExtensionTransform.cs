using System.Collections.Generic;
using UnityEngine;

public static partial class Extension
{
    public static T AddComponent<T>(this Transform target) where T : Component
    {
        if (target != null && target.gameObject != null)
        {
            return target.gameObject.AddComponent<T>();
        }
        return null;
    }

    public static T AddOrReplaceComponent<T>(this Transform target) where T : Component
    {
        if (target != null && target.gameObject != null)
        {
            if (!target.TryGetComponent<T>(out T compontent))
            {
                compontent = target.AddComponent<T>();
            }
            return compontent;
        }
        return null;
    }

    public static List<T> FindComponentsInChildren<T>(this Transform target) where T : Component
    {
        List<T> components = new List<T>();

        for (int i = 0; i < target.childCount; i++)
        {
            if (target.GetChild(i).TryGetComponent(out T component))
            {
                components.Add(component);
            }
        }
        return components;
    }

    public static T FindComponentInParent<T>(this Transform target) where T : Component
    {
        Transform parent = target; T component = null;

        while (parent != null)
        {
            if (parent.TryGetComponent<T>(out component))
            {
                break;
            }
            parent = parent.parent;
        }
        return component;
    }

    public static void RemoveComponent<T>(this Transform target) where T : Component
    {
        if (target != null && target.gameObject != null)
        {
            if (target.TryGetComponent<T>(out T compontent))
            {
                UnityEngine.Object.Destroy(compontent);
            }
        }
    }

    public static void SetPosition(this Transform target, Vector3 position, bool local = true)
    {
        if (local)
        {
            target.localPosition = position;
        }
        else
        {
            target.position = position;
        }
    }

    public static void SetPosition(this Transform target, float value, TDAxis axis, bool local = true)
    {
        Vector3 position = local ? target.localPosition : target.position;

        switch (axis)
        {
            case TDAxis.X:
                position.x = value;
                break;
            case TDAxis.Y:
                position.y = value;
                break;
            case TDAxis.Z:
                position.z = value;
                break;
        }

        target.SetPosition(position, local);
    } 

    public static void SetPosition(this RectTransform rect, Vector2 position)
    {
        rect.anchoredPosition = position;
    }

    public static void SetSize(this RectTransform rect, Vector2 size)
    {
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);

        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
    }

    public static Vector2 Offset(RectTransform rect)
    {
        Vector2 offset = Vector2.zero;

        while (rect != null)
        {
            if (rect.TryGetComponent(out Canvas _))
            {
                break;
            }
            else
            {
                offset += rect.anchoredPosition;
            }
            rect = rect.parent.GetComponent<RectTransform>();
        }
        return offset;
    }

    public static void Full(this RectTransform target)
    {
        target.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);

        target.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);

        target.anchorMin = Vector2.zero;

        target.anchorMax = Vector2.one;
    }

    public static void Reset(this RectTransform rect)
    {
        rect.localPosition = Vector3.zero;

        rect.localRotation = Quaternion.identity;

        rect.localScale = Vector3.one;
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

public enum TDAxis
{
    X,
    Y,
    Z,
}