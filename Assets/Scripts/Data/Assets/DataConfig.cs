using Game;
using LitJson;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class DataConfig : DataBase
    {
        public List<ConfigInformation> config = new List<ConfigInformation>();

        public string Get(string key)
        {
            for (int i = 0; i < config.Count; i++)
            {
                if (config[i].key == key)
                {
                    return config[i].value;
                }
            }
            return string.Empty;
        }

        public T Get<T>(string key)
        {
            string value = Get(key);

            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }
                catch
                {
                    Debuger.LogError(Author.Data, string.Format("[{0}]����ת��ʧ�ܣ����ݲ�Ϊ{1}", key, typeof(T).ToString()));
                }
            }
            return default;
        }

        public override void Set(string content)
        {
            base.Set(content);
            // һ��Ҫ�ǵ�ȥ�����һ�еĶ���
            JsonData json = JsonMapper.ToObject(content);

            if (json.ContainsKey("list"))
            {
                JsonData list = json.GetJson("list");

                int count = list.Count;

                for (int i = 0; i < count; i++)
                {
                    config.Add(new ConfigInformation()
                    {
                        key = list[i].GetString("key"),
                        value = list[i].GetString("value")
                    });
                }
            }
            else
            {
                Debuger.LogError(Author.Data, "����DB����ʧ��");
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