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
            if (type != HotfixComponentType.Custom)
            {
                return $"public {type} {key};";
            }
            else
            {
                return $"public {custom} {key};";
            }
        }

        public string ToRelevanceString(Transform parent)
        {
            var path = $"\"{target.FullName(parent)}\"";

            switch (type)
            {
                case HotfixComponentType.GameObject:
                    return $"{key} = transform.Find({path}).gameObject;";
                case HotfixComponentType.Transform:
                    return $"{key} = transform.Find({path});";
                case HotfixComponentType.Custom:
                    return $"{key} = transform.Find({path}).GetComponent<{type}>();";
                default:
                    return $"{key} = transform.Find({path}).GetComponent<{custom}>();";
            }
        }
    }

    public enum HotfixComponentType
    {
        Transform,

        GameObject,

        TextBind,

        ImageBind,

        ItemStatus,

        ItemSwitch,

        ItemSlider,

        PrefabTemplateBehaviour,

        Custom,
    }
}