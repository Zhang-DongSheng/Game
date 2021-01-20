
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
namespace BDSZ_2020
{
    [CustomEditor(typeof(BDSZ_UIRectCurve), true)]

    public class BDSZ_UIRectCurveEditor : BDSZ_UICurveBaseEditor
    {
 

        SerializedProperty m_fWidth;
        GUIContent m_fWidthLabel;
        SerializedProperty m_fHeight;
        GUIContent m_fHeightLabel;

        protected override void OnEnable()
        {
            s_Instance = this;
            m_fWidth = serializedObject.FindProperty("m_fWidth");
            m_fWidthLabel = new GUIContent("Width");
            m_fHeight = serializedObject.FindProperty("m_fHeight");
            m_fHeightLabel = new GUIContent("Height");

        }
        
        protected override void LayoutGUI()
        {

            EditorGUILayout.PropertyField(m_fWidth, m_fWidthLabel);
            EditorGUILayout.PropertyField(m_fHeight, m_fHeightLabel);
        }
    }
}