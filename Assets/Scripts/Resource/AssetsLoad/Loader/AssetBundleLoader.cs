using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Resource
{
    public class AssetBundleLoader : Loader
    {
        public override AssetsData LoadAssets(string path)
        {
            throw new NotImplementedException();
        }

        public override IEnumerator LoadAssetsAsync(string path, Action<AssetsData> callback)
        {
            throw new NotImplementedException();
        }
    }
}