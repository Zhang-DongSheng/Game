 
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
namespace BDSZ_2020
{
    [CustomEditor(typeof(BDSZ_ParticleRoot), false)]

    public class BDSZ_ParticleRootEditor : Editor
    {
        public static BDSZ_ParticleRootEditor s_Instance = null;
        SerializedProperty m_bClip;
        GUIContent m_bClipLabel;

        protected virtual void OnEnable()
        {
            s_Instance = this;
                     
            m_bClip = serializedObject.FindProperty("m_bClip");
            m_bClipLabel = new GUIContent("Clip");
            
        }
        protected virtual void OnDisable()
        {
            s_Instance = null;
        }
        public void DrawLayout()
        {

            GUILayout.Space(3f);

            BDSZ_EditorGUIUtility.BeginContents();
            LayoutGUI();
            BDSZ_EditorGUIUtility.EndContents();

        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawLayout();
            if (serializedObject.hasModifiedProperties)
            {

                serializedObject.ApplyModifiedProperties();
            }
        }

        protected virtual void LayoutGUI()
        {

            EditorGUILayout.PropertyField(m_bClip, m_bClipLabel);
        }
    }
}