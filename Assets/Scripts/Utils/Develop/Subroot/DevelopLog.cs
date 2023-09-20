using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Develop
{
    public class DevelopLog : DevelopBase
    {
        private Rect rect = new Rect();

        private readonly Dictionary<LogType, List<LogInformation>> _messages = new Dictionary<LogType, List<LogInformation>>();

        public override void Initialize()
        {
            rect = new Rect();
        }

        public override void Register()
        {
            Application.logMessageReceived += OnReceiveMessage;
        }

        public override void Unregister()
        {
            Application.logMessageReceived -= OnReceiveMessage;
        }

        public override void Refresh()
        {
            GUILayout.BeginVertical();
            {
                scroll = GUILayout.BeginScrollView(scroll);
                {
                    if (_messages.TryGetValue(LogType.Log, out List<LogInformation> logs))
                    {
                        int count = logs.Count;

                        for (int i = 0; i < count; i++)
                        {
                            RefreshLog(logs[i]);
                        }
                    }
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();
        }

        private void RefreshLog(LogInformation log)
        {
            GUILayout.BeginHorizontal(GUILayout.Height(100));
            {
                if (GUILayout.Button(log.content, GUILayout.ExpandWidth(true)))
                {
                    RefreshLogDetail(log);
                }
            }
            GUILayout.EndHorizontal();
        }

        private void RefreshLogDetail(LogInformation log)
        { 
            
        }

        private void OnReceiveMessage(string condition, string stackTrace, LogType type)
        {
            if (_messages.ContainsKey(type))
            {
                _messages[type].Add(new LogInformation(condition, stackTrace));
            }
            else
            {
                _messages.Add(type, new List<LogInformation>() { new LogInformation(condition, stackTrace) });
            }
        }

        class LogInformation
        {
            public string content;

            public string message;

            public DateTime time;

            public LogInformation(string condition, string stackTrace)
            {
                this.content = condition;

                this.message = stackTrace;

                time = DateTime.Now;
            }
        }

        public override string Name => "Log";
    }
}