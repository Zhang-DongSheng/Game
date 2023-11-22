using Game;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Window
{
    public class TaskCenter : CustomWindow
    {
        private TaskData task;

        private TaskInformation create;

        private bool active = false;

        private readonly List<GUILayoutOption> options = new List<GUILayoutOption>() { GUILayout.Width(45), GUILayout.Width(150) };

        [MenuItem("Window/Task")]
        protected static void Open()
        {
            Open<TaskCenter>("任务中心");
        }

        protected override void Initialise()
        {
            string content = Utility.Document.Read(Path);

            if (string.IsNullOrEmpty(content))
            {
                task = new TaskData();
            }
            else
            {
                task = JsonUtility.FromJson<TaskData>(content);
            }
            //初始数据
            create = new TaskInformation()
            {
                mode = Mode.Create,
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

            if (GUILayout.Button(active ? "-" : "+", GUILayout.Width(25)))
            {
                active = !active;
            }
            if (active)
            {
                GUILayout.BeginHorizontal(GUILayout.Height(60));
                {
                    RefreshTask(create);
                }
                GUILayout.EndHorizontal();
            }
        }

        private void RefreshTask(TaskInformation task)
        {
            switch (task.mode)
            {
                case Mode.Create:
                    {
                        RefreshWrite(task);

                        if (GUILayout.Button(ToLanguage("New"), GUILayout.Width(100), GUILayout.ExpandHeight(true)))
                        {
                            task.mode = Mode.Display;

                            task.identification = DateTime.UtcNow.Ticks;

                            this.task.Add(task); Save();

                            create = new TaskInformation()
                            {
                                mode = Mode.Create,
                            };
                        }
                    }
                    break;
                case Mode.Editor:
                    {
                        RefreshWrite(task);

                        GUILayout.BeginVertical(GUILayout.Width(100));
                        {
                            if (GUILayout.Button(ToLanguage("Save")))
                            {
                                task.mode = Mode.Display; Save();
                            }
                            if (GUILayout.Button(ToLanguage("Delete")))
                            {
                                this.task.Remove(task.identification); Save();
                            }
                        }
                        GUILayout.EndVertical();
                    }
                    break;
                case Mode.Display:
                    {
                        RefreshRead(task);

                        if (GUILayout.Button(ToLanguage("Modify")))
                        {
                            task.mode = Mode.Editor;
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
                    GUILayout.Button(ToLanguage("Name"), options[0]);

                    GUILayout.Label(task.name, options[1]);

                    GUILayout.Button(ToLanguage("State"), options[0]);

                    GUILayout.Label(string.Format("{0}", task.status), options[1]);

                    switch (task.status)
                    {
                        case Status.Develop:
                            {
                                GUILayout.Button(ToLanguage("Progress"), options[0]);

                                GUILayout.Label(string.Format("{0}%", task.progress), options[1]);
                            }
                            break;
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Button(ToLanguage("Description"), options[0]);

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
                    GUILayout.Button(ToLanguage("Name"), options[0]);

                    task.name = GUILayout.TextField(task.name, options[1]);

                    switch (task.mode)
                    {
                        case Mode.Editor:
                            {
                                GUILayout.Button(ToLanguage("State"), options[0]);

                                task.status = (Status)EditorGUILayout.EnumPopup(task.status, options[1]);

                                switch (task.status)
                                {
                                    case Status.Develop:
                                        {
                                            GUILayout.Button(ToLanguage("Progress"), options[0]);

                                            GUILayout.Label(string.Format("{0}%", task.progress), GUILayout.Width(100));

                                            task.progress = GUILayout.HorizontalSlider(task.progress, 0, 100, options[1]);
                                        }
                                        break;
                                    case Status.Fail:
                                        {

                                        }
                                        break;
                                }
                            }
                            break;
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Button(ToLanguage("Description"), options[0]);

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
                Utility.Document.Write(Path, JsonUtility.ToJson(task));
            }
            else
            {
                Utility.Document.Delete(Path);
            }
        }

        private string Path
        {
            get
            {
                return string.Format("{0}/{1}", Application.dataPath.Substring(0, Application.dataPath.Length - 6), "task");
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

            public void Remove(long identification)
            {
                int index = list.FindIndex(x => x.identification == identification);

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
            public long identification;

            public string name;

            public string description;

            public float progress;

            public Status status;

            public Mode mode;
        }

        enum Mode
        {
            Create,
            Editor,
            Display,
        }

        enum Status
        {
            Project,
            Develop,
            Test,
            Done,
            Fail,
        }
    }
}