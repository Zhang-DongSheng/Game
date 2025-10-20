using Game;
using UnityEngine;

namespace UnityEditor.Inspector
{
    [CustomPropertyDrawer(typeof(IntPair))]
    class IntPairDrawer : PropertyDrawer
    {
        private Rect[] rects = new Rect[3];

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                Utility.Rectangle.EditorDraw(position, 2, ref rects);

                EditorGUI.LabelField(rects[0], label);

                EditorGUIUtility.labelWidth = 30;

                var x = property.FindPropertyRelative("x");

                var y = property.FindPropertyRelative("y");

                x.intValue = EditorGUI.IntField(rects[1], "X", x.intValue);

                y.intValue = EditorGUI.IntField(rects[2], "Y", y.intValue);
            }
        }
    }
    [CustomPropertyDrawer(typeof(UIntPair))]
    class UIntPairDrawer : PropertyDrawer
    {
        private Rect[] rects = new Rect[3];

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                Utility.Rectangle.EditorDraw(position, 2, ref rects);

                EditorGUI.LabelField(rects[0], label);

                EditorGUIUtility.labelWidth = 30;

                var x = property.FindPropertyRelative("x");

                var y = property.FindPropertyRelative("y");

                x.intValue = EditorGUI.IntField(rects[1], "X", x.intValue);

                y.intValue = EditorGUI.IntField(rects[2], "Y", y.intValue);
            }
        }
    }
    [CustomPropertyDrawer(typeof(FloatPair))]
    class FloatPairDrawer : PropertyDrawer
    {
        private Rect[] rects = new Rect[3];

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                Utility.Rectangle.EditorDraw(position, 2, ref rects);

                EditorGUI.LabelField(rects[0], label);

                EditorGUIUtility.labelWidth = 30;

                var x = property.FindPropertyRelative("x");

                var y = property.FindPropertyRelative("y");

                x.floatValue = EditorGUI.FloatField(rects[0], "x", x.floatValue);

                y.floatValue = EditorGUI.FloatField(rects[1], "y", y.floatValue);
            }
        }
        [CustomPropertyDrawer(typeof(StringPair))]
        class StringPairDrawer : PropertyDrawer
        {
            private Rect[] rects = new Rect[3];

            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                using (new EditorGUI.PropertyScope(position, label, property))
                {
                    Utility.Rectangle.EditorDraw(position, 2, ref rects);

                    EditorGUI.LabelField(rects[0], label);

                    EditorGUIUtility.labelWidth = 30;

                    var x = property.FindPropertyRelative("x");

                    var y = property.FindPropertyRelative("y");

                    x.stringValue = EditorGUI.TextField(rects[0], "x", x.stringValue);

                    y.stringValue = EditorGUI.TextField(rects[1], "y", y.stringValue);
                }
            }
        }
    }
}