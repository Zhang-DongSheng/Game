using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Profiling;

namespace Game
{
    public class DevelopLogic : Singleton<DevelopLogic>, ILogic
    {
        private bool record = true;

        private WaitUntil wait;

        private readonly StringBuilder builder = new StringBuilder();

        private readonly List<string> messages = new List<string>();

        public void Initialize()
        {
            Application.logMessageReceived += OnLogMessageReceived;

            record = true;

            wait = new WaitUntil(() =>
            {
                return !record;
            });
            RuntimeManager.Instance.StartCoroutine(RecordProfiler());
        }

        public void Release()
        {
            Application.logMessageReceived -= OnLogMessageReceived;
        }

        public void Begain(string name)
        {
            Profiler.BeginSample(name);
        }

        public void End()
        {
            Profiler.EndSample();
        }

        public void Break()
        {
            record = false;

            string path = string.Format("{0}/{1}-{2}.txt", Application.persistentDataPath, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"), "log");

            using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate))
            {
                StreamWriter writer = new StreamWriter(stream);

                foreach (string line in messages)
                {
                    writer.WriteLine(line);
                }
                writer.Flush(); writer.Close();
            }
        }

        private void OnLogMessageReceived(string message, string source, LogType type)
        {
            builder.Clear();

            builder.AppendLine(message);

            builder.AppendLine(source);

            messages.Add(builder.ToString());
        }

        private IEnumerator RecordProfiler()
        {
            Profiler.logFile = string.Format("{0}/{1}-{2}.data", Application.persistentDataPath, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"), "profiler"); ;

            Profiler.enabled = true;

            Profiler.enableBinaryLog = true;

            yield return wait;

            Profiler.enableBinaryLog = false;

            yield return null;
        }
    }
}