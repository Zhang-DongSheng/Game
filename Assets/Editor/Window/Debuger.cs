using Game.Test;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Window
{
    class Debuger : CustomWindow
    {
        private Vector2 scrolllog = new Vector2(0, 0);

        private readonly List<string> parameter = new List<string>();

        private readonly List<LogInformation> logs = new List<LogInformation>();

        [MenuItem("Window/Analysis/Debuger #F4", false, -1)]
        protected static void Open()
        {
            Open<Debuger>("代码调试");
        }

        protected override void Initialise()
        {

        }

        protected override void Refresh()
        {
            GUILayout.BeginVertical(GUILayout.Height(Screen.height - 300));
            {
                scroll = GUILayout.BeginScrollView(scroll);
                {
                    RefreshParameter("Count:", parameter);
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            {
                scrolllog = GUILayout.BeginScrollView(scrolllog);
                {
                    for (int i = 0; i < logs.Count; i++)
                    {
                        RefreshLog(logs[i]);
                    }
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal(GUILayout.Height(42));
            {
                GUILayout.Space(3);

                if (GUILayout.Button("清除参数列表", GUILayout.ExpandHeight(true)))
                {
                    parameter.Clear();
                }
                if (GUILayout.Button("打开测试代码", GUILayout.ExpandHeight(true)))
                {
                    EditorUtility.OpenWithDefaultApp(string.Format("{0}/Scripts/Test/Test.cs", Application.dataPath));
                }
                GUILayout.Space(3);
            }
            GUILayout.EndHorizontal();

            if (GUILayout.Button(ToLanguage("Test"), GUILayout.Height(36)))
            {
                logs.Clear();

                Application.logMessageReceived += LogMessageReceiver;

                Test.Startover(parameter.ToArray());

                Application.logMessageReceived -= LogMessageReceiver;
            }
        }

        private void RefreshParameter(string key, IList list)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(key, GUILayout.Width(50));

                if (int.TryParse(GUILayout.TextField(list.Count.ToString()), out int count))
                {
                    count = Mathf.Clamp(count, 0, 30);
                }
                else
                {
                    count = 0;
                }
                // 删除
                if (GUILayout.Button("-", GUILayout.Width(30)))
                {
                    count--;
                }
                // 添加
                if (GUILayout.Button("+", GUILayout.Width(30)))
                {
                    count++;
                }
                // 刷新
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

        private void RefreshLog(LogInformation log)
        {
            GUILayout.BeginHorizontal();
            {
                EditorGUILayout.HelpBox(log.message, log.MessageType);
            }
            GUILayout.EndHorizontal();
        }

        private void LogMessageReceiver(string message, string source, LogType type)
        {
            logs.Add(new LogInformation(type, message, source));
        }
    }
}