using System;
using System.Collections.Generic;

namespace Game.Resource
{
    public class AssetsController
    {
        private readonly Dictionary<string, AssetsData> caches = new Dictionary<string, AssetsData>();

        private Loader loader;

        public AssetsController(LoadType type, bool cache = false)
        {
            switch (type)
            {
                case LoadType.Resources:
                    loader = new ResourcesLoader();
                    break;
                case LoadType.AssetBundle:
                    loader = new AssetBundleLoader();
                    break;
                case LoadType.AssetDatabase:
#if UNITY_EDITOR
                    loader = new AssetDatabaseLoader();
#else
                    loader = new ResourcesLoader();
#endif
                    break;
                default:
                    break;
            }
        }

        public AssetsData Load(string path)
        {
            LoadAssetsDependencies(path);

            if (caches.ContainsKey(path))
            {
                return caches[path];
            }
            else
            {
                AssetsData assets = loader.LoadAssets(path);

                if (assets != null)
                {
                    caches.Add(path, assets);
                }
                return assets;
            }
        }

        public void LoadAsync(string path, Action<AssetsData> callback)
        {
            LoadAssetsDependencies(path);

            if (caches.ContainsKey(path))
            {
                callback?.Invoke(caches[path]);
            }
            else
            {
                GameController.Instance.StartCoroutine(loader.LoadAssetsAsync(path, callback));
            }
        }

        private void LoadAssetsDependencies(string path)
        { 
            
        }

        public void Unload()
        { 
            
        }
    }
}
