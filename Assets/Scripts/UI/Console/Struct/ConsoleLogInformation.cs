using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.UI
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