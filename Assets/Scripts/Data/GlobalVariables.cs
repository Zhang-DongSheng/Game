using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public static class GlobalVariables
    {
        private static readonly object lockObject = new object();

        private static readonly Dictionary<string, string> variables = new Dictionary<string, string>();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void RuntimeInitialized()
        {
            Debuger.Log(Author.Data, "RuntimeInitialized");
        }

        public static T Get<T>(string key)
        {
            if (variables.ContainsKey(key))
            {
                return Format<T>(variables[key]);
            }
            return default;
        }

        public static void Set(string key, object value)
        {
            lock (lockObject)
            {
                try
                {
                    if (variables.ContainsKey(key))
                    {
                        variables[key] = Parse(value);
                    }
                    else
                    {
                        variables.Add(key, Parse(value));
                    }
                }
                catch (Exception e)
                {
                    Debuger.LogError(Author.Data, e.Message);
                }
            }
        }

        public static void Save()
        {

        }

        public static void Dispose()
        {
            variables.Clear();
        }

        private static string Parse(object value)
        {
            if (Convert.IsDBNull(value))
            {
                return string.Empty;
            }
            else
            {
                try
                {
                    if (value.GetType().IsValueType)
                    {
                        return value.ToString();
                    }
                    else if (value is String str)
                    {
                        return str;
                    }
                    else
                    {
                        return JsonUtility.ToJson(value);
                    }
                }
                catch (Exception e)
                {
                    Debuger.LogError(Author.Data, e.Message);
                }
                return string.Empty;
            }
        }

        private static T Format<T>(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return default;
            }
            else
            {
                try
                {
                    if (typeof(T).IsValueType)
                    {
                        return (T)Convert.ChangeType(value, typeof(T));
                    }
                    else if (typeof(T).Name == "String")
                    {
                        return (T)(object)value;
                    }
                    else
                    {
                        return JsonUtility.FromJson<T>(value);
                    }
                }
                catch (Exception e)
                {
                    Debuger.LogError(Author.Data, e.Message);
                }
                return default;
            }
        }
    }
}