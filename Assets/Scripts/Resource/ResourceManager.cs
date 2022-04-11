using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

namespace Game.Resource
{
    public static class ResourceManager
    {
        private static AssetsController controller;

        public static void Initialize()
        {
            controller = new AssetsController(LoadType.AssetDatabase);
        }

        public static Object Load(string name)
        {
            string path = ResourceConfig.Path(name);

            AssetsData assets = controller.Load(path);

            if (assets != null)
            {
                return assets.Assets[0];
            }
            return null;
        }

        public static T Load<T>(string name) where T : Object
        {
            string path = ResourceConfig.Path(name);

            AssetsData assets = controller.Load(path);

            if (assets != null)
            {
                return assets.Assets[0] as T;
            }
            return null;
        }

        public static void LoadAsync(string name, Action<Object> callback)
        {
            string path = ResourceConfig.Path(name);

            controller.LoadAsync(path, (result) =>
            {
                callback?.Invoke(result.Assets[0]);
            });
        }

        public static void LoadAsync<T>(string name, Action<T> callback) where T : Object
        {
            string path = ResourceConfig.Path(name);

            controller.LoadAsync(path, (result) =>
            {
                callback?.Invoke(result.Assets[0] as T);
            });
        }

        public static void UnLoadAsset(string name, bool isDestroy = false)
        {

        }
    }
}