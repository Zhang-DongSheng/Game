using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor.Inspector
{
    [CustomEditor(typeof(UIEffectLayout))]
    class InspectorUIEffectLayout : Editor
    {
        private UIEffectLayout layout;

        private bool custom;

        private void Awake()
        {
            layout = this.target as UIEffectLayout;
        }

        public override void OnInspectorGUI()
        {
            custom = EditorGUILayout.Toggle("自定义布局", custom);

            if (custom)
            {
                this.serializedObject.Update();

                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("auto"), new GUIContent("自动定位"), false);

                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("ignore"), new GUIContent("忽略"), false);

                SerializedProperty variety = this.serializedObject.FindProperty("variety");

                EditorGUILayout.PropertyField(variety, new GUIContent("亮度调整"), false);

                if (variety.boolValue)
                {
                    EditorGUI.indentLevel++;

                    EditorGUILayout.PropertyField(this.serializedObject.FindProperty("color"), new GUIContent("颜色"));

                    EditorGUILayout.PropertyField(this.serializedObject.FindProperty("lighteness"), new GUIContent("亮度"));

                    EditorGUI.indentLevel--;
                }

                this.serializedObject.ApplyModifiedProperties();
            }
            else
            {
                base.DrawDefaultInspector();
            }
        }
    }
}