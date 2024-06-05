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
                T asset = ResourceManager.Load<T>(string.Format("Package/Data/{0}.asset", key));

                if (asset != null)
                {
                    m_data.Add(key, asset);
                }
                return asset;
            }
        }

        public T Load<T>(string path) where T : DataBase
        {
            string key = typeof(T).Name;

            if (m_data.ContainsKey(key))
            {
                return m_data[key] as T;
            }
            else
            {
                T asset = ResourceManager.Load<T>(string.Format("{0}/{1}.asset", path, key));

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
                    ResourceManager.LoadAsync(string.Format("Package/Data/{0}.asset", key), (value) =>
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

        public static T Get<T>(IList<T> list, uint primary, bool order = false) where T : InformationBase, new()
        {
            int count = list == null ? 0 : list.Count;

            if (count == 0) return default;

            if (order)
            {
                int min = 0, max = count - 1;

                if (list[min].primary > primary ||
                    list[max].primary < primary)
                {
                    return default;
                }
                int middle;

                while (min <= max)
                {
                    middle = (min + max) >> 1;

                    if (list[middle].primary == primary)
                    {
                        return list[middle];
                    }
                    else if (list[middle].primary > primary)
                    {
                        max = middle - 1;
                    }
                    else
                    {
                        min = middle + 1;
                    }
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    if (list[i].primary == primary)
                    {
                        return list[i];
                    }
                }
            }
            return default;
        }
    }
}