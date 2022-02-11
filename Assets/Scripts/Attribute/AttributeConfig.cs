using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine
{
    public static class AttributeConfig
    {
        public static Color Color(AttributeColor attributeColor)
        {
            switch (attributeColor)
            {
                case AttributeColor.None:
                    return UnityEngine.Color.white;
                case AttributeColor.Red:
                    return UnityEngine.Color.red;
                case AttributeColor.Yellow:
                    return UnityEngine.Color.yellow;
                case AttributeColor.Blue:
                    return UnityEngine.Color.blue;
                case AttributeColor.Gray:
                    return UnityEngine.Color.gray;
                case AttributeColor.Green:
                    return UnityEngine.Color.green;
                case AttributeColor.Clear:
                    return UnityEngine.Color.black;
                default:
                    return UnityEngine.Color.white;
            }
        }
    }

    public enum AttributeColor
    { 
        None,
        Red,
        Yellow,
        Blue,
        Gray,
        Green,
        Clear,
    }
}
