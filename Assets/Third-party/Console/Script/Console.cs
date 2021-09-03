using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Profiling;

namespace Game.Console
{
    public class Console : MonoBehaviour
    {
        private readonly List<LogData> log_data = new List<LogData>();

        private readonly List<int> fps_data = new List<int>();

        private readonly List<int> fps_temp = new List<int>() { 0, 0, 0, 0 };

        private void Awake()
        {
            Application.logMessageReceived += LogMessageReceiver;
        }

        private void Update()
        {
            Update_FPS();
        }

        private void LogMessageReceiver(string message, string source, LogType type)
        {
            log_data.Add(new LogData(type, message, source));
        }

        private void Update_FPS()
        {
            int _fps = (int)(1f / Time.deltaTime);

            if (fps_data.Count > 9)
            {
                fps_data.RemoveAt(0);
            }
            fps_data.Add(_fps);

            fps_temp[3] = 0;
            fps_temp[1] = fps_temp[2] = fps_data[0];
            for (int i = 0; i < fps_data.Count; i++)
            {
                if (fps_temp[1] > fps_data[i])
                {
                    fps_temp[1] = fps_data[i];
                }
                else if (fps_temp[2] < fps_data[i])
                {
                    fps_temp[2] = fps_data[i];
                }
                fps_temp[3] += fps_data[i];
            }
            fps_temp[0] = fps_temp[3] / fps_data.Count;
        }

        private IEnumerator Save__Log(LogType ignore)
        {
            using (FileStream fs = new FileStream(Config.Path_Log, FileMode.OpenOrCreate))
            {
                StreamWriter sw = new StreamWriter(fs);

                for (int i = 0; i < log_data.Count; i++)
                {
                    if (log_data[i].type >= ignore)
                    {
                        sw.WriteLine(log_data[i].message);
                        sw.WriteLine(log_data[i].content);
                    }
                }

                sw.Dispose();
            }

            yield return null;
        }

        private IEnumerator Save__Profiler(int second)
        {
            Profiler.logFile = Config.Path_Profiler;
            Profiler.enabled = true;
            Profiler.enableBinaryLog = true;

            for (int i = 0; i < second; i++)
            {
                yield return new WaitForSeconds(1);
            }

            Profiler.enableBinaryLog = false;

            yield return null;
        }

        #region Function
        public void Clear_Log()
        {
            log_data.Clear();
        }

        public void Save_Log(LogType ignore = LogType.Log)
        {
            StartCoroutine(Save__Log(ignore));
        }

        public void Save_Profiler(int second)
        {
            StartCoroutine(Save__Profiler(second));
        }

        public void Save_ScreenCapture()
        {
            ScreenCapture.CaptureScreenshot(Config.Path_ScreenCapture, 0);
        }

        public void ExecuteCommand(string command)
        {
            if (string.IsNullOrEmpty(command))
                return;

            string[] rule = command.ToLower().Split(' ');

            if (rule.Length > 1)
            {
                switch (rule[0])
                {
                    case "get":
                        Debug.Log("获取");
                        break;
                    case "level":
                        Debug.Log("关卡");
                        break;
                    default:
                        Debug.LogWarningFormat("暂未支持该类型命令:{0}", rule[0]);
                        break;
                }
            }
            else
            {
                switch (rule[0])
                {
                    case "levelup":
                        Debug.Log("升级");
                        break;
                    default:
                        Debug.LogWarningFormat("暂未支持该类型命令:{0}", rule[0]);
                        break;
                }
            }
        }
        #endregion

        #region Param
        public List<LogData> LogData
        {
            get
            {
                return log_data;
            }
        }

        public float FPS
        {
            get
            {
                return fps_temp[0];
            }
        }
        #endregion
    }
}