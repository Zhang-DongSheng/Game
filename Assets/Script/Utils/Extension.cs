using System;
using System.Collections.Generic;
using UnityEngine;

public static class Extension
{
    #region IEnumerable
    public static V FindByIndex<K, V>(this Dictionary<K, V> pairs, int index)
    {
        if (index > -1 && pairs.Count > index)
        {
            foreach (KeyValuePair<K, V> pair in pairs)
            {
                if (index-- == 0)
                {
                    return pair.Value;
                }
            }
        }
        return default;
    }

    public static V Find<K, V>(this Dictionary<K, V> pairs, Predicate<V> match)
    {
        foreach (KeyValuePair<K, V> pair in pairs)
        {
            if (match(pair.Value))
            {
                return pair.Value;
            }
        }
        return default;
    }

    public static List<T> ToList<T>(this string source, char separator)
    {
        if (string.IsNullOrEmpty(source)) return null;

        var list = new List<T>();

        var strs = source.Split(separator);

        foreach (var str in strs)
        {
            list.Add((T)Convert.ChangeType(str, typeof(T)));
        }
        return list;
    }
    #endregion

    #region Number
    public static bool EqualOrMoreThan(this int self, int value)
    {
        return self >= value;
    }

    public static bool EqualOrLessThan(this int self, int value)
    {
        return self <= value;
    }

    public static bool Between(this int self, int min, int max)
    {
        return self >= min && self <= max;
    }

    public static bool Near(this int self, int other, int distance)
    {
        return self.Between(other - distance, other + distance);
    }

    public static bool EqualOrMoreThan(this float self, float value)
    {
        return self >= value;
    }

    public static bool EqualOrLessThan(this float self, float value)
    {
        return self <= value;
    }

    public static bool Between(this float self, float min, float max)
    {
        return self >= min && self <= max;
    }

    public static bool Near(this float self, float other, float distance)
    {
        return self.Between(other - distance, other + distance);
    }

    public static string ToSimple(this int self, int digit)
    {
        float number;

        string unit = string.Empty;

        if (self < 1000)
        {
            number = self;
        }
        else if (self < 1000000)
        {
            number = self / 1000; unit = "k+";
        }
        else
        {
            number = self / 1000000; unit = "m+";
        }
        return string.Format("{0}{1}", Math.Round(number, digit), unit);
    }
    #endregion

    #region UnityEngine
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
    #endregion
}