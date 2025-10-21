using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public sealed class UIParameter : IDisposable
    {
        private readonly Dictionary<string, object> paramters;

        public UIParameter()
        {
            paramters = new Dictionary<string, object>();
        }

        public object this[string key]
        {
            get
            {
                return Get(key);
            }
            set
            {
                AddOrReplace(key, value);
            }
        }

        public void AddOrReplace(string key, object value)
        {
            if (Contains(key))
            {
                paramters[key] = value;
            }
            else
            {
                paramters.Add(key, value);
            }
        }

        public void Remove(string key)
        {
            if (Contains(key))
            {
                paramters.Remove(key);
            }
        }

        public object Get(string key)
        {
            if (Contains(key))
            {
                return paramters[key];
            }
            return null;
        }

        public T Get<T>(string key)
        {
            if (Contains(key))
            {
                try
                {
                    return (T)paramters[key];
                }
                catch (Exception e)
                {
                    Debuger.LogException(Author.UI, e);
                }
            }
            return default;
        }

        private bool Contains(string key)
        {
            return paramters.ContainsKey(key);
        }

        public void Dispose()
        {
            paramters.Clear();
        }

        public override string ToString()
        {
            return string.Join(",", paramters);
        }
    }
}