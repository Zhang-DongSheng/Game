using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class DataManager : Singleton<DataManager>
    {
        private readonly Dictionary<string, DataBase> m_data = new Dictionary<string, DataBase>();

        #region Load
        public T Load<T>(string key, string path) where T : DataBase
        {
            if (m_data.ContainsKey(key))
            {
                return (T)m_data[key];
            }
            else
            {
                m_data.Add(key, Load<T>(key));
                return (T)m_data[key];
            }
        }

        private T Load<T>(string key) where T : DataBase
        {
            T _data = default;

            try
            {
                _data = Factory.Instance.Pop(key) as T;
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