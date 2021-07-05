using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Window
{
    public class Task : CustomWindow
    {
        private const string KEY = "TASKINFORMATION";

        private const int WIDTH = 45;

        private TaskInformation create;

        private TaskData task;

        protected override string Title { get { return "��������"; } }
        [MenuItem("Window/Task")]
        protected static void Open()
        {
            Open<Task>();
        }

        protected override void Init()
        {
            string content = UnityEngine.PlayerPrefs.GetString(KEY);

            if (string.IsNullOrEmpty(content))
            {
                task = new TaskData();
            }
            else
            {
                task = JsonUtility.FromJson<TaskData>(content);
            }

            create = new TaskInformation()
            {
                status = Status.Create,
            };
        }

        protected override void Refresh()
        {
            GUILayout.BeginVertical();
            {
                scroll = GUILayout.BeginScrollView(scroll);
                {
                    for (int i = 0; i < task.list.Count; i++)
                    {
                        GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(3));

                        GUILayout.BeginHorizontal(GUILayout.Height(60));
                        {
                            RefreshTask(task.list[i]);
                        }
                        GUILayout.EndHorizontal();
                    }
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal(GUILayout.Height(60));
            {
                RefreshTask(create);
            }
            GUILayout.EndHorizontal();
        }

        private void RefreshTask(TaskInformation task)
        {
            switch (task.status)
            {
                case Status.Create:
                    {
                        RefreshWrite(task);

                        if (GUILayout.Button("�¼�", GUILayout.Width(100), GUILayout.ExpandHeight(true)))
                        {
                            task.status = Status.None;

                            this.task.Add(task); Save();

                            create = new TaskInformation()
                            {
                                status = Status.Create,
                            };
                        }
                    }
                    break;
                case Status.Editor:
                    {
                        RefreshWrite(task);

                        GUILayout.BeginVertical(GUILayout.Width(100));
                        {
                            if (GUILayout.Button("����"))
                            {
                                task.status = Status.None; Save();
                            }
                            if (GUILayout.Button("ɾ��"))
                            {
                                this.task.Remove(task.name); Save();
                            }
                        }
                        GUILayout.EndVertical();
                    }
                    break;
                case Status.None:
                    {
                        RefreshRead(task);

                        if (GUILayout.Button("�޸�"))
                        {
                            task.status = Status.Editor;
                        }
                    }
                    break;
            }
        }

        private void RefreshRead(TaskInformation task)
        {
            GUILayout.BeginVertical();
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Button("����:", GUILayout.Width(WIDTH));

                    GUILayout.Label(task.name, GUILayout.Width(150));

                    GUILayout.Button("����:", GUILayout.Width(WIDTH));

                    GUILayout.Label(string.Format("{0}%", task.progress));
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Button("����:", GUILayout.Width(WIDTH));

                    GUILayout.Label(task.description, GUILayout.ExpandHeight(true));
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }

        private void RefreshWrite(TaskInformation task)
        {
            GUILayout.BeginVertical();
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Button("����:", GUILayout.Width(WIDTH));

                    task.name = GUILayout.TextField(task.name);

                    switch (task.status)
                    {
                        case Status.Editor:
                            {
                                GUILayout.Button("����:", GUILayout.Width(WIDTH));

                                GUILayout.Label(string.Format("{0}%", task.progress), GUILayout.Width(100));

                                task.progress = GUILayout.HorizontalSlider(task.progress, 0, 100, GUILayout.Width(150));
                            }
                            break;
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Button("����:", GUILayout.Width(WIDTH));

                    task.description = GUILayout.TextField(task.description, GUILayout.ExpandHeight(true));
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }

        private void Save()
        {
            if (task != null)
            {
                UnityEngine.PlayerPrefs.SetString(KEY, JsonUtility.ToJson(task));
            }
        }

        [Serializable]
        class TaskData
        {
            public List<TaskInformation> list = new List<TaskInformation>();

            public void Add(TaskInformation task)
            {
                list.Add(task);
            }

            public void Remove(string name)
            {
                int index = list.FindIndex(x => x.name == name);

                if (index != -1)
                {
                    list.RemoveAt(index);
                }
            }

            public void Clear()
            {
                list.Clear();
            }
        }
        [Serializable]
        class TaskInformation
        {
            public string name;

            public string description;

            public float progress;

            public Status status;
        }
        enum Status
        {
            None,
            Editor,
            Create,
        }
    }
}