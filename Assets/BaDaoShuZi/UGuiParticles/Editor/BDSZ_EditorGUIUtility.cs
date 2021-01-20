using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

namespace BDSZ_2020
{
   
    public static class BDSZ_EditorGUIUtility
    {
      
        static public bool DrawPrefixButton(string text)
        {
            return GUILayout.Button(text, "DropDown", GUILayout.Width(76f));
        }

        static public bool DrawPrefixButton(string text, params GUILayoutOption[] options)
        {
            return GUILayout.Button(text, "DropDown", options);
        }

        static public int DrawPrefixList(int index, string[] list, params GUILayoutOption[] options)
        {
            return EditorGUILayout.Popup(index, list, "DropDown", options);
        }

        static public int DrawPrefixList(string text, int index, string[] list, params GUILayoutOption[] options)
        {
            return EditorGUILayout.Popup(text, index, list, "DropDown", options);
        }


        static public bool DrawMinimalisticHeader(string text) { return DrawHeader(text, text, false, true); }

        static public bool DrawHeader(string text) { return DrawHeader(text, text, false,false); }

        static public bool DrawHeader(string text, string key) { return DrawHeader(text, key, false, false); }


        static public bool DrawHeader(string text, bool detailed) { return DrawHeader(text, text, detailed, !detailed); }

        static public bool DrawHeader(string text, string key, bool forceOn, bool minimalistic)
        {
            bool state = EditorPrefs.GetBool(key, true);

            if (!minimalistic) GUILayout.Space(3f);
            if (!forceOn && !state) GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f);
            GUILayout.BeginHorizontal();
            GUI.changed = false;

            if (minimalistic)
            {
                if (state) text = "\u25BC" + (char)0x200a + text;
                else text = "\u25BA" + (char)0x200a + text;

                GUILayout.BeginHorizontal();
                GUI.contentColor = EditorGUIUtility.isProSkin ? new Color(1f, 1f, 1f, 0.7f) : new Color(0f, 0f, 0f, 0.7f);
                if (!GUILayout.Toggle(true, text, "PreToolbar2", GUILayout.MinWidth(20f))) state = !state;
                GUI.contentColor = Color.white;
                GUILayout.EndHorizontal();
            }
            else
            {
                text = "<b><size=11>" + text + "</size></b>";
                if (state) text = "\u25BC " + text;
                else text = "\u25BA " + text;
                if (!GUILayout.Toggle(true, text, "dragtab", GUILayout.MinWidth(20f))) state = !state;
            }

            if (GUI.changed) EditorPrefs.SetBool(key, state);

            if (!minimalistic) GUILayout.Space(2f);
            GUILayout.EndHorizontal();
            GUI.backgroundColor = Color.white;
            if (!forceOn && !state) GUILayout.Space(3f);
            return state;
        }

        static public void BeginContents() { BeginContents(false); }

        static bool mEndHorizontal = false;


        static public void BeginContents(bool minimalistic)
        {
            if (!minimalistic)
            {
                mEndHorizontal = true;
                GUILayout.BeginHorizontal();
                
                //EditorGUILayout.BeginHorizontal("AS TextArea", GUILayout.MinHeight(10f));
                EditorGUILayout.BeginHorizontal("TabWindowBackground", GUILayout.MinHeight(10f));
            }
            else
            {
                mEndHorizontal = false;
                EditorGUILayout.BeginHorizontal(GUILayout.MinHeight(10f));
                GUILayout.Space(10f);
            }
            GUILayout.BeginVertical();
            GUILayout.Space(2f);
        }

