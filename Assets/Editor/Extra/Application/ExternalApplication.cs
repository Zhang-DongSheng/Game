using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace UnityEditor.Window
{
    class ExternalApplication : CustomWindow
    {
        const string KEY = "ExternalApplication";

        private string path;

        private ExApplication application;

        private readonly List<ExApplication> applications = new List<ExApplication>();

        [MenuItem("Extra/Application")]
        protected static void Open()
        {
            Open<ExternalApplication>("第三方应用");
        }

        protected override void Initialise()
        {
            applications.Clear();

            //Windows
            AddOrReplaceWindows32Application("cmd", ToLanguage("CMD"));
            //AddOrReplaceWindowsApplication("mspaint", ToLanguage("Draw"));
            AddOrReplaceWindowsApplication("notepad", ToLanguage("Notepad"));
            AddOrReplaceWindowsApplication("explorer", ToLanguage("Explorer"));

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

        protected override void Refresh()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(ToLanguage("add new application") + ":", GUILayout.Width(70));

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

                if (app.active)
                {
                    if (GUILayout.Button(ToLanguage("Close")))
                    {
                        app.Close();
                    }

                    if (GUILayout.Button(ToLanguage("Exit")))
                    {
                        app.Kill();
                    }
                }
                else
                {
                    if (GUILayout.Button(ToLanguage("Open")))
                    {
                        app.Startup(); application = app;
                    }
                }

                if (GUILayout.Button("-", GUILayout.Width(50)))
                {
                    if (EditorUtility.DisplayDialog("确认删除", string.Format("确认删除{0}应用", app.name), ToLanguage("Delete")))
                    {
                        int index = applications.FindIndex(x => x.path == app.path);

                        if (index > -1)
                        {
                            applications.RemoveAt(index);
                        }
                        Save();
                    }
                }
            }
            GUILayout.EndHorizontal();
        }

        private void RefreshDetail()
        {
            if (application == null) return;

            RefreshLine(ToLanguage("Application"), application.name);

            RefreshLine(ToLanguage("Path"), application.path);

            RefreshLine(ToLanguage("State"), application.active);
        }

        private void RefreshLine(string key, object value)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(key + ":", GUILayout.Width(100));

                GUILayout.Label(value.ToString());
            }
            GUILayout.EndHorizontal();
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
                    active = false,
                });
            }
        }

        private void AddOrReplaceWindowsApplication(string application, string name)
        {
            string path = string.Format("C:/Windows/{0}.exe", application);

            AddOrReplaceApplication(path, name);
        }

        private void AddOrReplaceWindows32Application(string application, string name)
        {
            string path = string.Format("C:/Windows/System32/{0}.exe", application);

            AddOrReplaceApplication(path, name);
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

        public bool active;

        private Process process;

        private int processID;

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
                processID = process.Id;

                if (process != null)
                {
                    process.EnableRaisingEvents = true;

                    process.Exited += OnExited;
                }
                active = true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Close()
        {
            try
            {
                if (process != null)
                {
                    process.Close();
                }
            }
            catch
            {
                process = Process.GetProcessById(processID);

                if (process != null)
                {
                    process.Close();
                }
            }
        }

        public void Kill()
        {
            try
            {
                if (process != null)
                {
                    process.Kill(); process = null;
                }
            }
            catch
            {
                process = Process.GetProcessById(processID);

                if (process != null)
                {
                    process.Kill(); process = null;
                }
            }
        }

        private void OnExited(object sender, System.EventArgs e)
        {
            active = false;
        }
    }
}