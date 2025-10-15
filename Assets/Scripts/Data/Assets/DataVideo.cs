using System.Collections.Generic;
using UnityEngine;

namespace Game.Data
{
    public class DataVideo : DataBase
    {
        public List<VideoInformation> list;

        public static VideoInformation Get(string video)
        {
            var data = DataManager.Instance.Load<DataVideo>();

            if (data != null)
            {
                return data.list.Find(x => x.name == video);
            }
            return null;
        }

        public override void Detection()
        {
            var dic = new Dictionary<string, int>();

            int count = list.Count;

            for (int i = 0; i < count; i++)
            {
                if (dic.ContainsKey(list[i].name))
                {
                    Debuger.LogError(Author.Data, "video exist the same key:" + list[i].name);
                }
                else
                {
                    dic.Add(list[i].name, 1);
                }
            }
        }

        public override void Clear()
        {
            list = new List<VideoInformation>();
        }
    }
    [System.Serializable]
    public class VideoInformation : InformationBase
    {
        public string name;

        public bool loop;

        public float time;

        public string path;
    }
}