        static public void EndContents()
        {
            GUILayout.Space(3f);
            GUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            if (mEndHorizontal)
            {
                GUILayout.Space(3f);
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(3f);
        }


        static public SerializedProperty DrawProperty(SerializedObject serializedObject, string property, params GUILayoutOption[] options)
        {
            return DrawProperty(null, serializedObject, property, false, options);
        }

        static public SerializedProperty DrawProperty(string label, SerializedObject serializedObject, string property, params GUILayoutOption[] options)
        {
            return DrawProperty(label, serializedObject, property, false, options);
        }

        static public SerializedProperty DrawPaddedProperty(SerializedObject serializedObject, string property, params GUILayoutOption[] options)
        {
            return DrawProperty(null, serializedObject, property, true, options);
        }

        static public SerializedProperty DrawPaddedProperty(string label, SerializedObject serializedObject, string property, params GUILayoutOption[] options)
        {
            return DrawProperty(label, serializedObject, property, true, options);
        }



        static public SerializedProperty DrawProperty(string label, SerializedObject serializedObject, string property, bool padding, params GUILayoutOption[] options)
        {
            SerializedProperty sp = serializedObject.FindProperty(property);

            if (sp != null)
            {
                

                if (padding) EditorGUILayout.BeginHorizontal();

                if (label != null) EditorGUILayout.PropertyField(sp, new GUIContent(label), options);
                else EditorGUILayout.PropertyField(sp, options);

                if (padding)
                {
                    BDSZ_EditorGUIUtility.DrawPadding();
                    EditorGUILayout.EndHorizontal();
                }
            }
            return sp;
        }

        static public void DrawProperty(string label, SerializedProperty sp, params GUILayoutOption[] options)
        {
            DrawProperty(label, sp, true, options);
        }


        static public void DrawProperty(string label, SerializedProperty sp, bool padding, params GUILayoutOption[] options)
        {
            if (sp != null)
            {
                if (padding) EditorGUILayout.BeginHorizontal();

                if (label != null) EditorGUILayout.PropertyField(sp, new GUIContent(label), options);
                else EditorGUILayout.PropertyField(sp, options);

                if (padding)
                {
                    BDSZ_EditorGUIUtility.DrawPadding();
                    EditorGUILayout.EndHorizontal();
                }
            }
        }
        static public void DrawBorderProperty(string name, SerializedObject serializedObject, string field)
        {
            if (serializedObject.FindProperty(field) != null)
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(name, GUILayout.Width(75f));

                    BDSZ_EditorGUIUtility.SetLabelWidth(50f);
                    GUILayout.BeginVertical();
                    BDSZ_EditorGUIUtility.DrawProperty("Left", serializedObject, field + ".x", GUILayout.MinWidth(80f));
                    BDSZ_EditorGUIUtility.DrawProperty("Bottom", serializedObject, field + ".y", GUILayout.MinWidth(80f));
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical();
                    BDSZ_EditorGUIUtility.DrawProperty("Right", serializedObject, field + ".z", GUILayout.MinWidth(80f));
                    BDSZ_EditorGUIUtility.DrawProperty("Top", serializedObject, field + ".w", GUILayout.MinWidth(80f));
                    GUILayout.EndVertical();

                    BDSZ_EditorGUIUtility.RestoreLabelWidth();
                }
                GUILayout.EndHorizontal();
            }
        }

        static public void DrawSizeProperty(string name, SerializedObject serializedObject, string field)
        {
            DrawSizeProperty(name, serializedObject, field, EditorGUIUtility.labelWidth,4.0f);
        }
        static public void DrawOffsetProperty(string name, SerializedObject serializedObject, string field, float labelWidth, float spacing)
        {
            if (serializedObject.FindProperty(field) != null)
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(name, GUILayout.Width(labelWidth));

                    BDSZ_EditorGUIUtility.SetLabelWidth(30f);
                    //GUILayout.BeginVertical();
                    BDSZ_EditorGUIUtility.DrawProperty("X", serializedObject, field + ".x", GUILayout.MinWidth(26f));
                    BDSZ_EditorGUIUtility.DrawProperty("Y", serializedObject, field + ".y", GUILayout.MinWidth(26f));
                    //GUILayout.EndVertical();

                    BDSZ_EditorGUIUtility.RestoreLabelWidth();
                    if (spacing != 0f) GUILayout.Space(spacing);
                }
                GUILayout.EndHorizontal();
            }
        }
 

        static public void DrawSizeProperty(string name, SerializedObject serializedObject, string field, float labelWidth, float spacing)
        {
            if (serializedObject.FindProperty(field) != null)
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(name, GUILayout.Width(labelWidth));

                    BDSZ_EditorGUIUtility.SetLabelWidth(30f);
                    //GUILayout.BeginVertical();
                    BDSZ_EditorGUIUtility.DrawProperty("W", serializedObject, field + ".x", GUILayout.MinWidth(26f));
                    BDSZ_EditorGUIUtility.DrawProperty("H", serializedObject, field + ".y", GUILayout.MinWidth(26f));
                    //GUILayout.EndVertical();

                    BDSZ_EditorGUIUtility.RestoreLabelWidth();
                    if (spacing != 0f) GUILayout.Space(spacing);
                }
                GUILayout.EndHorizontal();
            }
        }

        static public void DrawRectProperty(string name, SerializedObject serializedObject, string field)
        {
            DrawRectProperty(name, serializedObject, field, 56f, 18f);
        }
		static public void DrawMarginProperty(string name, SerializedObject serializedObject, string field, float labelWidth, float spacing)
		{
			if (serializedObject.FindProperty(field) != null)
			{
				GUILayout.BeginHorizontal();
				{
					GUILayout.Label(name, GUILayout.Width(labelWidth));

					BDSZ_EditorGUIUtility.SetLabelWidth(20f);
					GUILayout.BeginVertical();
					BDSZ_EditorGUIUtility.DrawProperty("Left", serializedObject, field + ".xMin", GUILayout.MinWidth(50f));
					BDSZ_EditorGUIUtility.DrawProperty("Top", serializedObject, field + ".yMin", GUILayout.MinWidth(50f));
					GUILayout.EndVertical();

					BDSZ_EditorGUIUtility.SetLabelWidth(50f);
					GUILayout.BeginVertical();
					BDSZ_EditorGUIUtility.DrawProperty("Right", serializedObject, field + ".xMax", GUILayout.MinWidth(80f));
					BDSZ_EditorGUIUtility.DrawProperty("Bottom", serializedObject, field + ".yMax", GUILayout.MinWidth(80f));
					GUILayout.EndVertical();

					BDSZ_EditorGUIUtility.RestoreLabelWidth();
					if (spacing != 0f) GUILayout.Space(spacing);
				}
				GUILayout.EndHorizontal();
			}
		}

        static public void DrawRectProperty(string name, SerializedObject serializedObject, string field, float labelWidth, float spacing)
        {
            if (serializedObject.FindProperty(field) != null)
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(name, GUILayout.Width(labelWidth));

                    BDSZ_EditorGUIUtility.SetLabelWidth(20f);
                    GUILayout.BeginVertical();
                    BDSZ_EditorGUIUtility.DrawProperty("X", serializedObject, field + ".x", GUILayout.MinWidth(50f));
                    BDSZ_EditorGUIUtility.DrawProperty("Y", serializedObject, field + ".y", GUILayout.MinWidth(50f));
                    GUILayout.EndVertical();

                    BDSZ_EditorGUIUtility.SetLabelWidth(50f);
                    GUILayout.BeginVertical();
                    BDSZ_EditorGUIUtility.DrawProperty("Width", serializedObject, field + ".width", GUILayout.MinWidth(80f));
                    BDSZ_EditorGUIUtility.DrawProperty("Height", serializedObject, field + ".height", GUILayout.MinWidth(80f));
                    GUILayout.EndVertical();

                    BDSZ_EditorGUIUtility.RestoreLabelWidth();
                    if (spacing != 0f) GUILayout.Space(spacing);
                }
                GUILayout.EndHorizontal();
            }
        }

        static float s_fSaveLabelWidth = 0.0f; 
        static public void SetLabelWidth(float width)
        {
            s_fSaveLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = width;
        }
        static public void RestoreLabelWidth()
        {
            EditorGUIUtility.labelWidth = s_fSaveLabelWidth;
        }


        static public void RegisterUndo(string name, params Object[] objects)
        {
            if (objects != null && objects.Length > 0)
            {
                UnityEditor.Undo.RecordObjects(objects, name);

                foreach (Object obj in objects)
                {
                    if (obj == null) continue;
                    EditorUtility.SetDirty(obj);
                }
            }
        }


        static public void DoAligmentControl(SerializedProperty alignment, int iAlign1, int iAlign2, string strLeft, string strMiddle, string strRight)
        {

            bool leftAlign = (alignment.intValue & iAlign1) != 0;
            bool rightAlign = (alignment.intValue & iAlign2) != 0;
            bool centerAlign = (leftAlign == false && rightAlign == false);

            bool bNewLeftAlign = GUILayout.Toggle(leftAlign, strLeft, "ButtonLeft");
            if (bNewLeftAlign != leftAlign)
            {
                alignment.intValue = alignment.intValue & (~(iAlign1 | iAlign2));
                if (bNewLeftAlign)
                {
                    alignment.intValue |= iAlign1;
                }

            }


            bool bNewCenterAlign = GUILayout.Toggle(centerAlign, strMiddle, "ButtonMid");
            if (bNewCenterAlign != centerAlign)
            {
                if (bNewCenterAlign)
                {
                    alignment.intValue = alignment.intValue & (~(iAlign1 | iAlign2));
                }
            }


            bool bNewRightAlign = GUILayout.Toggle(rightAlign, strRight, "ButtonRight");
            if (bNewRightAlign != rightAlign)
            {
                alignment.intValue = alignment.intValue & (~(iAlign1 | iAlign2));
                if (bNewRightAlign)
                {
                    alignment.intValue |= iAlign2;
                }

            }
        }
        static public void DrawPadding()
        {
            GUILayout.Space(18f);
        }
    }
}