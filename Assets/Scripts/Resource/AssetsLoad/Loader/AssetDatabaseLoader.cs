using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Resource
{
#if UNITY_EDITOR
    public class AssetDatabaseLoader : Loader
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
                Debuger.LogWarning(Author.Resource, string.Format("{0}, º”‘ÿ ß∞‹£°", path));
            }
            yield return null;
        }
    }
#endif
}