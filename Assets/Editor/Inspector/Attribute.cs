using UnityEngine;

namespace UnityEditor.Inspector
{
    [CustomPropertyDrawer(typeof(ReadonlyAttribute))]
    class ReadonlyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
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

            switch (property.propertyType)
            {
                case SerializedPropertyType.Vector2:
                    {
                        IntervalAttribute interval = attribute as IntervalAttribute;

                        Rect total = EditorGUI.PrefixLabel(position, label);

                        Rect left = new Rect(total.x, total.y, 50, total.height);

                        Rect value = new Rect(left.xMax, total.y, total.width - left.width * 2 - 4, total.height);

                        Rect right = new Rect(total.xMax - left.width - 2, total.y, left.width, total.height);

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
                        IntervalAttribute interval = attribute as IntervalAttribute;

                        position = EditorGUI.PrefixLabel(position, label);

                        float value = property.floatValue;

                        value = EditorGUI.Slider(position, value, interval.min, interval.max);

                        property.floatValue = value;
                    }
                    break;
                default:
                    {
                        GUI.Label(position, "You can use MinMax only on a Vector2!");
                    }
                    break;
            }
        }
    }
}