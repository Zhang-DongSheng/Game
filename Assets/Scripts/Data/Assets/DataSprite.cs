using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class DataSprite : DataBase
    {
        public List<SpriteInformation> sprites;

        public SpriteInformation Get(string key)
        {
            return sprites.Find(x => x.name == key);
        }
    }
    [System.Serializable]
    public class SpriteInformation : InformationBase
    {
        public string name;

        public string atlas;

        public Sprite sprite;
    }
}