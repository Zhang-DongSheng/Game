using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

namespace Game
{
    public static partial class Extension
    {
        /// <summary>
        /// 添加组件
        /// </summary>
        public static T AddOrReplaceComponent<T>(this Component component) where T : Component
        {
            if (component != null && component.gameObject is GameObject go)
            {
                if (!go.TryGetComponent(out T _component))
                {
                    _component = go.AddComponent<T>();
                }
                return _component;
            }
            return null;
        }
        /// <summary>
        /// 移除组件
        /// </summary>
        public static void RemoveComponent<T>(this Component component) where T : Component
        {
            if (component != null && component.gameObject is GameObject go)
            {
                if (go.TryGetComponent(out T _component))
                {
                    UnityEngine.Object.Destroy(_component);
                }
            }
        }
        /// <summary>
        /// 向上查找组件
        /// </summary>
        public static T FindComponentInParent<T>(this Component component) where T : Component
        {
            T result = null;

            Transform root = component.transform;

            while (root != null)
            {
                if (root.TryGetComponent(out result))
                {
                    break;
                }
                root = root.parent;
            }
            return result;
        }
        /// <summary>
        /// 自动关联Prefab
        /// </summary>
        public static void Relevance(this Component component)
        {
            FieldInfo[] fields = component.GetType().GetFields(Flags);

            string name;

            foreach (var field in fields)
            {
                name = field.Name.ToLower();

                if (field.FieldType.IsGenericType)
                {
                    Type element = field.FieldType.GetGenericArguments()[0];

                    if (typeof(IList).IsAssignableFrom(field.FieldType) &&
                        typeof(Component).IsAssignableFrom(element))
                    {
                        if (field.GetValue(component) is IList list)
                        {
                            if (list.Count == 0)
                            {
                                Transform parent = null;

                                foreach (var child in component.GetComponentsInChildren<Transform>())
                                {
                                    if (child.name.ToLower().Contains(name))
                                    {
                                        parent = child;
                                        break;
                                    }
                                }

                                if (parent == null) continue;

                                Component[] components = parent.GetComponentsInChildren(element);

                                for (int i = 0; i < components.Length; i++)
                                {
                                    if (components[i] != parent)
                                    {
                                        list.Add(components[i]);
                                    }
                                }
                                field.SetValue(component, list);
                            }
                            else
                            {
                                Debug.LogFormat("<color=green>[{0}]</color>已赋值", field.Name);
                            }
                        }
                    }
                }
                else
                {
                    if (typeof(Component).IsAssignableFrom(field.FieldType))
                    {
                        object value = field.GetValue(component);

                        if (Convert.IsDBNull(value) || value == null || value.ToString() == "null")
                        {
                            Component[] components = component.GetComponentsInChildren(field.FieldType);

                            for (int i = 0; i < components.Length; i++)
                            {
                                if (components[i].name.ToLower().Contains(name))
                                {
                                    field.SetValue(component, components[i]);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            Debug.LogFormat("<color=green>[{0}]</color>已赋值", field.Name);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 显示隐藏
        /// </summary>
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