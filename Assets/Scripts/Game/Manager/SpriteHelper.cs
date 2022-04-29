using Data;
using Game.Resource;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace Game
{
    public static class SpriteHelper
    {
        private readonly static Dictionary<string, SpriteAtlas> atlases = new Dictionary<string, SpriteAtlas>();

        private readonly static Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();

        private readonly static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        public static void SetQuality(Image image, Quality quality)
        {
            string name = string.Format("quality_{0}", quality);

            SetSprite(image, name);
        }

        public static void SetSprite(Image image, string name)
        {
            DataSprite data = DataManager.Instance.Load<DataSprite>();

            AtlasInformation information = data.GetAtlas(name);

            if (information == null) return;

            if (atlases.ContainsKey(information.name))
            {
                if (atlases[information.name] != null)
                {
                    SetSprite(image, atlases[information.name].GetSprite(name));
                }
            }
            else
            {
                ResourceManager.LoadAsync<SpriteAtlas>(information.path, (atlas) =>
                {
                    if (atlas != null)
                    {
                        SetSprite(image, atlas.GetSprite(name));
                    }
                    Add(information.name, atlas);
                });
            }
        }

        public static void SetSprite(Image image, Sprite sprite)
        {
            if (image != null)
            {
                image.sprite = sprite;
            }
        }

        public static void SetSprite(RawImage image, string name)
        {
            DataSprite data = DataManager.Instance.Load<DataSprite>();

            TextureInformation information = data.GetTexture2D(name);

            if (information == null) return;

            if (textures.ContainsKey(information.name))
            {
                if (textures[information.name] != null)
                {
                    SetSprite(image, textures[information.name]);
                }
            }
            else
            {
                ResourceManager.LoadAsync<Texture2D>(information.path, (texture) =>
                {
                    if (texture != null)
                    {
                        SetSprite(image, texture);
                    }
                    Add(information.name, texture);
                });
            }
        }

        public static void SetSprite(RawImage image, Texture2D texture)
        {
            if (image != null)
            {
                image.texture = texture;
            }
        }

        public static void SetSprite(SpriteRenderer renderer, string name)
        {
            DataSprite data = DataManager.Instance.Load<DataSprite>();

            SpriteInformation information = data.GetSprite(name);

            if (information == null) return;

            if (sprites.ContainsKey(information.name))
            {
                if (sprites[information.name] != null)
                {
                    SetSprite(renderer, sprites[information.name]);
                }
            }
            else
            {
                ResourceManager.LoadAsync<Sprite>(information.path, (sprite) =>
                {
                    if (sprite != null)
                    {
                        SetSprite(renderer, sprite);
                    }
                    Add(information.name, sprite);
                });
            }
        }

        public static void SetSprite(SpriteRenderer renderer, Sprite sprite)
        {
            if (renderer != null)
            {
                renderer.sprite = sprite;
            }
        }

        static void Add(string key, SpriteAtlas atlas)
        {
            if (!atlases.ContainsKey(key))
            {
                atlases.Add(key, atlas);
            }
        }

        static void Add(string key, Texture2D texture)
        {
            if (!textures.ContainsKey(key))
            {
                textures.Add(key, texture);
            }
        }

        static void Add(string key, Sprite sprite)
        {
            if (!sprites.ContainsKey(key))
            {
                sprites.Add(key, sprite);
            }
        }

        static void Remove(string key)
        {
            if (atlases.ContainsKey(key))
            {
                atlases.Remove(key);
            }
        }

        static void Clear()
        {
            atlases.Clear();
        }
    }
}