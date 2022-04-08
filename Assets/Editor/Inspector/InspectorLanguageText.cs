using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor
{
    [CustomEditor(typeof(TextLanguage))]
    public class InspectorLanguageText : UI.TextEditor
    {
        private SerializedProperty language;

        private SerializedProperty key;

        protected override void OnEnable()
        {
            base.OnEnable();

            language = serializedObject.FindProperty("language");

            key = serializedObject.FindProperty("key");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(language, new GUIContent("language"));

            if (language.boolValue)
            {
                EditorGUILayout.PropertyField(key, new GUIContent("key"));
            }
            serializedObject.ApplyModifiedProperties();

            base.OnInspectorGUI();
        }
    }
}