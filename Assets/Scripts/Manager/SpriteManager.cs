using Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace Game
{
    public class SpriteManager : Singleton<SpriteManager>
    {
        private readonly Dictionary<string, SpriteAtlas> atlas = new Dictionary<string, SpriteAtlas>();

        public void SetSprite(Image image, string name)
        {
            DataManager.Instance.LoadAsync<DataSprite>((asset) =>
            {
                SpriteInformation information = asset.Get(name);

                if (information == null) return;

                if (atlas.ContainsKey(information.atlas))
                {
                    image.sprite = atlas[information.atlas].GetSprite(information.name);
                }
                else
                {
                    
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
