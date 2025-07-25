#if UNITY_EDITOR
using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Resource
{
    public sealed class AssetDatabaseLoader : Loader
    {
        public override AssetsResponse LoadAssets(string path)
        {
            Object asset = UnityEditor.AssetDatabase.LoadAssetAtPath<Object>(path);

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
            Object asset = UnityEditor.AssetDatabase.LoadMainAssetAtPath(path);

            if (asset != null)
            {
                callback?.Invoke(new AssetsResponse(path)
                {
                    Assets = new Object[] { asset }
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
#endif