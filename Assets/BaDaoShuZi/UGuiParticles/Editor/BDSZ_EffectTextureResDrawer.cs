
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AnimatedValues;
namespace BDSZ_2020
{
    [CustomPropertyDrawer(typeof(BDSZ_EffectTextureRes), true)]
    public class BDSZ_EffectTextureResDrawer : PropertyDrawer
    {

        SerializedProperty m_Texture;
        GUIContent m_TextureLabel;
        SerializedProperty m_bGrid;
        GUIContent m_bGuidLabel;
        SerializedProperty m_iTileX;
        SerializedProperty m_iTileY;
        SerializedProperty m_iTileIndex;
        SerializedProperty m_fInterval;

        SerializedProperty m_fXVelocity;
        SerializedProperty m_fYVelocity;
        SerializedProperty m_Flip;
        GUIContent m_FlipLabel;
        GUIContent m_iTileXLabel;
        GUIContent m_iTileYLabel;
        GUIContent m_iTileIndexLabel;
        GUIContent m_fIntervalLabel;

        GUIContent m_fXVelocityLabel;
        GUIContent m_fYVelocityLabel;

        SerializedProperty m_fRollVelocity;
        GUIContent m_fRollVelocityLabel;
        protected void Init(SerializedProperty property)
        {

            m_Texture = property.FindPropertyRelative("m_Texture");
            m_TextureLabel = new GUIContent("Texture");
            m_bGrid = property.FindPropertyRelative("m_bGrid");
            m_bGuidLabel = new GUIContent("Grid");
            m_iTileX = property.FindPropertyRelative("m_iTileX");
            m_iTileY = property.FindPropertyRelative("m_iTileY");
            m_iTileIndex = property.FindPropertyRelative("m_iTileIndex");
            m_fInterval = property.FindPropertyRelative("m_fInterval");

            m_fXVelocity = property.FindPropertyRelative("m_fXVelocity");
            m_fYVelocity = property.FindPropertyRelative("m_fYVelocity");


            m_iTileXLabel = new GUIContent("TileX");
            m_iTileYLabel = new GUIContent("TileY");
            m_iTileIndexLabel = new GUIContent("TileIndex");
            m_fIntervalLabel = new GUIContent("Interval");

            m_fXVelocityLabel = new GUIContent("X Velocity");
            m_fYVelocityLabel = new GUIContent("Y Velocity");

            m_fRollVelocity = property.FindPropertyRelative("m_fRollVelocity");
            m_fRollVelocityLabel = new GUIContent("Roll Velocity");

            m_Flip = property.FindPropertyRelative("m_Flip");
            m_FlipLabel = new GUIContent("Flip");


        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            Init(property);
            var height = EditorGUIUtility.singleLineHeight * 3 + EditorGUIUtility.standardVerticalSpacing * 4;

            if (m_bGrid.boolValue == true)
                height += EditorGUIUtility.singleLineHeight * 4 + EditorGUIUtility.standardVerticalSpacing * 5;
            else
                height += EditorGUIUtility.singleLineHeight * 3 + EditorGUIUtility.standardVerticalSpacing * 4;
            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            Init(property);

            Rect rect = position;
            rect.height = EditorGUIUtility.singleLineHeight;
         
            EditorGUI.PropertyField(rect, m_Texture, m_TextureLabel);
             
            rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;

            EditorGUI.PropertyField(rect, m_Flip, m_FlipLabel);
            rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;

            EditorGUI.PropertyField(rect, m_bGrid, m_bGuidLabel);
            rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;

            if (m_bGrid.boolValue == true)
            {
                EditorGUI.PropertyField(rect, m_iTileX, m_iTileXLabel);
                rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;

                EditorGUI.PropertyField(rect, m_iTileY, m_iTileYLabel);
                rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;

                EditorGUI.PropertyField(rect, m_iTileIndex, m_iTileIndexLabel);
                rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;

                EditorGUI.PropertyField(rect, m_fInterval, m_fIntervalLabel);
                rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;
            }
            else
            {
                EditorGUI.PropertyField(rect, m_fXVelocity, m_fXVelocityLabel);
                rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;

                EditorGUI.PropertyField(rect, m_fYVelocity, m_fYVelocityLabel);
                rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(rect, m_fRollVelocity, m_fRollVelocityLabel);
                rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;
            }
        }

    }
}