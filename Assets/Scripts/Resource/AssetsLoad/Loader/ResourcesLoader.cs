using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Resource
{
    public sealed class ResourcesLoader : Loader
    {
        public override AssetsResponse LoadAssets(string path)
        {
            Object asset = Resources.Load(path);

            if (asset != null)
            {
                return new AssetsResponse(path)
                {
                    Assets = new Object[] { asset }
                };
            }
            return null;
        }

        public override IEnumerator LoadAssetsAsync(string path, Action<AssetsResponse> callback)
        {
            ResourceRequest request = Resources.LoadAsync(path);
            
            yield return request;

            if (request.asset != null)
            {
                callback?.Invoke(new AssetsResponse(path)
                {
                    Assets = new Object[] { request.asset }
                });
            }
            else
            {
                Debuger.LogWarning(Author.Resource, $"{path}, 加载失败！");
            }
            yield return null;
        }
    }
}