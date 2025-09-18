using Game.Data;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Profiling;

namespace Game.Logic
{
    public class ConsoleLogic : Singleton<ConsoleLogic>, ILogic
    {
        public void Initialize()
        {
            
        }

        public void ExecuteCommand(string command)
        {
            if (string.IsNullOrEmpty(command)) return;

            string[] rule = command.ToLower().Split(' ');

            if (rule.Length > 1)
            {
                switch (rule[0])
                {
                    case "get":
                        break;
                    case "level":
                        break;
                    default:
                        Debuger.LogWarning(Author.Script, $"暂未支持该类型命令:{rule[0]}");
                        break;
                }
            }
            else
            {
                switch (rule[0])
                {
                    case "levelup":
                        break;
                    default:
                        Debuger.LogWarning(Author.Script, $"暂未支持该类型命令:{rule[0]}");
                        break;
                }
            }
        }

        public void CaptureScreenshot()
        {
            var path = Application.persistentDataPath + "/Console/ScreenCapture.png";

            path = Utility.Path.NewFile(path);

            Utility.Document.CreateDirectory(path);

            ScreenCapture.CaptureScreenshot(path, 0);
        }

        public void SaveLog(List<ConsoleLogInformation> logs)
        {
            var path = Application.persistentDataPath + "/Console/Log.txt";

            path = Utility.Path.NewFile(path);

            Utility.Document.CreateDirectory(path);

            StringBuilder builder = new StringBuilder();

            using FileStream fs = new FileStream(path, FileMode.OpenOrCreate);

            StreamWriter sw = new StreamWriter(fs);

            for (int i = 0; i < logs.Count; i++)
            {
                builder.Clear();

                builder.Append($"[{logs[i].type}]");

                builder.Append(logs[i].time.ToString("HH:mm:ss"));

                builder.AppendLine(logs[i].message);

                builder.AppendLine(logs[i].source);

                sw.Write(builder.ToString());
            }
            sw.Dispose();
        }

        public void SaveProfiler(float second)
        {
            RuntimeManager.Instance.StartCoroutine(_SaveProfiler(second));
        }

        private IEnumerator _SaveProfiler(float second)
        {
            var path = Application.persistentDataPath + "/Console/log.raw";

            path = Utility.Path.NewFile(path);

            Utility.Document.CreateDirectory(path);

            Profiler.logFile = path;

            Profiler.enabled = true;

            Profiler.enableBinaryLog = true;

            Profiler.maxUsedMemory = 256 * 1024 * 1024;

            yield return new WaitForSeconds(second);

            Profiler.enableBinaryLog = false;

            yield return null;
        }
    }
}