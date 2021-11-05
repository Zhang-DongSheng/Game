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
                return m_data[key] as T;
            }
            else
            {
#if !DEBUG
                LoadResources<T>(key, path);
#else
                LoadAsync<T>(key);
#endif
                return null;
            }
        }

        private void LoadAsync<T>(string key) where T : DataBase
        {
            try
            {
                Factory.Instance.Pop(key, (value) =>
                {
                    m_data.Add(key, value as T);
                });
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        private void LoadResources<T>(string key, string path) where T : DataBase
        {
            try
            {
                m_data.Add(key, Resources.Load<T>(path));
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
#endregion
    }
}