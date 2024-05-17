using System;
using System.Collections.Generic;
using System.Text;

namespace UnityEngine
{
    public enum Author
    {
        None,
        Editor,
        Device,
        Test,
        Script,
        Utility,
        UI,
        File,
        Data,
        Sound,
        Resource,
        ILRuntime,
    }
    public static class Debuger
    {
        public static bool DEBUG = true;

        public static readonly Dictionary<Author, LogType> authors = new Dictionary<Author, LogType>()
        {
            { Author.None,     LogType.Exception },
            { Author.Editor,   LogType.Exception },
            { Author.Device,   LogType.Exception },
            { Author.File,     LogType.Exception },
            { Author.Script,   LogType.Log },
            { Author.Utility,  LogType.Log },
            { Author.UI,       LogType.Log },
            { Author.Test,     LogType.Log },
            { Author.Data,     LogType.Error },
            { Author.Sound,    LogType.Warning },
            { Author.Resource, LogType.Log },
            { Author.ILRuntime,LogType.Error },
        };
        private static readonly StringBuilder builder = new StringBuilder();

        public static void Assert(bool condition, object message, Object context = null)
        {
            Debug.Assert(condition, message, context);
        }

        public static void Log(Author author, object message, Object context = null)
        {
            if (!Pass(author, LogType.Log)) return;

            if (Convert.IsDBNull(context))
            {
                Debug.Log(Format(author, message));
            }
            else
            {
                Debug.Log(Format(author, message), context);
            }
        }

        public static void LogWarning(Author author, object message, Object context = null)
        {
            if (!Pass(author, LogType.Warning)) return;

            if (Convert.IsDBNull(context))
            {
                Debug.LogWarning(Format(author, message));
            }
            else
            {
                Debug.LogWarning(Format(author, message), context);
            }
        }

        public static void LogAssertion(Author author, object message, Object context = null)
        {
            if (!Pass(author, LogType.Log)) return;

            if (Convert.IsDBNull(context))
            {
                Debug.LogAssertion(Format(author, message));
            }
            else
            {
                Debug.LogAssertion(Format(author, message), context);
            }
        }

        public static void LogError(Author author, object message, Object context = null)
        {
            if (!Pass(author, LogType.Error)) return;

            if (Convert.IsDBNull(context))
            {
                Debug.LogError(Format(author, message));
            }
            else
            {
                Debug.LogError(Format(author, message), context);
            }
        }

        public static void LogException(Author author, Exception exception)
        {
            if (!Pass(author, LogType.Exception)) return;

            Debug.LogException(exception);
        }

        public static void LogNotifycation(Author author, string message)
        {
#if UNITY_EDITOR
            if (UnityEditor.EditorWindow.focusedWindow != null)
            {
                var content = new GUIContent(Format(author, message));

                UnityEditor.EditorWindow.focusedWindow.ShowNotification(content);
            }
#endif
        }

        public static void DisplayDialog(string content, Action<bool> callback)
        {
#if UNITY_EDITOR
            var result = UnityEditor.EditorUtility.DisplayDialog("Dialog", content, "OK");

            callback?.Invoke(result);
#endif
        }

        private static bool Pass(Author author, LogType level)
        {
            if (!DEBUG) return false;

            if (authors.ContainsKey(author) && authors[author] >= level)
            {
                return true;
            }
            return false;
        }

        private static string Format(Author author, object message)
        {
            builder.Clear();

            builder.Append("[");

            builder.Append(author);

            builder.Append("]:");

            builder.Append(message);

            return builder.ToString();
        }
    }

    public struct LogInformation
    {
        public LogType type;

        public DateTime time;

        public string message;

        public string content;

        public LogInformation(LogType type, string message, string content)
        {
            this.type = type;

            this.message = message;

            this.content = content;

            this.time = DateTime.Now;
        }


#if UNITY_EDITOR
        public UnityEditor.MessageType MessageType
        {
            get
            {
                switch (type)
                {
                    case LogType.Log:
                        return UnityEditor.MessageType.Info;
                    case LogType.Warning:
                        return UnityEditor.MessageType.Warning;
                    case LogType.Error:
                        return UnityEditor.MessageType.Error;
                    case LogType.Exception:
                        return UnityEditor.MessageType.Error;
                    default:
                        return UnityEditor.MessageType.None;
                }
            }
        }
#endif
    }
}