using Game.Attribute;
using UnityEngine;

namespace UnityEditor.Inspector
{
    public class PropertyUtils
    {
        public static object Parameter(SerializedProperty property)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:
                    return property.intValue;
                case SerializedPropertyType.Boolean:
                    return property.boolValue;
                case SerializedPropertyType.Float:
                    return property.floatValue;
                case SerializedPropertyType.String:
                    return property.stringValue;
                case SerializedPropertyType.Color:
                    return property.colorValue;
                case SerializedPropertyType.ObjectReference:
                    return property.objectReferenceValue;
                case SerializedPropertyType.Enum:
                    return property.enumValueIndex;
                case SerializedPropertyType.Vector2:
                    return property.vector2Value;
                case SerializedPropertyType.Vector2Int:
                    return property.vector2IntValue;
                case SerializedPropertyType.Vector3:
                    return property.vector3Value;
                case SerializedPropertyType.Vector3Int:
                    return property.vector3IntValue;
                case SerializedPropertyType.Vector4:
                    return property.vector4Value;
                case SerializedPropertyType.Quaternion:
                    return property.quaternionValue;
                case SerializedPropertyType.Rect:
                    return property.rectValue;
                case SerializedPropertyType.RectInt:
                    return property.rectIntValue;
                case SerializedPropertyType.Bounds:
                    return property.boundsValue;
                case SerializedPropertyType.BoundsInt:
                    return property.boundsIntValue;
                case SerializedPropertyType.Hash128:
                    return property.hash128Value;
                default:
                    return null;
            }
        }
    }
    [CustomPropertyDrawer(typeof(ReadonlyAttribute))]
    class ReadonlyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ReadonlyAttribute _attribute = attribute as ReadonlyAttribute;

            GUI.enabled = _attribute.editor;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
    [CustomPropertyDrawer(typeof(IntervalAttribute))]
    class IntervalDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //UnityEngine.Assertions.Assert.IsTrue(property.propertyType == SerializedPropertyType.Vector2, "Devi usare un vector2 per MinMax");

            IntervalAttribute interval = attribute as IntervalAttribute;

            position = EditorGUI.PrefixLabel(position, label);

            switch (property.propertyType)
            {
                case SerializedPropertyType.Vector2:
                    {
                        Rect left = new Rect(position.x, position.y, 50, position.height);

                        Rect value = new Rect(left.xMax, position.y, position.width - left.width * 2 - 4, position.height);

                        Rect right = new Rect(position.xMax - left.width - 2, position.y, left.width, position.height);

                        float min = property.vector2Value.x;

                        float max = property.vector2Value.y;

                        EditorGUI.MinMaxSlider(value, ref min, ref max, interval.min, interval.max);

                        property.vector2Value = new Vector2(min, max);

                        EditorGUI.LabelField(left, min.ToString("F3"));

                        EditorGUI.LabelField(right, max.ToString("F3"));
                    }
                    break;
                case SerializedPropertyType.Float:
                    {
                        float value = property.floatValue;

                        value = EditorGUI.Slider(position, value, interval.min, interval.max);

                        property.floatValue = value;
                    }
                    break;
                default:
                    {
                        GUI.Label(position, "You can use Interval only on a Vector2!");
                    }
                    break;
            }
        }
    }
    [CustomPropertyDrawer(typeof(CurveAttribute))]
    class CurveDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            CurveAttribute curve = attribute as CurveAttribute;

            switch (property.propertyType)
            {
                case SerializedPropertyType.AnimationCurve:
                    {
                        EditorGUI.CurveField(position, property, curve.color, curve.ranges, label);
                    }
                    break;
                default:
                    {
                        GUI.Label(position, "You can use Curve only on a AnimationCurve!");
                    }
                    break;
            }
        }
    }
    [CustomPropertyDrawer(typeof(LineAttribute))]
    class LineDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            LineAttribute line = attribute as LineAttribute;

            position = EditorGUI.IndentedRect(position);

            position.y += (EditorGUIUtility.singleLineHeight - line.height) / 2.0f;

            position.height = line.height;

            EditorGUI.DrawRect(position, line.color);
        }
    }
    [CustomPropertyDrawer(typeof(DisplayAttribute))]
    class DisplayDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            DisplayAttribute display = attribute as DisplayAttribute;

            if (display.active)
            {
                return EditorGUI.GetPropertyHeight(property, label, true);
            }
            return 0;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            DisplayAttribute display = attribute as DisplayAttribute;

            if (display.active)
            {
                if (!string.IsNullOrEmpty(display.name))
                {
                    label.text = display.name;
                }
                EditorGUI.PropertyField(position, property, label, true);
            }
        }
    }
    [CustomPropertyDrawer(typeof(ButtonAttribute))]
    class ButtonDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true) * 2 + 5;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ButtonAttribute _attribute = attribute as ButtonAttribute;

            position.height -= 5;

            position.height *= 0.5f;

            EditorGUI.PropertyField(position, property, label, true);

            position.y += position.height;

            position.y += 3;

            if (GUI.Button(position, _attribute.function))
            {
                _attribute.Call(property.serializedObject.targetObject, PropertyUtils.Parameter(property));
            }
        }
    }
}