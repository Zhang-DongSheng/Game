using System;
using System.Reflection;
using UnityEngine;

namespace Game.Attribute
{
    public class ButtonAttribute : PropertyAttribute
    {
        const BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        public string function;

        public bool parameter;

        public ButtonAttribute(string function, bool parameter = true)
        {
            this.function = function;

            this.parameter = parameter;
        }

#if UNITY_EDITOR
        public void Call(UnityEditor.SerializedProperty property)
        {
            try
            {
                Type type = property.serializedObject.targetObject.GetType();

                MethodInfo method = type.GetMethod(function, Flags);

                if (parameter)
                {
                    method.Invoke(property.serializedObject.targetObject, new object[] { Parameter(property) });
                }
                else
                {
                    method.Invoke(property.serializedObject.targetObject, null);
                }
            }
            catch (Exception e)
            {
                Debuger.LogException(Author.None, e);
            }
        }

        public object Parameter(UnityEditor.SerializedProperty property)
        {
            switch (property.propertyType)
            {
                case UnityEditor.SerializedPropertyType.Integer:
                    return property.intValue;
                case UnityEditor.SerializedPropertyType.Boolean:
                    return property.boolValue;
                case UnityEditor.SerializedPropertyType.Float:
                    return property.floatValue;
                case UnityEditor.SerializedPropertyType.String:
                    return property.stringValue;
                case UnityEditor.SerializedPropertyType.Color:
                    return property.colorValue;
                case UnityEditor.SerializedPropertyType.ObjectReference:
                    return property.objectReferenceValue;
                case UnityEditor.SerializedPropertyType.Enum:
                    return property.enumValueIndex;
                case UnityEditor.SerializedPropertyType.Vector2:
                    return property.vector2Value;
                case UnityEditor.SerializedPropertyType.Vector3:
                    return property.vector3Value;
                case UnityEditor.SerializedPropertyType.Vector4:
                    return property.vector4Value;
                case UnityEditor.SerializedPropertyType.Quaternion:
                    return property.quaternionValue;
                default:
                    return null;
            }
        }
#endif
    }
}