using System;
using UnityEngine;

namespace ILRuntime
{
    public static class Debuger
    {
        public static void Log(object message)
        {
            Debug.Log(Identification + message);
        }

        public static void LogWarning(object message)
        {
            Debug.LogWarning(Identification + message);
        }

        public static void LogError(object message)
        {
            Debug.LogError(Identification + message);
        }

        private static string Identification
        {
            get
            {
                return string.Format("<color=red>ILRuntime</color>:<color=green>[{0}]</color>", DateTime.Now);
            }
        }
    }
}