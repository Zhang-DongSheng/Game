using System;
using System.Collections.Generic;
using Game.Resource;
using UnityEngine;

namespace Data
{
    public class DataManager : Singleton<DataManager>
    {
        private readonly Dictionary<string, DataBase> m_data = new Dictionary<string, DataBase>();

        public T Load<T>() where T : DataBase
        {
            string key = typeof(T).Name;

            if (m_data.ContainsKey(key))
            {
                return m_data[key] as T;
            }
            else
            {
                T asset = ResourceManager.Load<T>(string.Format("Data/{0}.asset", key));

                if (asset != null)
                {
                    m_data.Add(key, asset);
                }
                return asset;
            }
        }

        public void LoadAsync<T>(Action<T> action) where T : DataBase
        {
            string key = typeof(T).Name;

            if (m_data.ContainsKey(key))
            {
                action?.Invoke(m_data[key] as T);
            }
            else
            {
                try
                {
                    ResourceManager.LoadAsync(string.Format("Data/{0}.asset", key), (value) =>
                    {
                        if (!m_data.ContainsKey(key))
                        {
                            m_data.Add(key, value as T);
                        }
                        action?.Invoke(m_data[key] as T);
                    });
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            }
        }
    }
}