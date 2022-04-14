using Data;
using Game.Resource;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace Game
{
    public class SpriteManager : Singleton<SpriteManager>
    {
        private readonly Dictionary<string, SpriteAtlas> atlases = new Dictionary<string, SpriteAtlas>();

        public void SetSprite(Image image, Quality quality)
        {
            string name = string.Format("quality_{0}", quality);

            SetSprite(image, name);
        }

        public void SetSprite(Image image, string name)
        {
            DataManager.Instance.LoadAsync<DataAtlas>((asset) =>
            {
                AtlasInformation information = asset.Get(name);

                if (information == null) return;

                if (atlases.ContainsKey(information.name))
                {
                    if (atlases[information.name] != null)
                    {
                        image.sprite = atlases[information.name].GetSprite(name);
                    }
                }
                else
                {
                    ResourceManager.LoadAsync<SpriteAtlas>(information.path, (atlas) =>
                    {
                        if (atlas != null)
                        {
                            image.sprite = atlas.GetSprite(name);
                        }
                        atlases.Add(information.name, atlas);
                    });
                }
            });
        }

        public void SetSprite(RawImage image, string key)
        {

        }

        public void SetSprite(Sprite image, string key)
        {

        }
    }
}
