using System;
using UnityEngine;

namespace Hotfix.Game
{
    public static class Debuger
    {
        public static void Log(object message)
        {
            UnityEngine.Debuger.Log(Author.Hotfix, Identification + message);
        }

        public static void LogWarning(object message)
        {
            UnityEngine.Debuger.LogWarning(Author.Hotfix, Identification + message);
        }

        public static void LogError(object message)
        {
            UnityEngine.Debuger.LogError(Author.Hotfix, Identification + message);
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