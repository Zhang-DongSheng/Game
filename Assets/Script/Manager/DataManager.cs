using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class DataManager : Singleton<DataManager>
    {
        private readonly Dictionary<string, ScriptableObject> m_data = new Dictionary<string, ScriptableObject>();

        #region Load
        public T Load<T>(string key, string path) where T : ScriptableObject
        {
            if (m_data.ContainsKey(key))
            {
                return (T)m_data[key];
            }
            else
            {
                m_data.Add(key, Load<T>(path));
                return (T)m_data[key];
            }
        }

        private T Load<T>(string path) where T : ScriptableObject
        {
            T _data = default;

            try
            {
                _data = Resources.Load(path) as T;
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }

            return _data;
        }
        #endregion
    }
}