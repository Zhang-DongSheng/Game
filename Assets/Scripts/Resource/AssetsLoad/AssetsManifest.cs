using System.Collections.Generic;
using UnityEngine;

namespace Game.Resource
{
    public class AssetsManifest
    {
        private AssetBundleManifest manifest;

        private readonly Dictionary<string, string[]> dependencies = new Dictionary<string, string[]>();

        public void UpdateDependencies()
        {
            string path = ResourceConfig.Path(LoadType.AssetBundle, GameConfig.Manifest);

            AssetBundle bundle = AssetBundle.LoadFromFile(path);

            manifest = bundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

            bundle.Unload(false);

            dependencies.Clear();

            string[] assets = manifest.GetAllAssetBundles();

            for (int i = 0; i < assets.Length; i++)
            {
                string[] depends = manifest.GetDirectDependencies(assets[i]);

                if (depends.Length > 0)
                {
                    string[] _dependencies = new string[depends.Length];

                    for (int j = 0; j < depends.Length; j++)
                    {
                        _dependencies[j] = ResourceConfig.Path(LoadType.AssetBundle, depends[j]);
                    }
                    dependencies.Add(ResourceConfig.Path(LoadType.AssetBundle, assets[i]), _dependencies);
                }
            }
        }

        public bool ExistDependencies(string key)
        {
            if (dependencies.ContainsKey(key))
            {
                return true;
            }
            return false;
        }

        public string[] GetDependencies(string key)
        {
            if (dependencies.TryGetValue(key, out string[] value))
            {
                return value;
            }
            return new string[0];
        }
    }
}