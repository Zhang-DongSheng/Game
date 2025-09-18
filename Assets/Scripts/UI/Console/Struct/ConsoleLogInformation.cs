using System;
using UnityEngine;

namespace Game.Data
{
    public class ConsoleLogInformation
    {
        public LogType type;

        public DateTime time;

        public string message;

        public string source;

        public ConsoleLogInformation(LogType type, string message, string source)
        {
            this.type = type;
            this.time = DateTime.Now;
            this.message = message;
            this.source = source;
        }
    }
}