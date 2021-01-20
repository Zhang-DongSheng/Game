using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
namespace BDSZ_2020
{
    public class BDSZ_EffectColorGradientEditor : ScriptableWizard
    {
        static public BDSZ_EffectColorGradientEditor s_Instance;

        void OnEnable() { s_Instance = this; }
        void OnDisable() { s_Instance = null; }

        BDSZ_EffectBehaviour m_EffectBase;
        EffectColorGradient m_ColorGradient;
        BDSZ_EffectBaseEditor.SetColorGradientFuncion m_SetFunction;
        

        public void Init(BDSZ_EffectBehaviour eb, EffectColorGradient cg, BDSZ_EffectBaseEditor.SetColorGradientFuncion fun)
        {
            m_EffectBase = eb;
            m_ColorGradient = cg;
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
                Color colorValue = m_ColorGradient.GetKeyValue(i);
                Color clrEdit = EditorGUILayout.ColorField("", colorValue, GUILayout.MinWidth(30f));
                if (GUI.changed)
                {
                    m_ColorGradient.SetKeyValue(i, clrEdit);
                    m_SetFunction(m_ColorGradient);
                    BDSZ_EditorGUIUtility.RegisterUndo(m_EffectBase.name + "Gradient" + i, m_EffectBase);
                }
                if (i == 0)
                    EditorGUILayout.Slider(0.0f, 0.0f, 0.0f, GUILayout.MinWidth(30.0f));
                else if (i == 3)
                    EditorGUILayout.Slider(1.0f, 1.0f, 1.0f, GUILayout.MinWidth(30.0f));
                else
                {
                    float fv = m_ColorGradient.GetTimeValue(i);
                    float fEditValue = EditorGUILayout.Slider(fv, 0.0f, 1.0f, GUILayout.MinWidth(30.0f));
                    if (fv != fEditValue)
                    {
                        m_ColorGradient.SetTimeValue(i, fEditValue);
                        m_SetFunction(m_ColorGradient);
                        BDSZ_EditorGUIUtility.RegisterUndo(m_EffectBase.name + "Gradient" + i, m_EffectBase);
                    }
                }
                GUILayout.EndHorizontal();
            }

        }
        static public void Show(BDSZ_EffectBehaviour eb, EffectColorGradient cg, BDSZ_EffectBaseEditor.SetColorGradientFuncion fun)
        {
            if (s_Instance == null)
            {
                s_Instance = ScriptableWizard.DisplayWizard<BDSZ_EffectColorGradientEditor>("Editor Gradient");
            }
            s_Instance.Init(eb, cg, fun);

        }
    }
}