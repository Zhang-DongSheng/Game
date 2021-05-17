using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Window
{
    class Debuger : EditorWindow
    {
        private readonly List<string> parameter = new List<string>();

        private Vector2 scroll;

        [MenuItem("Script/Debuger")]
        private static void Open()
        {
            EditorWindow window = GetWindow<Debuger>();
            window.titleContent = new GUIContent("Debuger");
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

        private void Startup()
        {

            string path = Application.dataPath + "/encrypt.txt";

            //FileUtils.WriteEncrypt(path, parameter[0]);
             
            string value= FileUtils.ReadEncrypt(path);

            Debug.LogError(value);
        }
    }
}
