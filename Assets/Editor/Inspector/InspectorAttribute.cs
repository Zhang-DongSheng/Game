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
    [CustomPropertyDrawer(typeof(FieldNameAttribute))]
    class DisplayDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            FieldNameAttribute display = attribute as FieldNameAttribute;

            if (display.active)
            {
                return EditorGUI.GetPropertyHeight(property, label, true);
            }
            return 0;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            FieldNameAttribute display = attribute as FieldNameAttribute;

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
    [CustomPropertyDrawer(typeof(SuffixAttribute))]
    class SuffixDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SuffixAttribute suffix = attribute as SuffixAttribute;

            label.text = string.Format("{0} {1}", label.text, suffix.suffix);

            EditorGUI.PropertyField(position, property, label, property.isExpanded);
        }
    }
    [CustomPropertyDrawer(typeof(IntervalAttribute))]
    class IntervalDrawer : PropertyDrawer
    {
        private Vector2 variable = new Vector2(0, 0);

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            IntervalAttribute interval = attribute as IntervalAttribute;

            position = EditorGUI.PrefixLabel(position, label);

            switch (property.propertyType)
            {
                case SerializedPropertyType.Vector2:
                    {
                        Rect left = new Rect(position.x, position.y, 50, position.height);

                        Rect value = new Rect(left.xMax, position.y, position.width - left.width * 2 - 4, position.height);

                        Rect right = new Rect(position.xMax - left.width - 2, position.y, left.width, position.height);

                        variable.x = property.vector2Value.x;

                        variable.y = property.vector2Value.y;

                        EditorGUI.MinMaxSlider(value, ref variable.x, ref variable.y, interval.min, interval.max);

                        property.vector2Value = new Vector2(variable.x, variable.y);

                        EditorGUI.LabelField(left, variable.x.ToString("F3"));

                        EditorGUI.LabelField(right, variable.y.ToString("F3"));
                    }
                    break;
                case SerializedPropertyType.Vector2Int:
                    {
                        Rect left = new Rect(position.x, position.y, 50, position.height);

                        Rect value = new Rect(left.xMax, position.y, position.width - left.width * 2 - 4, position.height);

                        Rect right = new Rect(position.xMax - left.width - 2, position.y, left.width, position.height);

                        variable.x = property.vector2IntValue.x;

                        variable.y = property.vector2IntValue.y;

                        EditorGUI.MinMaxSlider(value, ref variable.x, ref variable.y, interval.min, interval.max);

                        property.vector2IntValue = new Vector2Int((int)variable.x, (int)variable.y);

                        EditorGUI.LabelField(left, variable.x.ToString());

                        EditorGUI.LabelField(right, variable.y.ToString());
                    }
                    break;
                case SerializedPropertyType.Float:
                    {
                        variable.x = Mathf.Clamp(property.floatValue, interval.min, interval.max);

                        variable.x = EditorGUI.Slider(position, variable.x, interval.min, interval.max);

                        property.floatValue = variable.x;
                    }
                    break;
                case SerializedPropertyType.Integer:
                    {
                        variable.x = Mathf.Clamp(property.intValue, interval.min, interval.max);

                        variable.x = EditorGUI.Slider(position, variable.x, interval.min, interval.max);

                        property.intValue = (int)variable.x;
                    }
                    break;
                default:
                    {
                        GUI.Label(position, "You can use Interval by " + property.propertyType);
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
            EditorGUI.PropertyField(position, property, label, property.isExpanded);

            LineAttribute line = attribute as LineAttribute;

            position = EditorGUI.IndentedRect(position);

            position.y += (EditorGUIUtility.singleLineHeight - line.height) / 2.0f;

            position.height = line.height;

            EditorGUI.DrawRect(position, line.color);
        }
    }
    [CustomPropertyDrawer(typeof(ButtonAttribute))]
    class ButtonDrawer : PropertyDrawer
    {
        private string function;

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

            if (_attribute.parameter)
            {
                function = string.Format("{0}({1} parameter)", _attribute.function, property.type);
            }
            else
            {
                function = _attribute.function;
            }
            if (GUI.Button(position, function))
            {
                _attribute.Call(property.serializedObject.targetObject, PropertyUtils.Parameter(property));
            }
        }
    }
}