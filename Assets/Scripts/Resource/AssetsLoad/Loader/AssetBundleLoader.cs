using System;
using System.Collections;
using UnityEngine;

namespace Game.Resource
{
    public class AssetBundleLoader : Loader
    {
        public override AssetsResponse LoadAssets(string path)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                path = path.Replace(@"jar:file://", "");
                path = path.Replace("apk!/assets", "apk!assets");
            }
            AssetBundle bundel = AssetBundle.LoadFromFile(path);

            if (bundel == null) return null;

            return new AssetsResponse(path)
            {
                AssetBundle = bundel,
                Assets = bundel.LoadAllAssets(),
            };
        }

        public override IEnumerator LoadAssetsAsync(string path, Action<AssetsResponse> callback)
        {
            AssetBundleCreateRequest create = AssetBundle.LoadFromFileAsync(path);

            yield return create;

            AssetBundle bundel = create.assetBundle;

            AssetBundleRequest request = bundel.LoadAllAssetsAsync();

            yield return request;

            callback?.Invoke(new AssetsResponse(path)
            {
                AssetBundle = bundel,
                Assets = request.allAssets,
            });
            yield return null;
        }

        public override bool ExistDependencies(string name)
        {
            return base.ExistDependencies(name);
        }

        public override string[] GetAllDependenciesName(string name)
        {
            return base.GetAllDependenciesName(name);
        }
    }
}