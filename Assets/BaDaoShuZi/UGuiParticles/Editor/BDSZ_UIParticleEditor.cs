 

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
namespace BDSZ_2020
{
    
    [CustomEditor(typeof(BDSZ_UIParticle), true)]

    public class BDSZ_UIParticleEditor : BDSZ_EffectBaseEditor
    {
        SerializedProperty m_fGravity;
        GUIContent m_fGravityLabel;
        SerializedProperty m_bAllowProxy;
        GUIContent m_bAllowProxyLabel;

        SerializedProperty m_fRunCycle;
        GUIContent m_fRunCycleLabel;

        protected bool m_bParticelEmitter = false;

        BDSZ_UIParticle m_ParticleEffect = null;
        protected override void LayoutGUI()
        {
            base.LayoutGUI();

            if (BDSZ_EditorGUIUtility.DrawHeader("Particle"))
            {
                ParticleGUI(m_bParticelEmitter);
            }

        }
        protected override void OnEnable()
        {
            base.OnEnable();
            m_ParticleEffect = serializedObject.targetObject as BDSZ_UIParticle;
            m_fGravity = serializedObject.FindProperty("m_fGravity");
            m_fGravityLabel = new GUIContent("Gravity");

            m_fRunCycle = serializedObject.FindProperty("m_fRunCycle");
            m_fRunCycleLabel = new GUIContent("Run Cycle");
            m_bAllowProxy = serializedObject.FindProperty("m_bAllowProxy"); ;
            m_bAllowProxyLabel = new GUIContent("Allow Proxy");
            m_ParticleEffect.DrawSelectBound();
        }
        

        protected void ParticleGUI(bool bEmitter)
        {
            // align margin============================================
            EditorGUILayout.PropertyField(m_bAllowProxy, m_bAllowProxyLabel);
            base.DrawStartAttribute();
            FloatMinMaxGUI(m_ParticleEffect, "Start Size", m_ParticleEffect.GetStartSize(), m_ParticleEffect.SetStartSize);
            FloatMinMaxGUI(m_ParticleEffect, "Start Angle", m_ParticleEffect.GetStartAngle(), m_ParticleEffect.SetStartAngle);
            if (bEmitter)
                FloatMinMaxGUI(m_ParticleEffect, "Start Velocity", m_ParticleEffect.GetStartVelocity(), m_ParticleEffect.SetStartVelocity);
            if (bEmitter)
            {
                EditorGUILayout.PropertyField(m_fGravity, m_fGravityLabel);
            }
            base.DrawUpdateAttribute();
            FloatGradientGUI(m_ParticleEffect, "Run XScale", m_ParticleEffect.GetUpdateXScale(), m_ParticleEffect.SetUpdateXScale);
            FloatGradientGUI(m_ParticleEffect, "Run YScale", m_ParticleEffect.GetUpdateYScale(), m_ParticleEffect.SetUpdateYScale);
            FloatGradientGUI(m_ParticleEffect, "Run Angle", m_ParticleEffect.GetUpdateAngle(), m_ParticleEffect.SetUpdateAngle);
            if (bEmitter)
                FloatGradientGUI(m_ParticleEffect, "Run Velocity", m_ParticleEffect.GetUpdateVelocity(), m_ParticleEffect.SetUpdateVelocity);
            if (BDSZ_EditorGUIUtility.DrawHeader("Unusual"))
            {
                if (bEmitter)
                    EditorGUILayout.PropertyField(m_fRunCycle, m_fRunCycleLabel);
                EffectAttributeGUI(EEffectAttribute.StretchedBillboard, "Stretched Billboard");
                
                FloatMinMaxGUI(m_ParticleEffect, "CenterX", m_ParticleEffect.GetCenterX(), m_ParticleEffect.SetCenterX);
                FloatMinMaxGUI(m_ParticleEffect, "CenterY", m_ParticleEffect.GetCenterY(), m_ParticleEffect.SetCenterY);
            }
        }
    }
}