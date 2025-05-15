using Game;
using LitJson;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Data
{
    public class DataConfig : DataBase
    {
        public List<ConfigInformation> list;

        public static string Get(string key)
        {
            var data = DataManager.Instance.Load<DataConfig>();

            if (data != null)
            {
                for (int i = 0; i < data.list.Count; i++)
                {
                    if (data.list[i].key == key)
                    {
                        return data.list[i].value;
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
                    Debuger.LogError(Author.Data, string.Format("{0} Convert To {1} Fail!", key, typeof(T).ToString()));
                }
            }
            return default;
        }

        public override void Load(JsonData json)
        {
            int count = json.Count;

            for (int i = 0; i < count; i++)
            {
                list.Add(json[i].GetType<ConfigInformation>());
            }
        }

        public override void Detection()
        {
            var dic = new Dictionary<string, int>();

            int count = list.Count;

            for (int i = 0; i < count; i++)
            {
                if (dic.ContainsKey(list[i].key))
                {
                    Debuger.LogError(Author.Data, "config exist the same key:" + list[i].key);
                }
                else
                {
                    dic.Add(list[i].key, 1);
                }
            }
        }

        public override void Clear()
        {
            list = new List<ConfigInformation>();
        }
    }
    [System.Serializable]
    public class ConfigInformation : InformationBase
    {
        public string key;

        public string value;
    }
}