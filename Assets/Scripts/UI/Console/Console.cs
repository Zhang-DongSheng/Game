using UnityEngine;

namespace Game
{
    /// <summary>
    /// 控制台
    /// </summary>
    public class Console : MonoSingleton<Console>
    {
        public static readonly string[] Title = new string[5] { "控制台", "日志", "游戏信息", "设置", "设备信息" };

        private readonly bool[] l_toggle_state = new bool[3] { false, false, false };

        private Vector2 c_scroll_position;
        private Vector2 l_scroll_position;
        private Vector2 g_scroll_position;
        private Vector2 s_scroll_position;
        private Vector2 i_scroll_position;

        private readonly string[] c_input_value = new string[1];
        private readonly string[] l_input_value = new string[2];
        private readonly string[] s_input_value = new string[4];

        private int view_index = 0;

        public bool active;

        private FPS fps;

        private Logcat logcat;

        private Command command;


        private void Awake()
        {
            fps = new GameObject("fps").AddComponent<FPS>();

            fps.transform.SetParent(transform);

            logcat = new GameObject("logcat").AddComponent<Logcat>();

            logcat.transform.SetParent(transform);

            command = new Command();

            GameObject.DontDestroyOnLoad(gameObject);
        }

        private void OnGUI()
        {
            if (!active) return;

            GUI.skin.FindStyle("button").fontSize = 40;

            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), string.Empty);

            GUILayout.BeginArea(new Rect(0, 0, Screen.width, 60));
            {
                view_index = GUILayout.Toolbar(view_index, Title, GUILayout.ExpandHeight(true));
            }
            GUILayout.EndArea();

