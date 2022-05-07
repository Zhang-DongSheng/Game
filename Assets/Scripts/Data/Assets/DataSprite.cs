using System.Collections.Generic;

namespace Data
{
    public class DataSprite : DataBase
    {
        public List<AtlasInformation> atlases = new List<AtlasInformation>();

        public List<SpriteInformation> sprites = new List<SpriteInformation>();

        public List<TextureInformation> textures = new List<TextureInformation>();

        public AtlasInformation GetAtlas(string sprite)
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

        public SpriteInformation GetSprite(string sprite)
        {
            return sprites.Find(x => x.name == sprite);
        }

        public TextureInformation GetTexture2D(string name)
        {
            return textures.Find(x => x.name == name);
        }

        protected override void Editor()
        {
            
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
    [System.Serializable]
    public class SpriteInformation : InformationBase
    {
        public string name;

        public string path;
    }
    [System.Serializable]
    public class TextureInformation : InformationBase
    {
        public string name;

        public string path;
    }
}