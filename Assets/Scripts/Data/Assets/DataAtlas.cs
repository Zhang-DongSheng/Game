using System.Collections.Generic;

namespace Data
{
    public class DataAtlas : DataBase
    {
        public List<AtlasInformation> atlases;

        public AtlasInformation Get(string sprite)
        {
            foreach (var atlas in atlases)
            {
                if (atlas.Exist(sprite))
                {
                    return atlas;
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