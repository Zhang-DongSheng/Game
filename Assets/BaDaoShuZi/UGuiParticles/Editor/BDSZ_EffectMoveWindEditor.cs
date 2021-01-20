 

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
namespace BDSZ_2020
{
    [CustomEditor(typeof(BDSZ_EffectWindForce), true)]

    public class BDSZ_EffectMoveWindEditor : BDSZ_EffectBehaviourEditor
    {
        

        public static BDSZ_EffectMoveWindEditor s_Instance = null;

        BDSZ_EffectWindForce m_EffectMove;


        SerializedProperty m_fGravity;
        GUIContent m_fGravityLabel;
        SerializedProperty m_vBoundMin;
        GUIContent m_vBoundMinLabel;
        SerializedProperty m_vBoundMax;
        GUIContent m_vBoundMaxLabel;


        protected virtual void OnEnable()
        {
            s_Instance = this;
            m_EffectMove = serializedObject.targetObject as BDSZ_EffectWindForce;
            m_fGravity = serializedObject.FindProperty("m_fGravity");
            m_fGravityLabel = new GUIContent("Gravity");
            m_vBoundMin = serializedObject.FindProperty("m_vBoundMin");
            m_vBoundMinLabel = new GUIContent("BoundMin");
            m_vBoundMax = serializedObject.FindProperty("m_vBoundMax");
            m_vBoundMaxLabel = new GUIContent("BoundMax");
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

           
            EditorGUILayout.PropertyField(m_vBoundMin, m_vBoundMinLabel);
            EditorGUILayout.PropertyField(m_vBoundMax, m_vBoundMaxLabel);
            EditorGUILayout.PropertyField(m_fGravity, m_fGravityLabel);
          
            FloatMinMaxGUI(m_EffectMove, "Angle Range", m_EffectMove.GetAngleRange(), m_EffectMove.SetAngleRange);

            FloatMinMaxGUI(m_EffectMove, "Angle Frequency", m_EffectMove.GetAngleFrequency(), m_EffectMove.SetAngleFrequency);
            FloatMinMaxGUI(m_EffectMove, "Force Range", m_EffectMove.GetForceRange(), m_EffectMove.SetForceRange);
            FloatMinMaxGUI(m_EffectMove, "Force Frequency", m_EffectMove.GetForceFrequency(), m_EffectMove.SetForceFrequency);

            FloatMinMaxGUI(m_EffectMove, "Particle Inertia", m_EffectMove.GetParticleInertia(), m_EffectMove.SetParticleInertia);
        }
    }
}