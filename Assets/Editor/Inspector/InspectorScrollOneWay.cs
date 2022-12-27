using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor.Inspector
{
    [CustomEditor(typeof(ScrollOneWay))]
    public class InspectorScrollOneWay : UI.ScrollRectEditor
    {
        private SerializedProperty other;

        private SerializedProperty multi;

        protected override void OnEnable()
        {
            base.OnEnable();

            other = serializedObject.FindProperty("other");

            multi = serializedObject.FindProperty("multi");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(multi, new GUIContent("¿ªÆô"));

            EditorGUILayout.PropertyField(other, new GUIContent("Other"));

            serializedObject.ApplyModifiedProperties();

            base.OnInspectorGUI();
        }
    }
}