using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class DataSprite : DataBase
    {
        public List<AtlasInformation> list = new List<AtlasInformation>();

        public static AtlasInformation GetAtlas(string sprite)
        {
            var data = DataManager.Instance.Load<DataSprite>();

            if (data != null)
            {
                foreach (var atlas in data.list)
                {
                    if (atlas.Exist(sprite))
                    {
                        return atlas;
                    }
                }
            }
            return null;
        }

        public override void Detection()
        {
            var dic = new Dictionary<string, int>();

            foreach (var atlas in list)
            {
                int count = atlas.sprites.Count;

                for (int i = 0; i < count; i++)
                {
                    if (dic.ContainsKey(atlas.sprites[i]))
                    {
                        Debuger.LogError(Author.Data, "sprite exist the same key:" + atlas.sprites[i]);
                    }
                    else
                    {
                        dic.Add(atlas.sprites[i], 1);
                    }
                }
            }
        }

        public override void Clear()
        {
            list = new List<AtlasInformation>();
        }
    }
    [System.Serializable]
    public class AtlasInformation : InformationBase
    {
        public string name;

        public string path;

        public List<string> sprites;

        public bool Exist(string sprite)
        {
            if (sprites == null || sprites.Count == 0)
            {
                return false;
            }
            return sprites.Exists(x => x == sprite);
        }
    }
}