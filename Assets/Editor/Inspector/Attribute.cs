using UnityEngine;

namespace UnityEditor.Inspector
{
    [CustomPropertyDrawer(typeof(ReadonlyAttribute))]
    class ReadonlyDrawer : PropertyDrawer
    {
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
}