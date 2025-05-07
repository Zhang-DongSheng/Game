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
        Network,
        Utility,
        UI,
        File,
        Data,
        Sound,
        Resource,
        Hotfix,
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
            { Author.Script,   LogType.Exception },
            { Author.Network,  LogType.Exception },
            { Author.Utility,  LogType.Exception },
            { Author.UI,       LogType.Exception },
            { Author.Test,     LogType.Exception },
            { Author.Data,     LogType.Exception },
            { Author.Sound,    LogType.Exception },
            { Author.Resource, LogType.Exception },
            { Author.Hotfix,LogType.Exception },
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
        #region 解决日志双击溯源问题
#if UNITY_EDITOR
        [UnityEditor.Callbacks.OnOpenAssetAttribute(0)]
        static bool OnOpenAsset(int instanceID, int line)
        {
            string content = GetStackTrace();

            if (string.IsNullOrEmpty(content)) return false;

            if (!content.Contains("Debuger")) return false;
            // 使用正则表达式匹配at的哪个脚本的哪一行
            var matches = System.Text.RegularExpressions.Regex.Match(content, @"\(at (.+)\)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            string pathLine;
            while (matches.Success)
            {
                pathLine = matches.Groups[1].Value;

                if (!pathLine.Contains("Debuger.cs"))
                {
                    int splitIndex = pathLine.LastIndexOf(":");
                    // 脚本路径
                    string path = pathLine.Substring(0, splitIndex);
                    // 行号
                    line = Convert.ToInt32(pathLine.Substring(splitIndex + 1));

                    string fullPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("Assets"));

                    fullPath = fullPath + path;
                    // 跳转到目标代码的特定行
                    UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(fullPath.Replace('/', '\\'), line);
                    break;
                }
                matches = matches.NextMatch();
            }
            return true;
        }
        /// <summary>
        /// 获取当前日志窗口选中的日志的堆栈信息
        /// </summary>
        static string GetStackTrace()
        {
            // 通过反射获取ConsoleWindow类
            var window = typeof(UnityEditor.EditorWindow).Assembly.GetType("UnityEditor.ConsoleWindow");
            // 获取窗口实例
            var fieldInfo = window.GetField("ms_ConsoleWindow",
                System.Reflection.BindingFlags.Static |
                System.Reflection.BindingFlags.NonPublic);
            var console = fieldInfo.GetValue(null);
            if (console != null)
            {
                if ((object)UnityEditor.EditorWindow.focusedWindow == console)
                {
                    // 获取m_ActiveText成员
                    fieldInfo = window.GetField("m_ActiveText",
                        System.Reflection.BindingFlags.Instance |
                        System.Reflection.BindingFlags.NonPublic);
                    // 获取m_ActiveText的值
                    string activeText = fieldInfo.GetValue(console).ToString();
                    return activeText;
                }
            }
            return null;
        }
#endif
        #endregion
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