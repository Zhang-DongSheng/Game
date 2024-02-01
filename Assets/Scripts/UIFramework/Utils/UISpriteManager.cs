using Data;
using Game.Resource;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace Game.UI
{
    public class UISpriteManager : Singleton<UISpriteManager>
    {
        private readonly Dictionary<string, SpriteAtlas> atlases = new Dictionary<string, SpriteAtlas>();

        public Sprite GetSprite(string name)
        {
            AtlasInformation information = DataSprite.GetAtlas(name);

            if (information == null) return null;

            if (atlases.ContainsKey(information.name))
            {
                if (atlases[information.name] != null)
                {
                    return atlases[information.name].GetSprite(name);
                }
            }
            else
            {
                var atlas = ResourceManager.Load<SpriteAtlas>(information.path);

                if (atlas == null) return null;

                atlases.Add(information.name, atlas);

                return atlas.GetSprite(name);
            }
            return null;
        }
    }
}