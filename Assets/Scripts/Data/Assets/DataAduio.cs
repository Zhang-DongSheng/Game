using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class DataAduio : DataBase
    {
        public List<AudioInformation> list;

        public static AudioInformation Get(string key)
        {
            var data = DataManager.Instance.Load<DataAduio>();

            if (data != null)
            {
                return data.list.Find(x => x.key == key);
            }
            return null;
        }

        public override void Detection()
        {
            var dic = new Dictionary<string, int>();

            int count = list.Count;

            for (int i = 0; i < count; i++)
            {
                if (dic.ContainsKey(list[i].key))
                {
                    Debuger.LogError(Author.Data, "audio exist the same key:" + list[i].key);
                }
                else
                {
                    dic.Add(list[i].key, 1);
                }
            }
        }

        public override void Clear()
        {
            list = new List<AudioInformation>();
        }
    }
    [System.Serializable]
    public class AudioInformation : InformationBase
    {
        public string key;

        public string path;
    }
}
