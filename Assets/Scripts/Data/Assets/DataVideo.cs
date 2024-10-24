using System.Collections.Generic;
using UnityEngine;

namespace Game.Data
{
    public class DataVideo : DataBase
    {
        public List<VideoInformation> list;

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

        public string path;
    }
}