            switch (view_index)
            {
                case 0:
                    View_Console();
                    break;
                case 1:
                    View_Log();
                    break;
                case 2:
                    View_Game();
                    break;
                case 3:
                    View_Set();
                    break;
                case 4:
                    View_Infomation();
                    break;
                default:
                    break;
            }
        }

        private void View_Console()
        {
            GUILayout.BeginArea(new Rect(20, 80, Screen.width / 2 - 30, Screen.height - 100));
            {
                c_scroll_position = GUILayout.BeginScrollView(c_scroll_position);
                {
                    for (int i = 0; i < ConsoleConfig.Command.Length; i++)
                    {
                        GUILayout.Label(ConsoleConfig.Command[i]);
                    }
                }
                GUILayout.EndArea();
            }
            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(Screen.width / 2 + 10, 80, Screen.width / 2 - 30, Screen.height - 240));
            {
                c_input_value[0] = GUILayout.TextField(c_input_value[0], GUILayout.ExpandHeight(true));
            }
            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(Screen.width / 2 + 10, Screen.height - 140, Screen.width / 2 - 30, 50));
            {
                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("执行命令", GUILayout.ExpandHeight(true)))
                    {
                        command.ExecuteCommand(c_input_value[0]);
                    }
                    if (GUILayout.Button("清除命令", GUILayout.ExpandHeight(true)))
                    {
                        c_input_value[0] = string.Empty;
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(Screen.width / 2 + 10, Screen.height - 70, Screen.width / 2 - 30, 50));
            {
                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("联系我们", GUILayout.ExpandHeight(true)))
                    {
                        Utility.Common.OpenQQ(GameConfig.QQ);
                    }
                    if (GUILayout.Button("关闭窗口", GUILayout.ExpandHeight(true)))
                    {
                        Close();
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndArea();
        }

        private void View_Log()
        {
            GUILayout.BeginArea(new Rect(20, 80, Screen.width / 2 - 30, Screen.height - 100));
            {
                GUILayout.BeginVertical();
                {
                    GUILayout.BeginHorizontal();
                    {
                        l_toggle_state[0] = GUILayout.Toggle(l_toggle_state[0], "日志");
                        l_toggle_state[1] = GUILayout.Toggle(l_toggle_state[1], "警告");
                        l_toggle_state[2] = GUILayout.Toggle(l_toggle_state[2], "错误");
                    }
                    GUILayout.EndHorizontal();

                    l_scroll_position = GUILayout.BeginScrollView(l_scroll_position);
                    {
                        //for (int i = 0; i < m_console.LogData.Count; i++)
                        //{
                        //    if (!l_toggle_state[0] && m_console.LogData[i].type == LogType.Log)
                        //    {
                        //        continue;
                        //    }
                        //    if (!l_toggle_state[1] && m_console.LogData[i].type == LogType.Warning)
                        //    {
                        //        continue;
                        //    }
                        //    if (!l_toggle_state[2])
                        //    {
                        //        if (m_console.LogData[i].type.Equals(LogType.Error) ||
                        //            m_console.LogData[i].type.Equals(LogType.Exception))
                        //        {
                        //            continue;
                        //        }
                        //    }
                        //    if (GUILayout.Button(m_console.LogData[i].Message, GUILayout.Height(30)))
                        //    {
                        //        l_input_value[1] = m_console.LogData[i].source;
                        //    }
                        //}
                    }
                    GUILayout.EndScrollView();
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(Screen.width / 2 + 10, 80, Screen.width / 2 - 30, Screen.height - 240));
            {
                GUILayout.Box(l_input_value[1], GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            }
            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(Screen.width / 2 + 10, Screen.height - 140, Screen.width / 2 - 30, 50));
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Box("<color=black>保存时间\n(s)</color>", GUILayout.ExpandHeight(true));
                    l_input_value[0] = GUILayout.TextField(l_input_value[0], GUILayout.ExpandHeight(true));
                    if (GUILayout.Button("保存分析数据", GUILayout.ExpandHeight(true)))
                    {
                        //int.TryParse(l_input_value[0], out int time);
                        //m_console.Save_Profiler(time);
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(Screen.width / 2 + 10, Screen.height - 70, Screen.width / 2 - 30, 50));
            {
                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("截图", GUILayout.ExpandHeight(true)))
                    {
                        //m_console.Save_ScreenCapture();
                    }
                    if (GUILayout.Button("清除日志", GUILayout.ExpandHeight(true)))
                    {
                        //m_console.Clear_Log();
                    }
                    if (GUILayout.Button("保存日志", GUILayout.ExpandHeight(true)))
                    {
                        //m_console.Save_Log();
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndArea();
        }

        private void View_Game()
        {
            GUILayout.BeginArea(new Rect(20, 80, Screen.width - 40, Screen.height - 100));
            {
                g_scroll_position = GUILayout.BeginScrollView(g_scroll_position);
                {
                    GUILayout.Label("FPS:" + fps.Fps);

                    //custom some game infomation...
                }
                GUILayout.EndArea();
            }
            GUILayout.EndArea();
        }

        private void View_Set()
        {
            GUILayout.BeginArea(new Rect(20, 80, Screen.width - 40, Screen.height - 100));
            {
                GUILayout.BeginVertical();
                {
                    s_scroll_position = GUILayout.BeginScrollView(s_scroll_position);
                    {
                        GUILayout.BeginHorizontal(GUILayout.Height(50));
                        {
                            GUILayout.Box("屏幕常亮", GUILayout.ExpandHeight(true));
                            bool state = Screen.sleepTimeout.Equals(SleepTimeout.NeverSleep);
                            if (GUILayout.Button(state ? "关闭" : "开启", GUILayout.ExpandHeight(true)))
                            {
                                if (state)
                                {
                                    Screen.sleepTimeout = SleepTimeout.SystemSetting;
                                }
                                else
                                {
                                    Screen.sleepTimeout = SleepTimeout.NeverSleep;
                                }
                            }
                        }
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal(GUILayout.Height(50));
                        {
                            GUILayout.Box("多指触控", GUILayout.ExpandHeight(true));
                            bool state = Input.multiTouchEnabled;
                            if (GUILayout.Button(state ? "关闭" : "开启", GUILayout.ExpandHeight(true)))
                            {
                                Input.multiTouchEnabled = !state;
                            }
                        }
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal(GUILayout.Height(50));
                        {
                            GUILayout.Box("时间\n" + Time.timeScale, GUILayout.ExpandHeight(true));
                            s_input_value[0] = GUILayout.TextField(s_input_value[0], GUILayout.ExpandHeight(true));
                            if (GUILayout.Button("设置时间", GUILayout.ExpandHeight(true)))
                            {
                                if (!string.IsNullOrEmpty(s_input_value[0]))
                                {
                                    float.TryParse(s_input_value[0], out float value);
                                    Time.timeScale = value;
                                }
                            }
                        }
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal(GUILayout.Height(50));
                        {
                            GUILayout.Box("帧率\n" + Application.targetFrameRate, GUILayout.ExpandHeight(true));
                            s_input_value[1] = GUILayout.TextField(s_input_value[1], GUILayout.ExpandHeight(true));
                            if (GUILayout.Button("设置帧率", GUILayout.ExpandHeight(true)))
                            {
                                if (!string.IsNullOrEmpty(s_input_value[1]))
                                {
                                    int.TryParse(s_input_value[1], out int value);
                                    if (value != -1)
                                    {
                                        Application.targetFrameRate = value;
                                        QualitySettings.vSyncCount = 0;
                                    }
                                    else
                                    {
                                        QualitySettings.vSyncCount = 1;
                                    }
                                }
                            }
                        }
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal(GUILayout.Height(50));
                        {
                            s_input_value[2] = GUILayout.TextField(s_input_value[2], GUILayout.ExpandHeight(true));
                            s_input_value[3] = GUILayout.TextField(s_input_value[3], GUILayout.ExpandHeight(true));
                            if (GUILayout.Button("屏幕尺寸", GUILayout.ExpandHeight(true)))
                            {
                                if (!string.IsNullOrEmpty(s_input_value[2]))
                                {
                                    int.TryParse(s_input_value[2], out int width);
                                    int.TryParse(s_input_value[3], out int height);

                                    width = width != 0 ? width : Screen.width;
                                    height = height != 0 ? height : Screen.height;

                                    Screen.SetResolution(width, height, true);
                                }
                            }
                        }
                        GUILayout.EndHorizontal();
                    }
                    GUILayout.EndScrollView();
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndArea();
        }

        private void View_Infomation()
        {
            GUI.color = Color.green;

            GUILayout.BeginArea(new Rect(20, 80, Screen.width - 40, Screen.height - 100));
            {
                i_scroll_position = GUILayout.BeginScrollView(i_scroll_position);
                {
                    for (int i = 0; i < ConsoleConfig.Infomation.Length; i++)
                    {
                        GUILayout.Label(ConsoleConfig.Infomation[i]);
                    }
                }
                GUILayout.EndArea();
            }
            GUILayout.EndArea();
        }

        public static void Open()
        {
            Instance.active = true;
        }

        public static void Close()
        {
            Instance.active = false;
        }

        public static void Print(string message)
        {
            Debuger.Log(Author.Script, message);
        }
    }
}