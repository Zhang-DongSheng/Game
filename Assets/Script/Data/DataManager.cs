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
                m_data.Add(key, Load<T>(path));
                return (T)m_data[key];
            }
        }

        private T Load<T>(string path) where T : DataBase
        {
            T _data = default;

            try
            {
                //_data = Factory.Instance.GetComponent;
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