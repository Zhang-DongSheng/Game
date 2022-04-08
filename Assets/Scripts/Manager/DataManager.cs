using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class DataManager : Singleton<DataManager>
    {
        private readonly Dictionary<string, DataBase> m_data = new Dictionary<string, DataBase>();

        public T Load<T>(string path) where T : DataBase
        {
            string key = typeof(T).Name;

            if (m_data.ContainsKey(key))
            {
                return m_data[key] as T;
            }
            else
            {
                T asset = null;
#if UNITY_EDITOR
                asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(string.Format("Assets/{0}.asset", path));
#else
                asset = Resources.Load<T>(path);
#endif
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
                    Factory.Instance.Pop(key, (value) =>
                    {
                        m_data.Add(key, value as T); action?.Invoke(m_data[key] as T);
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