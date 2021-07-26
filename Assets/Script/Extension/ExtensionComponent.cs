using System;
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

            foreach (var field in fields)
            {
                if (field.FieldType.IsGenericType)
                {
                    
                }
                else
                {
                    if (components.Contains(field.FieldType))
                    {
                        object value = field.GetValue(self);

                        if (value.ToString() == "null")
                        {
                            Component[] components = self.GetComponentsInChildren(field.FieldType);

                            for (int i = 0; i < components.Length; i++)
                            {
                                if (components[i].name.Contains(field.Name))
                                {
                                    field.SetValue(self, components[i]);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            Debug.LogError(field.Name + "ÒÑ¸³Öµ");
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