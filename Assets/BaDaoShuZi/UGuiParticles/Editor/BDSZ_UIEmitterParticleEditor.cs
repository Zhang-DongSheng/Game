using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
namespace BDSZ_2020
{
    [CustomEditor(typeof(BDSZ_UIEmitterParticles), true)]

    public class BDSZ_UIEmitterParticleEditor : BDSZ_UIParticleEditor
    {


        BDSZ_UIEmitterParticles m_Particle = null;
        SerializedProperty m_Emitter;


        protected override void OnEnable()
        {
            m_bParticelEmitter = true;
            base.OnEnable();
            m_Emitter = serializedObject.FindProperty("m_Emitter");
            m_Particle = serializedObject.targetObject as BDSZ_UIEmitterParticles;
        }

        protected void EmitterGUI()
        {

            if (BDSZ_EditorGUIUtility.DrawHeader("Emitter"))
            {
                EditorGUILayout.PropertyField(m_Emitter);
            }

        }
        protected override void LayoutGUI()
        {
            base.LayoutGUI();

            EmitterGUI();
        }
    }
}