using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
namespace BDSZ_2020
{
    [CustomEditor(typeof(BDSZ_EffectBehaviour), true)]
    public class BDSZ_EffectBehaviourEditor : Editor
    {
        public delegate void SetColorGradientFuncion(EffectColorGradient userData);
        class ColorGradientModeCallbackData
        {
            public SetColorGradientFuncion Callback;
            public EffectColorGradient Value;
            public int selectedState = 0;

            public ColorGradientModeCallbackData(SetColorGradientFuncion bp, EffectColorGradient value, int index)
            {
                this.Callback = bp;
                this.Value = value;
                this.selectedState = index;
            }
        }
        void EffectColorGradientMenuCallback(object userData)
        {
            ColorGradientModeCallbackData gd = userData as ColorGradientModeCallbackData;

            gd.Value.mode = (EEffectValueGradient)gd.selectedState;
            gd.Callback(gd.Value);
        }
        public void EffectColorGradientValuePop(SetColorGradientFuncion callback, EffectColorGradient cg)
        {
            if (GUILayout.Button("", "StaticDropdown", GUILayout.Width(16f), GUILayout.Height(16f)))
            {
                GUIContent[] array = new GUIContent[]
                    {
                    new GUIContent("Constant"),
                    new GUIContent("Gradient Between Two"),
                    new GUIContent("Gradient Between Three"),
                    new GUIContent("Gradient"),
                    };
                GenericMenu genericMenu = new GenericMenu();
                int iSelect = (int)cg.mode;
                for (int i = 0; i < array.Length; i++)
                {
                    genericMenu.AddItem(array[i], i == iSelect, EffectColorGradientMenuCallback, new ColorGradientModeCallbackData(callback, cg, i));
                }
                genericMenu.ShowAsContext();

            }
        }

        public void ColorGradientGUI(BDSZ_EffectBehaviour ef, string strLabel, EffectColorGradient cg, SetColorGradientFuncion callback)
        {

            GUILayout.BeginHorizontal();
            GUILayout.Label(strLabel, GUILayout.Width(EditorGUIUtility.labelWidth));

            if (cg.mode == EEffectValueGradient.Constant || cg.mode == EEffectValueGradient.GradientTwo || cg.mode == EEffectValueGradient.GradientThree)
            {
                GUI.changed = false;
                Color clrEdit = EditorGUILayout.ColorField("", cg.MinValue, GUILayout.MinWidth(30f));
                if (GUI.changed)
                {
                    cg.MinValue = clrEdit;
                    callback(cg);
                    BDSZ_EditorGUIUtility.RegisterUndo(ef.name + strLabel + "0", ef);
                    ef.OnValidate();
                }
                if (cg.mode == EEffectValueGradient.GradientTwo || cg.mode == EEffectValueGradient.GradientThree)
                {
                    GUI.changed = false;
                    clrEdit = EditorGUILayout.ColorField("", cg.MaxValue, GUILayout.MinWidth(30f));
                    if (GUI.changed)
                    {
                        cg.MaxValue = clrEdit;
                        callback(cg);
                        BDSZ_EditorGUIUtility.RegisterUndo(ef.name + strLabel + "1", ef);
                        ef.OnValidate();
                    }
                }
                if (cg.mode == EEffectValueGradient.GradientThree)
                {
                    GUI.changed = false;
                    clrEdit = EditorGUILayout.ColorField("", cg.m_Key1Value, GUILayout.MinWidth(30f));
                    if (GUI.changed)
                    {
                        cg.m_Key1Value = clrEdit;
                        callback(cg);
                        BDSZ_EditorGUIUtility.RegisterUndo(ef.name + strLabel + "2", ef);
                        ef.OnValidate();
                    }
                }
            }
            else
            {
                if (EditorGUILayout.DropdownButton(m_GradientLabel, FocusType.Passive, GUILayout.MinWidth(30f)))
                {
                    BDSZ_EffectColorGradientEditor.Show(ef, cg, callback);
                }
            }
            EffectColorGradientValuePop(callback, cg);
            GUILayout.EndHorizontal();
            GUILayout.Space(4);
        }
        class ColorMinMaxModeCallbackData
        {
            public SetColorMinMaxFuncion Callback;
            public EffectColorMinMax Value;
            public int selectedState = 0;
            public ColorMinMaxModeCallbackData(SetColorMinMaxFuncion bp, EffectColorMinMax value, int index)
            {
                this.Callback = bp;
                this.Value = value;
                this.selectedState = index;
            }
        }
        void EffectColorMinMaxMenuCallback(object userData)
        {
            ColorMinMaxModeCallbackData gd = userData as ColorMinMaxModeCallbackData;
            gd.Value.mode = (EEffectValueMinMax)gd.selectedState;
            gd.Callback(gd.Value);
        }
        public void EffectColorMinMaxValuePop(SetColorMinMaxFuncion callback, EffectColorMinMax cg)
        {
            if (GUILayout.Button("", "StaticDropdown", GUILayout.Width(16f), GUILayout.Height(16f)))
            {
                GUIContent[] array = new GUIContent[]
                    {
                    new GUIContent("Constant"),
                    new GUIContent("Rand Between Two"),

                    };
                GenericMenu genericMenu = new GenericMenu();
                int iSelect = (int)cg.mode;
                for (int i = 0; i < array.Length; i++)
                {
                    genericMenu.AddItem(array[i], i == iSelect, EffectColorMinMaxMenuCallback, new ColorMinMaxModeCallbackData(callback, cg, i));
                }
                genericMenu.ShowAsContext();

            }
        }

