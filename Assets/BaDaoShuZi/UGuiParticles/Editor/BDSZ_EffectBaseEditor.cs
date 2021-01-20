
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
namespace BDSZ_2020
{
    [CustomEditor(typeof(BDSZ_EffectBase), true)]
    public class BDSZ_EffectBaseEditor : BDSZ_EffectBehaviourEditor
    {


        public static BDSZ_EffectBaseEditor s_Instance = null;

        SerializedProperty m_ShaderType;
        GUIContent m_ShaderTypeLabel;
        SerializedProperty m_TextureRes;
        GUIContent m_TextureResLabel;

        SerializedProperty m_fDuration;
        GUIContent m_fDurationLabel;

        SerializedProperty m_fStartDelay;
        GUIContent m_fStartDelayLabel;
        SerializedProperty m_fFadeIn;
        GUIContent m_fFadeInLabel;
        SerializedProperty m_fFadeOut;
        GUIContent m_fFadeOutLabel;

        SerializedProperty m_vPosition;
        GUIContent m_vPositionLabel;

        SerializedProperty m_fCurveVelocity;
        GUIContent m_fCurveVelocityLabel;

        BDSZ_EffectBase m_EffectBase;


        int m_iTableIndex = 0;
        protected virtual void OnEnable()
        {
            s_Instance = this;
            m_EffectBase = serializedObject.targetObject as BDSZ_EffectBase;
            m_ShaderTypeLabel = new GUIContent("ShaderType");
            m_ShaderType = serializedObject.FindProperty("m_ShaderType");
            m_TextureRes = serializedObject.FindProperty("m_TextureRes");
            m_TextureResLabel = new GUIContent("Main");

            m_fDuration = serializedObject.FindProperty("m_fDuration");
            m_fDurationLabel = new GUIContent("Duration");

            m_vPosition = serializedObject.FindProperty("m_vPosition");
            m_vPositionLabel = new GUIContent("Location");

            m_fStartDelay = serializedObject.FindProperty("m_fStartDelay");
            m_fStartDelayLabel = new GUIContent("Start Delay");

            m_fFadeIn = serializedObject.FindProperty("m_fFadeIn");
            m_fFadeInLabel = new GUIContent("FadeIn");
            m_fFadeOut = serializedObject.FindProperty("m_fFadeOut");
            m_fFadeOutLabel = new GUIContent("FadeOut");

            m_fCurveVelocity = serializedObject.FindProperty("m_fCurveVelocity");
            m_fCurveVelocityLabel = new GUIContent("Curve Velocity");
        }

        protected virtual void OnDisable()
        {
             s_Instance = null;
        }
         
        public void DrawLayout()
        {

            GUILayout.Space(3f);
            int newTab = m_iTableIndex;
            GUILayout.BeginHorizontal();
            if (GUILayout.Toggle(newTab == 0, "Effect", "ButtonLeft")) newTab = 0;
            if (GUILayout.Toggle(newTab == 1, "Texture", "ButtonRight")) newTab = 1;
            GUILayout.EndHorizontal();
            BDSZ_EditorGUIUtility.BeginContents();

            m_iTableIndex = newTab;
            // BDSZ_EditorGUIUtility.BeginContents(false);
            if (m_iTableIndex == 0)
            {
                LayoutGUI();
            }
            else if (m_iTableIndex == 1)
            {
                EditorGUILayout.PropertyField(m_TextureRes, m_TextureResLabel);
            }

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
        protected void EffectAttributeGUI(EEffectAttribute ea, string strLabel)
        {
            bool bLooping = m_EffectBase.IsEffectAttribute(ea);
            GUILayout.BeginHorizontal();
            GUILayout.Label(strLabel, GUILayout.Width(EditorGUIUtility.labelWidth));

            bool bEditValue = EditorGUILayout.Toggle(bLooping);
            if (bEditValue != bLooping)
            {
                m_EffectBase.UpdateEffectAttribute(bEditValue, ea);
                m_EffectBase.OnValidate();
            }
            GUILayout.EndHorizontal();
        }

        protected virtual void DrawStartAttribute()
        {
            ColorMinMaxGUI(m_EffectBase, "Start Color", m_EffectBase.GetStartColor(), m_EffectBase.SetStartColor);

        }
        protected virtual void DrawUpdateAttribute()
        {
            ColorGradientGUI(m_EffectBase, "Run Color", m_EffectBase.GetUpdateColor(), m_EffectBase.SetUpdateColor);
            EditorGUILayout.PropertyField(m_fFadeIn, m_fFadeInLabel);
            EditorGUILayout.PropertyField(m_fFadeOut, m_fFadeOutLabel);
        }
        protected virtual void LayoutGUI()
        {

           
            EditorGUILayout.PropertyField(m_ShaderType, m_ShaderTypeLabel);
            EditorGUILayout.PropertyField(m_fDuration, m_fDurationLabel);
            EditorGUILayout.PropertyField(m_fStartDelay, m_fStartDelayLabel);
            EditorGUILayout.PropertyField(m_vPosition, m_vPositionLabel);
            EffectAttributeGUI(EEffectAttribute.Looping, "Looping");
            EffectAttributeGUI(EEffectAttribute.Billboard, "Billboard");
            if (m_EffectBase.Curve != null)
            {
                EditorGUILayout.PropertyField(m_fCurveVelocity, m_fCurveVelocityLabel);
            }
        }
    }
}