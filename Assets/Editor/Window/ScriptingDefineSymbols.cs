using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Window
{
    public class ScriptingDefineSymbols : CustomWindow
    {
        private readonly List<string> defines = new List<string>();

        [MenuItem("Tools/�궨��")]
        protected static void Open()
        {
            Open<ScriptingDefineSymbols>("�궨��");
        }

        protected override void Init()
        {
            string content = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);

            defines.Clear();

            if (string.IsNullOrEmpty(content))
            {
                ShowNotification("��ǰƽ̨�궨��Ϊ��!");
            }
            else
            {
                defines.AddRange(content.Split(';'));
            }
        }

        protected override void Refresh()
        {
            GUILayout.BeginVertical();
            {
                scroll = GUILayout.BeginScrollView(scroll);
                {
                    for (int i = 0; i < defines.Count; i++)
                    {
                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.Label(defines[i]);

                            if (GUILayout.Button("�Ƴ�", GUILayout.Width(100)))
                            {
                                Remove(i);
                            }
                        }
                        GUILayout.EndHorizontal();
                    }
                }
                GUILayout.EndScrollView();

                GUILayout.BeginHorizontal();
                {
                    input.value = GUILayout.TextField(input.value);

                    if (GUILayout.Button("���", GUILayout.Width(100)))
                    {
                        Add(input.value);
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();

            if (GUILayout.Button("����"))
            {
                Save();
            }
        }

        private void Add(string value)
        {
            if (string.IsNullOrEmpty(value)) return;

            if (defines.Contains(value)) return;

            defines.Add(value);
        }

        private void Remove(int index)
        {
            if (defines.Count > index)
            {
                defines.RemoveAt(index);
            }
        }

        private void Save()
        {
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup,
                string.Join(";", defines.ToArray()));
        }
    }
}