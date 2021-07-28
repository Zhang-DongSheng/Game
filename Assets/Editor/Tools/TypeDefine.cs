using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor
{
    public class TypeDefine
    {
        public readonly static List<Type> types = new List<Type>()
        {
            null,
            typeof(Transform),
            
            #region UI
            typeof(RectTransform),
            typeof(Button),
            typeof(Image),
            typeof(Text),
            typeof(Toggle),
            typeof(ToggleGroup),
            typeof(ScrollRect),
            typeof(Scrollbar),
            typeof(Slider),
            typeof(Shadow),
            typeof(Outline),
            #endregion
            
            #region Custom
            typeof(GraphicColorFade),
            #endregion
        };

        public static Type GetType(int index)
        {
            if (index < types.Count)
            {
                return types[index];
            }
            return null;
        }

        public static string[] ToArrayString()
        {
            string[] list = new string[types.Count];

            for (int i = 0; i < types.Count; i++)
            {
                if (types[i] != null)
                {
                    list[i] = types[i].ToString();
                }
                else
                {
                    list[i] = "None";
                }
            }
            return list;
        }
    }
}