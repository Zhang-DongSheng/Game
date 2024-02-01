using System.Collections.Generic;

namespace Data
{
    public class DataSprite : DataBase
    {
        public List<AtlasInformation> atlases = new List<AtlasInformation>();

        public static AtlasInformation GetAtlas(string sprite)
        {
            var data = DataManager.Instance.Load<DataSprite>();

            if (data != null)
            {
                foreach (var atlas in data.atlases)
                {
                    if (atlas.Exist(sprite))
                    {
                        return atlas;
                    }
                }
            }
            return null;
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