using System;
using System.Collections.Generic;
using System.Text;

namespace UnityEngine
{
    public enum Author
    {
        None,
        Test,
        Owner,
    }
    public static class Debuger
    {
        private const bool DEBUG = true;

        private static readonly Dictionary<Author, LogType> authors = new Dictionary<Author, LogType>()
        {
            { Author.None, LogType.Exception},
            { Author.Test, LogType.Log},
            { Author.Owner, LogType.Error},
        };

        private static readonly StringBuilder builder = new StringBuilder();

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

        private static bool Pass(Author author, LogType level)
        {
            if (!DEBUG) return false;

            if (authors.ContainsKey(author) && authors[author] <= level)
            {
                return true;
            }
            return false;
        }

        private static string Format(Author author, object message)
        {
            //builder.Clear();

            //builder.Append("[");

            //builder.Append(author);

            //builder.Append(":");

            //builder.Append(Application.isPlaying ? Time.time : DateTime.Now.Millisecond);

            //builder.Append("]");

            //builder.Append(" ");

            //builder.Append(message);

            //return builder.ToString();

            return message.ToString();
        }
    }
}