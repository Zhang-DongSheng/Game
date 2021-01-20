 

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AnimatedValues;
namespace BDSZ_2020
{
    [CustomPropertyDrawer(typeof(BDSZ_EffectEmitter), true)]
    public class BDSZ_EffectEmitterDrawer : PropertyDrawer
    {


        SerializedProperty m_EmitterType;
        GUIContent m_EmitterTypeLabel;

        SerializedProperty m_bInnerEmitter;
        GUIContent m_bInnerEmitterLabel;
        SerializedProperty m_bRandDir;
        GUIContent m_bRandDirLabel;


        SerializedProperty m_fEmitterRatio;
        GUIContent m_fEmitterRatioLabel;
        SerializedProperty m_iEmitterMax;
        GUIContent m_iEmitterMaxLabel;

        SerializedProperty m_fEmitterCycle;
        GUIContent m_fEmitterCycleLabel;

        SerializedProperty m_vExtent;
        GUIContent m_vExtentLabel;

        SerializedProperty m_fSphereInnerRadius;
        GUIContent m_fSphereInnerRadiusLabel;
        SerializedProperty m_fSphereOuterRadius;
        GUIContent m_fSphereOuterRadiusLabel;


        SerializedProperty m_fConeAngle;
        GUIContent m_fConeAngleLabel;

       

        SerializedProperty m_fEmiterDirAngle;
        GUIContent m_fEmiterDirAngleLabel;
        

        protected void Init(SerializedProperty property)
        {
     

            m_EmitterType = property.FindPropertyRelative("m_EmitterType");
            m_EmitterTypeLabel = new GUIContent("EmitterType");
            m_bInnerEmitter = property.FindPropertyRelative("m_bInnerEmitter");
            m_bInnerEmitterLabel = new GUIContent("Inner Emitter");
            m_bRandDir = property.FindPropertyRelative("m_bRandDir");
            m_bRandDirLabel = new GUIContent("Rand Dir");


            m_fEmitterRatio = property.FindPropertyRelative("m_fEmitterRatio");
            m_fEmitterRatioLabel = new GUIContent("EmitterRatio");
            m_iEmitterMax = property.FindPropertyRelative("m_iEmitterMax");
            m_iEmitterMaxLabel = new GUIContent("MaxCount");

            m_fEmitterCycle = property.FindPropertyRelative("m_fEmitterCycle");
            m_fEmitterCycleLabel = new GUIContent("Cycle");
            m_vExtent = property.FindPropertyRelative("m_vExtent");
            m_vExtentLabel = new GUIContent("Extent");

            m_fSphereInnerRadius = property.FindPropertyRelative("m_fSphereInnerRadius");
            m_fSphereInnerRadiusLabel = new GUIContent("Radius Inner");
            m_fSphereOuterRadius = property.FindPropertyRelative("m_fSphereOuterRadius");
            m_fSphereOuterRadiusLabel = new GUIContent("Radius Outer");

            m_fConeAngle = property.FindPropertyRelative("m_fConeAngle");
            m_fConeAngleLabel = new GUIContent("Cone Angle");



            m_fEmiterDirAngle = property.FindPropertyRelative("m_fEmiterDirAngle");
            m_fEmiterDirAngleLabel = new GUIContent("Dir Angle");
            
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            Init(property);
            var height = EditorGUIUtility.singleLineHeight * 7 + EditorGUIUtility.standardVerticalSpacing * 8;
            if (m_EmitterType.intValue == (int)EEmitterType.Circle)
                height += EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing * 3;
            else if (m_EmitterType.intValue == (int)EEmitterType.Cone)
                height += EditorGUIUtility.singleLineHeight * 1 + EditorGUIUtility.standardVerticalSpacing * 2;
            else if (m_EmitterType.intValue == (int)EEmitterType.Rect)
                height += EditorGUIUtility.singleLineHeight * 1 + EditorGUIUtility.standardVerticalSpacing * 2;

            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            Init(property);

            Rect rect = position;
            rect.height = EditorGUIUtility.singleLineHeight;

            int type = m_EmitterType.intValue;

            EditorGUI.PropertyField(rect, m_EmitterType, m_EmitterTypeLabel);
            rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;

            EditorGUI.PropertyField(rect, m_fEmiterDirAngle, m_fEmiterDirAngleLabel);
            rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;

           

            EditorGUI.PropertyField(rect, m_bInnerEmitter, m_bInnerEmitterLabel);
            rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;
            if (type == (int)EEmitterType.Circle)
            {
                EditorGUI.PropertyField(rect, m_bRandDir, m_bRandDirLabel);
                rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;
            }

            EditorGUI.PropertyField(rect, m_fEmitterRatio, m_fEmitterRatioLabel);
            rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(rect, m_iEmitterMax, m_iEmitterMaxLabel);
            rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;

            EditorGUI.PropertyField(rect, m_fEmitterCycle, m_fEmitterCycleLabel);
            rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;
            if (type != (int)EEmitterType.Circle)
            {
                EditorGUI.PropertyField(rect, m_vExtent, m_vExtentLabel);
                rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;
            }
            if (type == (int)EEmitterType.Circle)
            {
                EditorGUI.PropertyField(rect, m_fSphereInnerRadius,  m_fSphereInnerRadiusLabel);
                rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;

                EditorGUI.PropertyField(rect, m_fSphereOuterRadius, m_fSphereOuterRadiusLabel);
                rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;
            }
            else if (type == (int)EEmitterType.Cone)
            {

                EditorGUI.PropertyField(rect, m_fConeAngle, m_fConeAngleLabel);
                rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;
            }
            

        }
    }

}