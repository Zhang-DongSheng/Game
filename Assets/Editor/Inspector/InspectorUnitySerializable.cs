using Data.Serializable;
using UnityEngine;

namespace UnityEditor
{
    [CustomPropertyDrawer(typeof(UnityVector2))]
    public class InspectorUnityVector2 : PropertyDrawer
    {
        private const int COUNT = 2;

        private const int NAMELENGTH = 81;

        private SerializedProperty[] properties = new SerializedProperty[COUNT];

        private Rect[] rects = new Rect[COUNT];

        private Vector2 cell;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                EditorGUIUtility.labelWidth = NAMELENGTH;

                EditorGUI.LabelField(position, property.displayName);

                EditorGUIUtility.labelWidth = 30;

                position.x += NAMELENGTH;

                position.width -= NAMELENGTH;

                position.height = EditorGUIUtility.singleLineHeight;

                properties[0] = property.FindPropertyRelative("x");

                properties[1] = property.FindPropertyRelative("y");

                cell = new Vector2(position.width / COUNT, position.height);

                for (int i = 0; i < COUNT; i++)
                {
                    rects[i] = new Rect(position)
                    {
                        x = position.x + cell.x * i,
                        width = cell.x,
                    };
                    EditorGUI.FloatField(rects[i], properties[i].displayName, properties[i].floatValue);
                }
            }
        }
    }
    [CustomPropertyDrawer(typeof(UnityVector3))]
    public class InspectorUnityVector3 : PropertyDrawer
    {
        private const int COUNT = 3;

        private const int NAMELENGTH = 81;

        private SerializedProperty[] properties = new SerializedProperty[COUNT];

        private Rect[] rects = new Rect[COUNT];

        private Vector2 cell;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                EditorGUIUtility.labelWidth = NAMELENGTH;

                EditorGUI.LabelField(position, property.displayName);

                EditorGUIUtility.labelWidth = 30;

                position.x += NAMELENGTH;

                position.width -= NAMELENGTH;

                position.height = EditorGUIUtility.singleLineHeight;

                properties[0] = property.FindPropertyRelative("x");

                properties[1] = property.FindPropertyRelative("y");

                properties[2] = property.FindPropertyRelative("z");

                cell = new Vector2(position.width / COUNT, position.height);

                for (int i = 0; i < COUNT; i++)
                {
                    rects[i] = new Rect(position)
                    {
                        x = position.x + cell.x * i,
                        width = cell.x,
                    };
                    EditorGUI.FloatField(rects[i], properties[i].displayName, properties[i].floatValue);
                }
            }
        }
    }
    [CustomPropertyDrawer(typeof(UnityVector4))]
    public class InspectorUnityVector4 : PropertyDrawer
    {
        private const int COUNT = 4;

        private const int NAMELENGTH = 81;

        private const float LINESPACE = 2f;

        private SerializedProperty[] properties = new SerializedProperty[COUNT];

        private Rect[] rects = new Rect[COUNT];

        private Vector2 cell;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 2 + LINESPACE;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                EditorGUIUtility.labelWidth = NAMELENGTH;

                EditorGUI.LabelField(position, property.displayName);

                EditorGUIUtility.labelWidth = 30;

                position.x += NAMELENGTH;

                position.width -= NAMELENGTH;

                position.height = EditorGUIUtility.singleLineHeight;

                properties[0] = property.FindPropertyRelative("x");

                properties[1] = property.FindPropertyRelative("y");

                properties[2] = property.FindPropertyRelative("z");

                properties[3] = property.FindPropertyRelative("w");

                cell = new Vector2(position.width / 2, position.height);

                for (int i = 0; i < COUNT; i++)
                {
                    if (i < 2)
                    {
                        rects[i] = new Rect(position)
                        {
                            x = i * cell.x + position.x,
                            width = cell.x,
                        };
                    }
                    else
                    {
                        rects[i] = new Rect(position)
                        {
                            x = position.x + cell.x * (i - 2),
                            y = position.y + cell.y + LINESPACE,
                            width = cell.x,
                        };
                    }
                    EditorGUI.FloatField(rects[i], properties[i].displayName, properties[i].floatValue);
                }
            }
        }
    }
    [CustomPropertyDrawer(typeof(UnityColor))]
    public class InspectorUnityColor : PropertyDrawer
    {
        private const int COUNT = 4;

        private const int NAMELENGTH = 81;

        private SerializedProperty[] properties = new SerializedProperty[COUNT];

        private Color color;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                EditorGUIUtility.labelWidth = NAMELENGTH;

                EditorGUI.LabelField(position, property.displayName);

                EditorGUIUtility.labelWidth = 30;

                position.x += NAMELENGTH;

                position.width -= NAMELENGTH;

                position.height = EditorGUIUtility.singleLineHeight;

                properties[0] = property.FindPropertyRelative("r");

                properties[1] = property.FindPropertyRelative("g");

                properties[2] = property.FindPropertyRelative("b");

                properties[3] = property.FindPropertyRelative("a");

                color = new Color(properties[0].floatValue,
                    properties[1].floatValue,
                    properties[2].floatValue,
                    properties[3].floatValue);
                color = EditorGUI.ColorField(position, color);

                properties[0].floatValue = color.r;

                properties[1].floatValue = color.g;

                properties[2].floatValue = color.b;

                properties[3].floatValue = color.a;
            }
        }
    }
    [CustomPropertyDrawer(typeof(UnityParameter))]
    public class InspectorUnityParameter : PropertyDrawer
    {
        private const int COUNT = 2;

        private const int NAMELENGTH = 81;

        private SerializedProperty[] properties = new SerializedProperty[COUNT];

        private Rect[] rects = new Rect[COUNT];

        private Vector2 cell;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                EditorGUIUtility.labelWidth = NAMELENGTH;

                EditorGUI.LabelField(position, property.displayName);

                EditorGUIUtility.labelWidth = 40;

                position.x += NAMELENGTH;

                position.width -= NAMELENGTH;

                position.height = EditorGUIUtility.singleLineHeight;

                properties[0] = property.FindPropertyRelative("key");

                properties[1] = property.FindPropertyRelative("value");

                cell = new Vector2(position.width / COUNT, position.height);

                for (int i = 0; i < COUNT; i++)
                {
                    rects[i] = new Rect(position)
                    {
                        x = position.x + cell.x * i,
                        width = cell.x,
                    };
                    EditorGUI.TextField(rects[i], properties[i].displayName, properties[i].stringValue);
                }
            }
        }
    }
}