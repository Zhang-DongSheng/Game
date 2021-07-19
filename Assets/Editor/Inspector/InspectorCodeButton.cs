using UnityEngine;

namespace UnityEditor.Inspector
{
    [CustomEditor(typeof(CodeButton))]
    public class InspectorCodeButton : UI.ButtonEditor
    {
        private SerializedProperty key;

        private SerializedProperty value;

        protected override void OnEnable()
        {
            base.OnEnable();

            key = serializedObject.FindProperty("key");

            value = serializedObject.FindProperty("value");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(key, new GUIContent("key"));

            EditorGUILayout.PropertyField(value, new GUIContent("value"));

            serializedObject.ApplyModifiedProperties();

            base.OnInspectorGUI();
        }
    }
}