        public delegate void SetColorMinMaxFuncion(EffectColorMinMax userData);
        public void ColorMinMaxGUI(BDSZ_EffectBehaviour ef, string strLabel, EffectColorMinMax cg, SetColorMinMaxFuncion callback)
        {

            GUILayout.BeginHorizontal();
            GUILayout.Label(strLabel, GUILayout.Width(EditorGUIUtility.labelWidth));
            BDSZ_EditorGUIUtility.SetLabelWidth(16f);
            GUI.changed = false;
            Color clrEdit = EditorGUILayout.ColorField("", cg.MinValue, GUILayout.MinWidth(30f));
            if (GUI.changed)
            {
                cg.MinValue = clrEdit;
                callback(cg);
                BDSZ_EditorGUIUtility.RegisterUndo(ef.name + strLabel + "0", ef);
                ef.OnValidate();
            }
            if (cg.mode == EEffectValueMinMax.RandTwo)
            {
                GUI.changed = false;
                clrEdit = EditorGUILayout.ColorField("", cg.MaxValue, GUILayout.MinWidth(30f));
                if (GUI.changed)
                {
                    cg.MaxValue = clrEdit;
                    callback(cg);
                    BDSZ_EditorGUIUtility.RegisterUndo(ef.name + strLabel + "1", ef);
                    ef.OnValidate();
                }
            }
            BDSZ_EditorGUIUtility.RestoreLabelWidth();
            EffectColorMinMaxValuePop(callback, cg);
            GUILayout.EndHorizontal();
            GUILayout.Space(4);
        }
        public delegate void SetFloatMinMaxFuncion(EffectFloatMinMax userData);

        public void FloatMinMaxGUI(BDSZ_EffectBehaviour ef, string strLabel, EffectFloatMinMax cg, SetFloatMinMaxFuncion callback)
        {

            GUILayout.BeginHorizontal();
            GUILayout.Label(strLabel, GUILayout.Width(EditorGUIUtility.labelWidth));
            BDSZ_EditorGUIUtility.SetLabelWidth(16f);
            GUI.changed = false;
            float clrEdit = EditorGUILayout.FloatField("", cg.MinValue, GUILayout.MinWidth(30f));
            if (GUI.changed)
            {
                cg.MinValue = clrEdit;
                callback(cg);
                BDSZ_EditorGUIUtility.RegisterUndo(ef.name + strLabel + "0", ef);
                ef.OnValidate();
            }
            if (cg.mode == EEffectValueMinMax.RandTwo)
            {
                GUI.changed = false;
                clrEdit = EditorGUILayout.FloatField("", cg.MaxValue, GUILayout.MinWidth(30f));
                if (GUI.changed)
                {
                    cg.MaxValue = clrEdit;
                    callback(cg);
                    BDSZ_EditorGUIUtility.RegisterUndo(ef.name + strLabel + "1", ef);
                    ef.OnValidate();
                }
            }
            BDSZ_EditorGUIUtility.RestoreLabelWidth();
            EffectFloatMinMaxValuePop(callback, cg);
            GUILayout.EndHorizontal();
            GUILayout.Space(4);
        }
        public void FloatGradientGUI(BDSZ_EffectBehaviour ef, string strLabel, EffectFloatGradient cg, SetFloatGradientFuncion callback)
        {

            GUILayout.BeginHorizontal();
            GUILayout.Label(strLabel, GUILayout.Width(EditorGUIUtility.labelWidth));

            if (cg.mode == EEffectValueGradient.Constant || cg.mode == EEffectValueGradient.GradientTwo || cg.mode == EEffectValueGradient.GradientThree)
            {
                BDSZ_EditorGUIUtility.SetLabelWidth(16f);

                GUI.changed = false;
                float clrEdit = EditorGUILayout.FloatField("", cg.MinValue, GUILayout.MinWidth(30f));
                if (GUI.changed)
                {
                    cg.MinValue = clrEdit;
                    callback(cg);
                    BDSZ_EditorGUIUtility.RegisterUndo(ef.name + strLabel + "0", ef);
                    ef.OnValidate();
                }
                if (cg.mode == EEffectValueGradient.GradientTwo || cg.mode == EEffectValueGradient.GradientThree)
                {
                    GUI.changed = false;
                    clrEdit = EditorGUILayout.FloatField("", cg.MaxValue, GUILayout.MinWidth(30f));
                    if (GUI.changed)
                    {
                        cg.MaxValue = clrEdit;
                        callback(cg);
                        BDSZ_EditorGUIUtility.RegisterUndo(ef.name + strLabel + "1", ef);
                        ef.OnValidate();
                    }
                }
                if (cg.mode == EEffectValueGradient.GradientThree)
                {
                    GUI.changed = false;
                    clrEdit = EditorGUILayout.FloatField("", cg.m_Key1Value, GUILayout.MinWidth(30f));
                    if (GUI.changed)
                    {
                        cg.m_Key1Value = clrEdit;
                        callback(cg);
                        BDSZ_EditorGUIUtility.RegisterUndo(ef.name + strLabel + "2", ef);
                        ef.OnValidate();
                    }
                }
                BDSZ_EditorGUIUtility.RestoreLabelWidth();
            }
            else
            {

                if (EditorGUILayout.DropdownButton(m_GradientLabel, FocusType.Passive, GUILayout.MinWidth(30f)))
                {
                    BDSZ_EffectFloatGradientEditor.Show(ef, cg, callback);
                }
            }
            EffectFloatGradientValuePop(callback, cg);
            GUILayout.EndHorizontal();
            GUILayout.Space(4);

        }


