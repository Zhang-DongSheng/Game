using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
namespace BDSZ_2020
{
    public class BDSZ_EffectFloatGradientEditor : ScriptableWizard
    {
        static public BDSZ_EffectFloatGradientEditor s_Instance;

        void OnEnable() { s_Instance = this; }
        void OnDisable() { s_Instance = null; }

        BDSZ_EffectBehaviour m_EffectBase;
        EffectFloatGradient m_FloatGradient;
        BDSZ_EffectBaseEditor.SetFloatGradientFuncion m_SetFunction;
        public void Init(BDSZ_EffectBehaviour eb, EffectFloatGradient cg, BDSZ_EffectBaseEditor.SetFloatGradientFuncion fun)
        {
            m_EffectBase = eb;
            m_FloatGradient = cg;
            m_SetFunction = fun;
        }
        void OnGUI()
        {
            GUILayout.Space(16f);
            GUILayout.BeginHorizontal();
            GUILayout.Space(16f);
            GUILayout.Label("Key", GUILayout.MinWidth(30f));
            GUILayout.Label("Time", GUILayout.MinWidth(30f));
            GUILayout.EndHorizontal();

            for (int i = 0; i < 4; i++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(16f);

                GUI.changed = false;
                float colorValue = m_FloatGradient.GetKeyValue(i);
                float clrEdit = EditorGUILayout.FloatField("", colorValue, GUILayout.MinWidth(30f));
                if (GUI.changed)
                {
                    m_FloatGradient.SetKeyValue(i, clrEdit);
                    m_SetFunction(m_FloatGradient);
                    
                    BDSZ_EditorGUIUtility.RegisterUndo(m_EffectBase.name + "Gradient" + i, m_EffectBase);
                }
                if (i == 0)
                    EditorGUILayout.Slider(0.0f, 0.0f, 0.0f, GUILayout.MinWidth(30.0f));
                else if (i == 3)
                    EditorGUILayout.Slider(1.0f, 1.0f, 1.0f, GUILayout.MinWidth(30.0f));
                else
                {
                    float fv = m_FloatGradient.GetTimeValue(i);
                    float fEditValue = EditorGUILayout.Slider(fv, 0.0f, 1.0f, GUILayout.MinWidth(30.0f));
                    if (fv != fEditValue)
                    {
                        m_FloatGradient.SetTimeValue(i, fEditValue);
                        m_SetFunction(m_FloatGradient);
                        BDSZ_EditorGUIUtility.RegisterUndo(m_EffectBase.name + "Gradient" + i, m_EffectBase);
                    }
                }
                GUILayout.EndHorizontal();
            }

        }
        static public void Show(BDSZ_EffectBehaviour eb, EffectFloatGradient cg, BDSZ_EffectBehaviourEditor.SetFloatGradientFuncion fun)
        {
            if (s_Instance == null)
            {
                s_Instance = ScriptableWizard.DisplayWizard<BDSZ_EffectFloatGradientEditor>("Editor Gradient");
            }
            s_Instance.Init(eb, cg, fun);

        }

    }
}