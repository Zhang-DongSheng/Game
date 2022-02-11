using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine.Renewable
{
    public class RenewableAssetBundle : MonoSingleton<RenewableAssetBundle>
    {
        private readonly Dictionary<string, Action<Object>> downloading = new Dictionary<string, Action<Object>>();

        public void Load(string key, byte[] buffer, string name, Action<Object> callBack)
        {
            if (buffer != null)
            {
                AssetBundle bundle = AssetBundle.LoadFromMemory(buffer);

                Object asset = bundle.LoadAsset<Object>(name);

                callBack.Invoke(asset);
            }
        }

        public void LoadAsync(string key, byte[] buffer, string name, Action<Object> callBack)
        {
            if (downloading.ContainsKey(key))
            {
                downloading[key] += callBack;
            }
            else
            {
                downloading.Add(key, callBack);

                StartCoroutine(LoadAssetBundleAsync(key, buffer, name));
            }
        }

        IEnumerator LoadAssetBundleAsync(string key, byte[] buffer, string name)
        {
            if (buffer == null) yield break;

            AssetBundle check = null;

            IEnumerator<AssetBundle> list = AssetBundle.GetAllLoadedAssetBundles().GetEnumerator();

            while (list.MoveNext())
            {
                if (list.Current == null)
                {
                    break;
                }
                else
                {
                    if (list.Current.name == name)
                    {
                        Debug.LogWarning("<color=red>has same bundel : </color>" + name);
                        check = list.Current;
                        break;
                    }
                }
            }
            if (check != null)
            {
                check.Unload(false);
            }

            AssetBundleCreateRequest create = AssetBundle.LoadFromMemoryAsync(buffer);

            yield return create;

            if (create.assetBundle == null) yield break;

            if (create.assetBundle.Contains(name))
            {
                AssetBundleRequest request = create.assetBundle.LoadAssetAsync<Object>(name);

                yield return request;

                if (downloading.ContainsKey(key))
                {
                    downloading[key]?.Invoke(request.asset);
                    downloading.Remove(key);
                }
                create.assetBundle.Unload(false);
            }
            else
            {
                create.assetBundle.Unload(true);
            }
            yield return null;
        }
    }
}