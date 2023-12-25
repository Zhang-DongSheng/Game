using System;
using UnityEngine;

namespace Game
{
    internal class Logcat : MonoBehaviour
    {
        private void Awake()
        {
            Application.logMessageReceived += LogMessageReceived;
        }

        private void OnDestroy()
        {
            Application.logMessageReceived -= LogMessageReceived;
        }

        private void LogMessageReceived(string message, string source, LogType type)
        {
            //log_data.Add(new LogData(type, message, source));
        }

        public void Save()
        {

        }
        [System.Serializable]
        public class LogData
        {
            public LogType type;
            public DateTime time;
            public string message;
            public string source;

            public LogData(LogType type, string message, string source)
            {
                this.type = type;
                this.time = DateTime.Now;
                this.message = message;
                this.source = source;
            }

            public string Message
            {
                get
                {
                    switch (type)
                    {
                        case LogType.Log:
                            return string.Format("<color=green>{0}</color>", message);
                        case LogType.Warning:
                            return string.Format("<color=yellow>{0}</color>", message);
                        case LogType.Error:
                            return string.Format("<color=red>{0}</color>", message);
                        case LogType.Exception:
                            return string.Format("<color=black>{0}</color>", message);
                        default:
                            return string.Format("<color=white>{0}</color>", message);
                    }
                }
            }
        }
    }
}
