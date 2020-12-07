using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

namespace UnityEngine
{
    public static class RenewableUtils
    {
        private static readonly Dictionary<string, List<Image>> compontents = new Dictionary<string, List<Image>>();

        public static void SetImage(Image image, string key, RPKey cache = RPKey.None, Action<string> callback = null)
        {
            string parameter = Path.GetFileNameWithoutExtension(key);

            if (RenewablePool.Instance.Exist(cache, key, string.Empty))
            {
                Object asset = RenewablePool.Instance.Pop<Object>(cache, key + parameter);

                SetImage(image, asset as Texture2D);

                callback?.Invoke(key);
            }
            else
            {
                if (compontents.ContainsKey(key))
                {
                    compontents[key].Add(image);
                }
                else
                {
                    compontents.Add(key, new List<Image>() { image });
                }

                RenewableResource.Instance.Get(new RenewableRequest(key, parameter, 0, StorageClass.Write, DownloadFileType.None), (handle) =>
                {
                    if (RenewablePool.Instance.Exist(cache, handle.key + handle.parameter, handle.secret))
                    {
                        Object asset = RenewablePool.Instance.Pop<Object>(cache, handle.key + handle.parameter);

                        if (asset != null && compontents.ContainsKey(handle.key))
                        {
                            SetImages(compontents[handle.key], asset as Texture2D);

                            callback?.Invoke(handle.key);

                            compontents.Remove(handle.key);
                        }
                    }
                    else
                    {
                        RenewableAssetBundle.Instance.LoadAsync(handle.key, handle.buffer, handle.parameter, (asset) =>
                        {
                            if (asset != null)
                            {
                                if (asset != null && compontents.ContainsKey(handle.key))
                                {
                                    SetImages(compontents[handle.key], asset as Texture2D);

                                    callback?.Invoke(handle.key);

                                    compontents.Remove(handle.key);
                                }
                                RenewablePool.Instance.Push(cache, handle.key + handle.parameter, handle.secret, handle.recent, asset);
                            }
                        });
                    }
                }, null);
            }
        }

        public static void SetImages(List<Image> images, Texture2D texture)
        {
            for (int i = 0; i < images.Count; i++)
            {
                if (images[i].sprite != null)
                {
                    UnityEngine.GameObject.Destroy(images[i].sprite);
                }
            }

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

            for (int i = 0; i < images.Count; i++)
            {
                images[i].sprite = sprite;
            }
        }

        public static void SetImage(Image image, Texture2D texture)
        {
            if (image.sprite != null)
            {
                UnityEngine.GameObject.Destroy(image.sprite);
            }

            image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
        }
    }
}