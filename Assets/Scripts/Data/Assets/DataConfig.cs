using Game;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class DataConfig : DataBase
    {
        public List<ConfigInformation> config = new List<ConfigInformation>();

        public static string Get(string key)
        {
            var data = DataManager.Instance.Load<DataConfig>();

            if (data != null)
            {
                for (int i = 0; i < data.config.Count; i++)
                {
                    if (data.config[i].key == key)
                    {
                        return data.config[i].value;
                    }
                }
            }
            return string.Empty;
        }

        public static T Get<T>(string key)
        {
            string value = Get(key);

            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    return (T)System.Convert.ChangeType(value, typeof(T));
                }
                catch
                {
                    Debuger.LogError(Author.Data, string.Format("[{0}]类型转换失败，数据不为{1}", key, typeof(T).ToString()));
                }
            }
            return default;
        }

        public override void Set(string content)
        {
            base.Set(content);

            int count = m_list.Count;

            for (int i = 0; i < count; i++)
            {
                config.Add(m_list[i].GetType<ConfigInformation>());
            }
        }

        public override void Clear()
        {
            config = new List<ConfigInformation>();
        }
    }
    [System.Serializable]
    public class ConfigInformation : InformationBase
    {
        public string key;

        public string value;
    }
}