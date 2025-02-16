using Game.Effect;
using UnityEngine;

namespace UnityEditor.Inspector
{
    [CustomPropertyDrawer(typeof(PostProperty))]
    public class InspectorPostProperty : PropertyDrawer
    {
        private readonly Rect[] rects = new Rect[3];

        private readonly string label = "Value";

        private float width;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                EditorGUIUtility.labelWidth = 50;

                width = position.width / 3f - 5;

                position.height = EditorGUIUtility.singleLineHeight;

                int count = rects.Length;

                for (int i = 0; i < count; i++)
                {
                    rects[i] = new Rect(position)
                    {
                        x = position.x + i * width + Mathf.Clamp01(i) * 10,
                        width = width,
                    };
                }

                SerializedProperty sp_key = property.FindPropertyRelative("key");
                SerializedProperty sp_type = property.FindPropertyRelative("type");
                SerializedProperty sp_number = property.FindPropertyRelative("number");
                SerializedProperty sp_value = property.FindPropertyRelative("value");
                SerializedProperty sp_color = property.FindPropertyRelative("color");
                SerializedProperty sp_vector = property.FindPropertyRelative("vector");
                SerializedProperty sp_texture = property.FindPropertyRelative("texture");
                {
                    sp_key.stringValue =
                        EditorGUI.TextField(rects[0], "Param", sp_key.stringValue);
                    sp_type.enumValueIndex =
                        EditorGUI.Popup(rects[1], sp_type.enumValueIndex, sp_type.enumDisplayNames);
                    switch ((PostPropertyType)sp_type.enumValueIndex)
                    {
                        case PostPropertyType.Int:
                            sp_number.intValue =
                        EditorGUI.IntField(rects[2], this.label, sp_number.intValue);
                            break;
                        case PostPropertyType.Float:
                            sp_value.floatValue =
                        EditorGUI.FloatField(rects[2], this.label, sp_value.floatValue);
                            break;
                        case PostPropertyType.Color:
                            sp_color.colorValue =
                        EditorGUI.ColorField(rects[2], this.label, sp_color.colorValue);
                            break;
                        case PostPropertyType.Vector:
                            sp_vector.vector4Value =
                        EditorGUI.Vector4Field(rects[2], this.label, sp_vector.vector4Value);
                            break;
                        case PostPropertyType.Texture:
                            sp_texture.objectReferenceValue =
                        EditorGUI.ObjectField(rects[2], sp_texture.objectReferenceValue, typeof(Texture), false);
                            break;
                    }
                }
            }
        }
    }
}