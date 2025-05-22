using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class ConsoleLogcat : ConsoleBase
    {
        public override string Name => "输出日志";

        public int number = 100;

        [SerializeField] private PrefabTemplate prefab;

        private readonly List<ItemConsoleLog> items = new List<ItemConsoleLog>();

        private readonly List<ConsoleLogInformation> list = new List<ConsoleLogInformation>();

        public override void Initialize()
        {
            Application.logMessageReceived += LogMessageReceived;
        }

        public override void Dispose()
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
    }
}