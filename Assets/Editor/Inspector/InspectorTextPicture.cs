using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(TextPicture))]
    public class InspectorTextPicture : UnityEditor.UI.TextEditor
    {
        private SerializedProperty offset;

        private SerializedProperty scale;

        private SerializedProperty link;

        private SerializedProperty list;

        protected override void OnEnable()
        {
            base.OnEnable();

            offset = serializedObject.FindProperty("imageOffset");

            scale = serializedObject.FindProperty("ImageScalingFactor");

            link = serializedObject.FindProperty("hyperlinkColor");

            list = serializedObject.FindProperty("inspectorIconList");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.PropertyField(offset, new GUIContent("Offset"));

            EditorGUILayout.PropertyField(scale, new GUIContent("Scale"));

            EditorGUILayout.PropertyField(link, new GUIContent("Link Color"));

            EditorGUILayout.PropertyField(list, new GUIContent("List"), true);

            serializedObject.ApplyModifiedProperties();
        }
    }
}