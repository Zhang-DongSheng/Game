using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Profiling;

namespace Game.Develop
{
    public class DevelopController : MonoSingleton<DevelopController>
    {
        private bool record = true;

        private WaitUntil wait;

        private DevelopView view;

        private readonly StringBuilder builder = new StringBuilder();

        private readonly List<string> messages = new List<string>();

        protected override void Initialize()
        {
            Application.logMessageReceived += OnLogMessageReceived;

            record = true;

            wait = new WaitUntil(() =>
            {
                return !record;
            });
            RuntimeManager.Instance.StartCoroutine(RecordProfiler());
        }

        protected override void Release()
        {
            Application.logMessageReceived -= OnLogMessageReceived;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                DisplayDevelop();
            }
        }

        public static void BeginSample(string name)
        {
            Profiler.BeginSample(name);
        }

        public static void EndSample()
        {
            Profiler.EndSample();
        }

        public static void ExtractSample(string name, Action action)
        {
            Profiler.BeginSample(name);
            action?.Invoke();
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

        private void DisplayDevelop()
        {
            if (view != null)
            {
                view.Open();
            }
            else
            {
                GameObject target = new GameObject("view");

                target.transform.parent = transform;

                view = target.AddComponent<DevelopView>();
            }
        }
    }
}