using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Resource
{
    public class ResourcesLoader : Loader
    {
        public override AssetsData LoadAssets(string path)
        {
            Object asset = Resources.Load(path);

            if (asset != null)
            {
                return new AssetsData(path)
                {
                    Assets = new Object[] { asset }
                };
            }
            return null;
        }

        public override IEnumerator LoadAssetsAsync(string path, Action<AssetsData> callback)
        {
            ResourceRequest request = Resources.LoadAsync(path);
            
            yield return request;

            if (request.asset != null)
            {
                callback?.Invoke(new AssetsData(path)
                {
                    Assets = new Object[] { request.asset }
                });
            }
            else
            {
                Debuger.LogWarning(Author.Resource, string.Format("{0}, º”‘ÿ ß∞‹£°", path));
            }
            yield return null;
        }
    }
}