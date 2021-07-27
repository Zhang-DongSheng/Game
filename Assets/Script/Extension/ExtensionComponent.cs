using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public static partial class Extension
    {
        private static readonly List<Type> components = new List<Type>()
        {
            typeof(GameObject),
            typeof(Transform),
            typeof(Button),
            typeof(Image),
            typeof(Text),
            typeof(Toggle),
            typeof(Scrollbar),
            typeof(ScrollRect),
            typeof(RectTransform),
        };

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
                if (go.TryGetComponent(out T component))
                {
                    UnityEngine.Object.Destroy(component);
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

        public static void Relevance(this Component self)
        {
            FieldInfo[] fields = self.GetType().GetFields(Flags);

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
                        if (field.GetValue(self) is IList list)
                        {
                            if (list.Count == 0)
                            {
                                Transform parent = null;

                                foreach (var child in self.GetComponentsInChildren<Transform>())
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
                                field.SetValue(self, list);
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
                        object value = field.GetValue(self);

                        if (Convert.IsDBNull(value) || value.ToString() == "null")
                        {
                            Component[] components = self.GetComponentsInChildren(field.FieldType);

                            for (int i = 0; i < components.Length; i++)
                            {
                                if (components[i].name.ToLower().Contains(name))
                                {
                                    field.SetValue(self, components[i]);
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