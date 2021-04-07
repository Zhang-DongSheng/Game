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

        [MenuItem("Extra/Application")]
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

            string value = PlayerPrefs.GetString(KEY);

            if (!string.IsNullOrEmpty(value))
            {
                string[] list = value.Split(',');

                for (int i = 0; i < list.Length; i++)
                {
                    applications.Add(new ExApplication()
                    {
                        name = Path.GetFileNameWithoutExtension(list[i]),
                        path = list[i],
                        status = ExStatus.Off,
                    });
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
                GUILayout.Label("�����Ӧ�ã�", GUILayout.Width(70));

                if (GUILayout.Button(path))
                {
                    path = EditorUtility.OpenFilePanel("Application", "", "exe");
                }

                if (GUILayout.Button("+", GUILayout.Width(50)))
                {
                    if (string.IsNullOrEmpty(path)) return;

                    if (!applications.Exists(x => x.path == path))
                    {
                        applications.Add(new ExApplication()
                        {
                            name = Path.GetFileNameWithoutExtension(path),
                            path = path,
                            status = ExStatus.Off,
                        });
                        Save();
                    }
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
                            if (GUILayout.Button("�ر�"))
                            {
                                app.Close();
                            }

                            if (GUILayout.Button("�˳�"))
                            {
                                app.Kill();
                            }
                        }
                        break;
                    case ExStatus.Off:
                        {
                            if (GUILayout.Button("��"))
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

            RefreshLine("Ӧ�����ƣ�", application.name);

            RefreshLine("·����", application.path);

            RefreshLine("״̬��", application.status);

            if (application.process == null) return;

            RefreshLine("�������ƣ�", application.process.ProcessName);

            RefreshLine("����ID��", application.process.Id);
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
            PlayerPrefs.SetString(KEY, value);
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
