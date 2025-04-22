using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ILRuntimeView))]
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

            if (type != ILRuntimeComponentType.Custom)
            {
                return $"{key} = transform.Find({path}).GetComponent<{type}>();";
            }
            else
            {
                return $"{key} = transform.Find({path}).GetComponent<{custom}>();";
            }
        }
    }

    public enum ILRuntimeComponentType
    {
        GameObject,

        Transform,

        Text,

        Image,

        Custom,
    }
}