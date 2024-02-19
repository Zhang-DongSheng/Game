using System;
using Object = UnityEngine.Object;

namespace Game.Resource
{
    public static class ResourceManager
    {
        private static AssetsController controller;

        private static LoadingType type;
#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
        private static void Initialize()
        {
            Initialize(LoadingType.AssetDatabase);
        }
#endif
        public static void Initialize(LoadingType type)
        {
            ResourceManager.type = type;

            controller = new AssetsController(type);
        }

        public static void UpdateDependencies()
        {
            controller.UpdateDependencies();
        }

        public static Object Load(string name)
        {
            string path = ResourceConfig.Path(type, name);

            AssetsResponse assets = controller.Load(path);

            if (assets != null && assets.MainAsset != null)
            {
                return assets.MainAsset;
            }
            return null;
        }

        public static T Load<T>(string name) where T : Object
        {
            string path = ResourceConfig.Path(type, name);

            AssetsResponse assets = controller.Load(path);

            if (assets != null && assets.MainAsset != null)
            {
                return assets.MainAsset as T;
            }
            return null;
        }

        public static void LoadAsync(string name, Action<Object> callback)
        {
            string path = ResourceConfig.Path(type, name);

            controller.LoadAsync(path, (result) =>
            {
                if (result != null && result.MainAsset != null)
                {
                    callback?.Invoke(result.MainAsset);
                }
                else
                {
                    callback?.Invoke(default);
                }
            });
        }

        public static void LoadAsync<T>(string name, Action<T> callback) where T : Object
        {
            string path = ResourceConfig.Path(type, name);

            controller.LoadAsync(path, (result) =>
            {
                if (result != null && result.MainAsset != null)
                {
                    callback?.Invoke(result.MainAsset as T);
                }
                else
                {
                    callback?.Invoke(default);
                }
            });
        }

        public static void UnLoadAsset(string name)
        {
            string path = ResourceConfig.Path(type, name);

            controller.Unload(path);

            GC.Collect();
        }

        public static void UnLoadAllAsset()
        {
            controller.Unload();

            GC.Collect();
        }
    }
}