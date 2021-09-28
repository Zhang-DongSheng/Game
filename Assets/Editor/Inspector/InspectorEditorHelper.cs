using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Inspector
{
    [CustomEditor(typeof(EditorHelper))]
    public class InspectorEditorHelper : Editor
    {
        private EditorHelper editor;

        private void Awake()
        {
            editor = this.target as EditorHelper;
        }

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Ëæ»úÎ»ÖÃ"))
            {
                Vector2 point = Random.insideUnitCircle * 1000f;

                EditorHelper editor = serializedObject.targetObject as EditorHelper;

                Transform transform = editor.transform;

                transform.position = new Vector3(point.x, editor.transform.position.y, point.y);
            }
        }
    }
}