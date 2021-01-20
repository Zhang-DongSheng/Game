 
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
namespace BDSZ_2020
{
    [CustomEditor(typeof(BDSZ_UICurveBase), true)]

    public class BDSZ_UICurveBaseEditor : Editor
    {


        public static BDSZ_UICurveBaseEditor s_Instance = null;
         

        protected virtual void OnEnable()
        {
            s_Instance = this;
         

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
             
        }
    }
}