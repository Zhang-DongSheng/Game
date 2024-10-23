using Data;
using Game.Resource;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace Game.UI
{
    public class SpriteManager : Singleton<SpriteManager>
    {
        private readonly Dictionary<string, string> dictionary = new Dictionary<string, string>();

        private readonly Dictionary<string, SpriteAtlas> atlases = new Dictionary<string, SpriteAtlas>();

        public Sprite GetSprite(string name)
        {
            string path;

            if (dictionary.TryGetValue(name, out string key))
            {
                path = string.Empty;
            }
            else
            {
                var data = DataSprite.GetAtlas(name);

                if (data == null) return null;

                key = data.name;

                path = data.path;

                dictionary.Add(name, key);
            }

            if (atlases.TryGetValue(key, out SpriteAtlas atlas))
            {
                return atlases[key].GetSprite(name);
            }
            else
            {
                atlas = ResourceManager.Load<SpriteAtlas>(path);

                if (atlas == null) return null;

                atlases.Add(key, atlas);

                return atlas.GetSprite(name);
            }
        }
    }
}