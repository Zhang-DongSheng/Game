using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace UnityEditor.Window
{
    class ExternalApplication : EditorWindow
    {
        const string KEY = "ExternalApplication";

        private readonly List<ExApplication> applications = new List<ExApplication>();

        private Vector2 scroll;

        private string path;

        private ExApplication application;

        [MenuItem("Application/Extra")]
        protected static void Open()
        {
            ExternalApplication window = GetWindow<ExternalApplication>();
            window.titleContent = new GUIContent("Application");
            window.minSize = Vector2.one * 300;
            window.maxSize = Vector2.one * 1000;
            window.Load();
            window.Show();
        }

        private void Load()
        {
            applications.Clear();

            //Windows
            AddOrReplaceWindowsApplication("mspaint", "画图");
            AddOrReplaceWindowsApplication("notepad", "记事本");

            string value = UnityEngine.PlayerPrefs.GetString(KEY);

            if (!string.IsNullOrEmpty(value))
            {
                string[] list = value.Split(',');

                for (int i = 0; i < list.Length; i++)
                {
                    AddOrReplaceApplication(list[i]);
                }
            }
        }

        private void OnGUI()
        {
            Refresh();
        }

        private void Refresh()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("添加新应用：", GUILayout.Width(70));

                if (GUILayout.Button(path))
                {
                    path = EditorUtility.OpenFilePanel("Application", "", "exe");
                }

                if (GUILayout.Button("+", GUILayout.Width(50)))
                {
                    if (string.IsNullOrEmpty(path)) return;

                    AddOrReplaceApplication(path);

                    Save();
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical(GUILayout.Width(300));
                {
                    scroll = GUILayout.BeginScrollView(scroll);
                    {
                        for (int i = 0; i < applications.Count; i++)
                        {
                            RefreshItem(applications[i]);
                        }
                    }
                    GUILayout.EndScrollView();
                }
                GUILayout.EndVertical();

                GUILayout.Box(string.Empty, GUILayout.ExpandHeight(true), GUILayout.Width(3));

                GUILayout.BeginVertical();
                {
                    RefreshDetail();
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }

        private void RefreshItem(ExApplication app)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(app.name, GUILayout.Width(100));

                switch (app.status)
                {
                    case ExStatus.On:
                        {
                            if (GUILayout.Button("关闭"))
                            {
                                app.Close();
                            }

                            if (GUILayout.Button("退出"))
                            {
                                app.Kill();
                            }
                        }
                        break;
                    case ExStatus.Off:
                        {
                            if (GUILayout.Button("打开"))
                            {
                                app.Startup();

                                application = app;
                            }
                        }
                        break;
                }

                if (GUILayout.Button("-", GUILayout.Width(50)))
                {
                    int index = applications.FindIndex(x => x.path == app.path);

                    if (index > -1)
                    {
                        applications.RemoveAt(index);
                    }
                    Save();
                }
            }
            GUILayout.EndHorizontal();
        }

        private void RefreshDetail()
        {
            if (application == null) return;

            RefreshLine("应用名称：", application.name);

            RefreshLine("路径：", application.path);

            RefreshLine("状态：", application.status);

            if (application.process == null) return;

            RefreshLine("进程名称：", application.process.ProcessName);

            RefreshLine("进程ID：", application.process.Id);
        }

        private void RefreshLine(string key, object value)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(key, GUILayout.Width(100));

                GUILayout.Label(value.ToString());
            }
            GUILayout.EndHorizontal();
        }

        private void AddOrReplaceWindowsApplication(string application, string name)
        {
            string path = string.Format("C:/Windows/System32/{0}.exe", application);

            AddOrReplaceApplication(path, name);
        }

        private void AddOrReplaceApplication(string path, string name = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = Path.GetFileNameWithoutExtension(path);
            }

            if (applications.Exists(x => x.name == name))
            {

            }
            else
            {
                applications.Add(new ExApplication()
                {
                    name = name,
                    path = path,
                    status = ExStatus.Off,
                });
            }
        }

        private void Save()
        {
            string value = string.Empty;

            for (int i = 0; i < applications.Count; i++)
            {
                value += applications[i].path;

                if (i < applications.Count - 1)
                {
                    value += ",";
                }
            }
            UnityEngine.PlayerPrefs.SetString(KEY, value);
        }
    }

    public class ExApplication
    {
        public string name;

        public string path;

        public Process process;

        public ExStatus status;

        public void Startup()
        {
            try
            {
                Process[] processes = Process.GetProcessesByName(name);

                if (processes != null && processes.Length > 0)
                {
                    process = processes[0];
                }
                else
                {
                    process = Process.Start(path);
                }

                if (process != null)
                {
                    process.EnableRaisingEvents = true;

                    process.Exited += OnExited;
                }
                status = ExStatus.On;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Close()
        {
            if (process != null)
            {
                process.Close();
            }
        }

        public void Kill()
        {
            if (process != null)
            {
                process.Kill(); process = null;
            }
        }

        private void OnExited(object sender, EventArgs e)
        {
            status = ExStatus.Off;
        }
    }

    public enum ExStatus
    { 
        On,
        Off,
    }
}
