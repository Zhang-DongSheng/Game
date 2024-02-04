using Game;
using UnityEngine;

namespace UnityEditor.Inspector
{
    [CustomPropertyDrawer(typeof(IntPair))]
    class IntPairDrawer : PropertyDrawer
    {
        private readonly Rect[] rects = new Rect[2];

        private float width;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                EditorGUIUtility.labelWidth = 50;

                position.height = EditorGUIUtility.singleLineHeight;

                width = position.width * 0.5f - 5;

                int count = rects.Length;

                for (int i = 0; i < count; i++)
                {
                    rects[i] = new Rect(position)
                    {
                        x = position.x + i * width + Mathf.Max(i, 0) * 10,
                        width = width,
                    };
                }

                var x = property.FindPropertyRelative("x");

                var y = property.FindPropertyRelative("y");

                x.intValue = EditorGUI.IntField(rects[0], "x", x.intValue);

                y.intValue = EditorGUI.IntField(rects[1], "y", y.intValue);
            }
        }
    }
    [CustomPropertyDrawer(typeof(UIntPair))]
    class UIntPairDrawer : PropertyDrawer
    {
        private readonly Rect[] rects = new Rect[2];

        private float width;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                EditorGUIUtility.labelWidth = 50;

                position.height = EditorGUIUtility.singleLineHeight;

                width = position.width * 0.5f - 5;

                int count = rects.Length;

                for (int i = 0; i < count; i++)
                {
                    rects[i] = new Rect(position)
                    {
                        x = position.x + i * width + Mathf.Max(i, 0) * 10,
                        width = width,
                    };
                }

                var x = property.FindPropertyRelative("x");

                var y = property.FindPropertyRelative("y");

                x.intValue = EditorGUI.IntField(rects[0], "x", x.intValue);

                y.intValue = EditorGUI.IntField(rects[1], "y", y.intValue);
            }
        }
    }
    [CustomPropertyDrawer(typeof(FloatPair))]
    class FloatPairDrawer : PropertyDrawer
    {
        private readonly Rect[] rects = new Rect[2];

        private float width;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                EditorGUIUtility.labelWidth = 50;

                position.height = EditorGUIUtility.singleLineHeight;

                width = position.width * 0.5f - 5;

                int count = rects.Length;

                for (int i = 0; i < count; i++)
                {
                    rects[i] = new Rect(position)
                    {
                        x = position.x + i * width + Mathf.Max(i, 0) * 10,
                        width = width,
                    };
                }

                var x = property.FindPropertyRelative("x");

                var y = property.FindPropertyRelative("y");

                x.floatValue = EditorGUI.FloatField(rects[0], "x", x.floatValue);

                y.floatValue = EditorGUI.FloatField(rects[1], "y", y.floatValue);
            }
        }
    }
}
