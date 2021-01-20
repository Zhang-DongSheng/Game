using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
namespace BDSZ_2020
{
    [CustomEditor(typeof(BDSZ_UICircleCurve), true)]

    public class BDSZ_UICircleCurveEditor : BDSZ_UICurveBaseEditor
    {

        SerializedProperty m_fCircleRadius;
        GUIContent m_fCircleRadiusLabel;

        SerializedProperty m_iArcAngle;
        GUIContent m_iArcAngleLabel;
        protected override void OnEnable()
        {
            s_Instance = this;
            m_fCircleRadius = serializedObject.FindProperty("m_fCircleRadius");
            m_fCircleRadiusLabel = new GUIContent("Radius");
            m_iArcAngle = serializedObject.FindProperty("m_iArcAngle");
            m_iArcAngleLabel = new GUIContent("Arc Angle");
        }
        protected override void LayoutGUI()
        {
            EditorGUILayout.PropertyField(m_fCircleRadius, m_fCircleRadiusLabel);
            EditorGUILayout.PropertyField(m_iArcAngle, m_iArcAngleLabel);
        }
    }
}