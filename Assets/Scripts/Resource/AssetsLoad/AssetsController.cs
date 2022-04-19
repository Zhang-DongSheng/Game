using System;
using System.Collections;
using System.Collections.Generic;

namespace Game.Resource
{
    public class AssetsController
    {
        private readonly Dictionary<string, AssetsResponse> caches = new Dictionary<string, AssetsResponse>();

        private readonly List<AssetsRequest> requests = new List<AssetsRequest>();

        private Loader loader;

        private int loading;

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

        public AssetsResponse Load(string path)
        {
            LoadAssetsDependencies(path);

            if (caches.ContainsKey(path))
            {
                return caches[path];
            }
            else
            {
                AssetsResponse assets = loader.LoadAssets(path);

                if (assets != null)
                {
                    caches.Add(path, assets);
                }
                return assets;
            }
        }

        public void LoadAsync(string path, Action<AssetsResponse> callback)
        {
            AssetsRequest request = requests.Find(x => x.path == path);

            if (request != null)
            {
                request.callback += callback;

                UnityEngine.Debug.LogError(path + request.callback.GetInvocationList().Length);
            }
            else
            {
                requests.Add(new AssetsRequest()
                {
                    path = path,
                    callback = callback,
                    status = RequestStatus.Ready,
                });
                LoadAssetsAsync();
            }
        }

        public void Unload(string path)
        {

        }

        private void LoadAssetsDependencies(string path)
        {

        }

        private void LoadAssetsAsync()
        {
            loading = 0;

            int count = requests.Count;

            for (int i = count - 1; i > -1; i--)
            {
                switch (requests[i].status)
                {
                    case RequestStatus.Loading:
                        loading++;
                        break;
                    case RequestStatus.Complete:
                        requests.RemoveAt(i);
                        break;
                }
            }

            if (loading >= 3) return;

            count = requests.Count;

            for (int i = 0; i < count; i++)
            {
                switch (requests[i].status)
                {
                    case RequestStatus.Ready:
                        {
                            GameController.Instance.StartCoroutine(LoadAssetsAsync(requests[i]));
                            loading++;
                        }
                        break;
                }
                if (loading >= 3) break;
            }
        }

        private IEnumerator LoadAssetsAsync(AssetsRequest request)
        {
            request.status = RequestStatus.Loading;

            yield return LoadAssetsDependenciesAsync(request.path);

            if (caches.ContainsKey(request.path))
            {
                request.callback?.Invoke(caches[request.path]);

                request.status = RequestStatus.Complete;

                LoadAssetsAsync();
            }
            else
            {
                yield return loader.LoadAssetsAsync(request.path, (response) =>
                {
                    caches.Add(request.path, response);

                    request.callback?.Invoke(response);

                    request.status = RequestStatus.Complete;

                    LoadAssetsAsync();
                });
            }
        }

        private IEnumerator LoadAssetsDependenciesAsync(string path)
        {
            yield return null;
        }
    }
}