using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public static class GlobalVariables
    {
        private static readonly object lockObject = new object();

        private static readonly Dictionary<string, object> variables = new Dictionary<string, object>();

        public static T Get<T>(string key)
        {
            if (variables.ContainsKey(key))
            {
                return (T)variables[key];
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
                        variables[key] = value;
                    }
                    else
                    {
                        variables.Add(key, value);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            }
        }

        public static void Dispose()
        {
            variables.Clear();
        }
    }
}