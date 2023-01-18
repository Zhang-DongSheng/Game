using System.Collections.Generic;

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
                return default;
            }
            return default;
        }
    }
    [System.Serializable]
    public class ConfigInformation : InformationBase
    {
        public string key;

        public string value;
    }
}