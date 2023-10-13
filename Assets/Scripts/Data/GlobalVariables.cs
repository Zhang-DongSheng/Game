using Game;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Data
{
    public static class GlobalVariables
    {
        private static readonly Dictionary<string, string> variables = new Dictionary<string, string>();

        private static readonly object _lock = new object();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void RuntimeInitialized()
        {
            string path = string.Format("{0}/{1}", Application.persistentDataPath, "globalvariables");

            using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate))
            {
                StreamReader reader = new StreamReader(stream);

                string line = reader.ReadLine();

                while (!string.IsNullOrEmpty(line))
                {
                    string[] paramter = line.Split('|');

                    if (paramter.Length == 2)
                    {
                        variables.Add(paramter[0], Utility.Cryptogram.Decrypt(paramter[1]));
                    }
                    line = reader.ReadLine();
                }
                reader.Dispose();
            }
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
            lock (_lock)
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
                    Save();
                }
                catch (Exception e)
                {
                    Debuger.LogError(Author.Data, e.Message);
                }
            }
        }

        public static bool Exists(string key)
        {
            return variables.ContainsKey(key);
        }

        public static void Save()
        {
            string path = string.Format("{0}/{1}", Application.persistentDataPath, "globalvariables");

            using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate))
            {
                StreamWriter writer = new StreamWriter(stream);

                foreach (var value in variables)
                {
                    writer.WriteLine(string.Format("{0}|{1}", value.Key, Utility.Cryptogram.Encrypt(value.Value)));
                }
                writer.Flush(); writer.Dispose();
            }
        }

        public static void Dispose()
        {
            variables.Clear(); Save();
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
                    if (typeof(T).IsEnum)
                    {
                        return (T)Enum.Parse(typeof(T), value);
                    }
                    else if (typeof(T).IsValueType)
                    {
                        return (T)Convert.ChangeType(value, typeof(T));
                    }
                    else if (typeof(T).Equals(typeof(string)))
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