using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TEST
{
    public class TestWindow : EditorWindow
    {
        private readonly List<string> parameter = new List<string>();

        private Vector2 scroll;

        [MenuItem("Extra/Test")]
        private static void Open()
        {
            EditorWindow window = GetWindow<TestWindow>();
            window.titleContent = new GUIContent("测试窗口");
            window.minSize = Vector2.one * 300;
            window.maxSize = Vector2.one * 1000;
            window.Show();
        }

        private void OnGUI()
        {
            RefreshUI();
        }

        private void RefreshUI()
        {
            GUILayout.BeginVertical(GUILayout.Height(Screen.height - 64));
            {
                scroll = GUILayout.BeginScrollView(scroll);
                {
                    RefreshParameter("参数：", parameter);
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();

            if (GUILayout.Button("启动", GUILayout.Height(36)))
            {
                Startup();
            }
        }

        private void RefreshParameter(string key, IList list)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(key, GUILayout.Width(50));

                if (int.TryParse(GUILayout.TextField(list.Count.ToString()), out int count))
                {
                    if (list.Count != count)
                    {
                        if (list.Count < count)
                        {
                            while (list.Count < count)
                            {
                                list.Add(string.Empty);
                            }
                        }
                        else
                        {
                            while (list.Count > count)
                            {
                                list.RemoveAt(list.Count - 1);
                            }
                        }
                    }
                }
            }
            GUILayout.EndHorizontal();

            for (int i = 0; i < list.Count; i++)
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Space(20);

                    GUILayout.Label(i.ToString(), GUILayout.Width(20));

                    list[i] = GUILayout.TextField(list[i].ToString());
                }
                GUILayout.EndHorizontal();
            }
        }

        private void Startup()
        { 
            
        }
    }
}
