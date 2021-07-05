using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Window
{
    class Debuger : CustomWindow
    {
        private readonly List<string> parameter = new List<string>();

        protected override string Title { get { return "调试工具"; } }
        [MenuItem("Script/Debuger")]
        protected static void Open()
        {
            Open<Debuger>();
        }

        protected override void Init() { }

        protected override void Refresh()
        {
            GUILayout.BeginVertical(GUILayout.Height(Screen.height - 100));
            {
                scroll = GUILayout.BeginScrollView(scroll);
                {
                    RefreshParameter("Count:", parameter);
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("清除参数列表", GUILayout.ExpandHeight(true)))
                {
                    parameter.Clear();
                }
                if (GUILayout.Button("打开测试代码", GUILayout.ExpandHeight(true)))
                {
                    EditorUtility.OpenWithDefaultApp(string.Format("{0}/Script/Test/Test.cs", Application.dataPath));
                }
            }
            GUILayout.EndHorizontal();

            if (GUILayout.Button("测试", GUILayout.Height(36)))
            {
                TEST.Test.Startover(parameter.ToArray());
            }
        }

        private void RefreshParameter(string key, IList list)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(key, GUILayout.Width(50));

                if (!int.TryParse(GUILayout.TextField(list.Count.ToString()), out int count))
                {
                    count = 0;
                }

                if (GUILayout.Button("-", GUILayout.Width(30)))
                {
                    count--;
                }

                if (GUILayout.Button("+", GUILayout.Width(30)))
                {
                    count++;
                }

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
            GUILayout.EndHorizontal();

            for (int i = 0; i < list.Count; i++)
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Space(10);

                    GUILayout.Label(string.Format("※{0}", i), GUILayout.Width(30));

                    list[i] = GUILayout.TextField(list[i].ToString());
                }
                GUILayout.EndHorizontal();
            }
        }
    }
}