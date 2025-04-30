using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    [DisallowMultipleComponent]
    public class ILRuntimeComponents : MonoBehaviour
    {
        public List<ILRuntimeComponent> components;
    }
    [System.Serializable]
    public class ILRuntimeComponent
    {
        public string key;

        public Transform target;

        public ILRuntimeComponentType type;

        public string custom;

        public string ToDefineString()
        {
            if (type != ILRuntimeComponentType.Custom)
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
                case ILRuntimeComponentType.GameObject:
                    return $"{key} = transform.Find({path}).gameObject;";
                case ILRuntimeComponentType.Transform:
                    return $"{key} = transform.Find({path});";
                case ILRuntimeComponentType.Custom:
                    return $"{key} = transform.Find({path}).GetComponent<{type}>();";
                default:
                    return $"{key} = transform.Find({path}).GetComponent<{custom}>();";
            }
        }
    }

    public enum ILRuntimeComponentType
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