        class FloatMinMaxModeCallbackData
        {
            public SetFloatMinMaxFuncion Callback;
            public EffectFloatMinMax Value;
            public int selectedState = 0;

            public FloatMinMaxModeCallbackData(SetFloatMinMaxFuncion bp, EffectFloatMinMax value, int index)
            {
                this.Callback = bp;
                this.Value = value;
                this.selectedState = index;
            }
        }
        void EffectFloatMinMaxMenuCallback(object userData)
        {
            FloatMinMaxModeCallbackData gd = userData as FloatMinMaxModeCallbackData;
            gd.Value.mode = (EEffectValueMinMax)gd.selectedState;
            gd.Callback(gd.Value);
        }
        public void EffectFloatMinMaxValuePop(SetFloatMinMaxFuncion callback, EffectFloatMinMax cg)
        {
            if (GUILayout.Button("", "StaticDropdown", GUILayout.Width(16f), GUILayout.Height(16f)))
            {
                GUIContent[] array = new GUIContent[]
                    {
                    new GUIContent("Constant"),
                    new GUIContent("Rand Between Two"),

                    };
                GenericMenu genericMenu = new GenericMenu();
                int iSelect = (int)cg.mode;
                for (int i = 0; i < array.Length; i++)
                {
                    genericMenu.AddItem(array[i], i == iSelect, EffectFloatMinMaxMenuCallback, new FloatMinMaxModeCallbackData(callback, cg, i));
                }
                genericMenu.ShowAsContext();

            }
        }

        public delegate void SetFloatGradientFuncion(EffectFloatGradient userData);
        class FloatGradientModeCallbackData
        {
            public SetFloatGradientFuncion Callback;
            public EffectFloatGradient Value;
            public int selectedState = 0;

            public FloatGradientModeCallbackData(SetFloatGradientFuncion bp, EffectFloatGradient value, int index)
            {
                this.Callback = bp;
                this.Value = value;
                this.selectedState = index;
            }
        }



        void EffectFloatGradientMenuCallback(object userData)
        {
            FloatGradientModeCallbackData gd = userData as FloatGradientModeCallbackData;
            gd.Value.mode = (EEffectValueGradient)gd.selectedState;
            gd.Callback(gd.Value);
        }

        public void EffectFloatGradientValuePop(SetFloatGradientFuncion callback, EffectFloatGradient cg)
        {
            if (GUILayout.Button("", "StaticDropdown", GUILayout.Width(16f), GUILayout.Height(16f)))
            {
                GUIContent[] array = new GUIContent[]
                    {
                    new GUIContent("Constant"),
                    new GUIContent("Gradient Between Two"),
                    new GUIContent("Gradient Betwwen Three"),
                    new GUIContent("Gradient"),
                    };
                GenericMenu genericMenu = new GenericMenu();
                int iSelect = (int)cg.mode;
                for (int i = 0; i < array.Length; i++)
                {
                    genericMenu.AddItem(array[i], i == iSelect, EffectFloatGradientMenuCallback, new FloatGradientModeCallbackData(callback, cg, i));
                }
                genericMenu.ShowAsContext();

            }
        }
        GUIContent m_GradientLabel = new GUIContent("Gradient");

    }
}