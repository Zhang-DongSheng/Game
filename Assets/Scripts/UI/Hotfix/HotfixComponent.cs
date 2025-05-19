using System;
using System.Reflection;
using UnityEngine;

namespace Game.UI
{
    [System.Serializable]
    public class HotfixComponent
    {
        public string key;

        public Transform target;

        public HotfixComponentType type;

        public string custom;

        public string ToDefineString()
        {
            switch (type)
            {
                case HotfixComponentType.Hotfix:
                case HotfixComponentType.Custom:
                    return $"public {custom} {key};";
                default:
                    return $"public {GetTypeFullName(type)} {key};";
            }
        }

        public string ToRelevanceString(Transform parent)
        {
            var path = $"\"{target.FullName(parent)}\"";

            switch (type)
            {
                case HotfixComponentType.Transform:
                    return $"{key} = transform.Find({path});";
                case HotfixComponentType.GameObject:
                    return $"{key} = transform.Find({path}).gameObject;";
                case HotfixComponentType.Hotfix:
                    return $"{key} = new {custom}(transform.Find({path}));";
                case HotfixComponentType.Custom:
                    return $"{key} = transform.Find({path}).GetComponent<{custom}>();";
                default:
                    return $"{key} = transform.Find({path}).GetComponent<{GetTypeFullName(type)}>();";
            }
        }

        public string GetTypeFullName(HotfixComponentType type)
        {
            switch (type)
            {
                case HotfixComponentType.Hotfix:
                case HotfixComponentType.Custom:
                    return custom;
                default:
                    { 
                        var name = Utility.Class.GetFullName(type.ToString(), x => x.IsInherit(typeof(MonoBehaviour)));

                        if (name.StartsWith("Game.UI."))
                        {
                            name = name[8..];
                        }
                        return name;
                    }
            }
        }
    }

    public enum HotfixComponentType
    {
        Transform = 0,

        GameObject = 1,

        Hotfix = 2,

        Custom = 3,

        TextBind,

        ImageBind,

        ItemStatus,

        ItemSwitch,

        ItemSlider,

        PrefabTemplateBehaviour,
    }
}