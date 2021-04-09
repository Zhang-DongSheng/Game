using System;

namespace UnityEngine.Renewable
{
    public static class RenewableUtils
    {
        public static void SetImage(string key, Action<Sprite> callback = null)
        {
            string parameter = AssetKey(key);

            if (RenewablePool.Instance.Exist(RPKey.None, key + parameter, string.Empty))
            {
                Object asset = RenewablePool.Instance.Pop<Object>(RPKey.None, key + parameter);

                if (asset != null)
                {
                    callback?.Invoke(ConvertToSprite(asset));
                }
            }
            else
            {
                RenewableResource.Instance.Get(new RenewableRequest(key, parameter, 0, StorageClass.Write, DownloadFileType.None), (handle) =>
                {
                    if (RenewablePool.Instance.Exist(RPKey.None, handle.key + handle.parameter, handle.secret))
                    {
                        Object asset = RenewablePool.Instance.Pop<Object>(RPKey.None, handle.key + handle.parameter);

                        if (asset != null)
                        {
                            callback?.Invoke(ConvertToSprite(asset));
                        }
                    }
                    else
                    {
                        RenewableAssetBundle.Instance.LoadAsync(handle.key, handle.buffer, handle.parameter, (asset) =>
                        {
                            if (asset != null)
                            {
                                callback?.Invoke(ConvertToSprite(asset));

                                RenewablePool.Instance.Push(RPKey.None, handle.key + handle.parameter, handle.secret, handle.recent, asset);
                            }
                        });
                    }
                }, null);
            }
        }

        public static Sprite ConvertToSprite(Object asset)
        {
            if (asset != null && asset is Texture2D texture)
            {
                return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            }
            return null;
        }

        public static string AssetKey(string key)
        {
            if (key.Contains("_"))
            {
                return key.Substring(key.IndexOf('_') + 1);
            }
            return key;
        }
    }
}