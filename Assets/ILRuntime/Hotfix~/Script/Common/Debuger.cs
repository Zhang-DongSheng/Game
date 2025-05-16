using System;
using UnityEngine;

namespace ILRuntime.Game
{
    public static class Debuger
    {
        public static void Log(object message)
        {
            UnityEngine.Debuger.Log(Author.ILRuntime, Identification + message);
        }

        public static void LogWarning(object message)
        {
            UnityEngine.Debuger.LogWarning(Author.ILRuntime, Identification + message);
        }

        public static void LogError(object message)
        {
            UnityEngine.Debuger.LogError(Author.ILRuntime, Identification + message);
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