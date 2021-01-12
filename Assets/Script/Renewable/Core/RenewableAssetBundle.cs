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
            if (downloading.ContainsKey(key))
            {
                downloading[key] += callBack;
            }
            else
            {
                downloading.Add(key, callBack);

                AssetBundle bundle = AssetBundle.LoadFromMemory(buffer);

                Object asset = bundle.LoadAsset<Object>(name);

                downloading[key]?.Invoke(asset);
            }
        }

        public void Load(string key, AssetBundle bundle, string name, Action<Object> callBack)
        {
            if (downloading.ContainsKey(key))
            {
                downloading[key] += callBack;
            }
            else
            {
                downloading.Add(key, callBack);

                Object asset = bundle.LoadAsset<Object>(name);

                downloading[key]?.Invoke(asset);
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

        public void LoadAsync(string key, AssetBundle bundle, string name, Action<Object> callBack)
        {
            if (downloading.ContainsKey(key))
            {
                downloading[key] += callBack;
            }
            else
            {
                downloading.Add(key, callBack);

                StartCoroutine(LoadAssetAsync(key, bundle, name));
            }
        }

        IEnumerator LoadAssetBundleAsync(string key, byte[] buffer, string name)
        {
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

            AssetBundleCreateRequest reqAB = AssetBundle.LoadFromMemoryAsync(buffer);

            AssetBundleRequest request = reqAB.assetBundle.LoadAssetAsync<Object>(name);

            if (downloading.ContainsKey(key))
            {
                downloading[key]?.Invoke(request.asset);
                downloading.Remove(key);
            }
            reqAB.assetBundle.Unload(false);

            yield return null;
        }

        IEnumerator LoadAssetAsync(string key, AssetBundle bundle, string name)
        {
            AssetBundleRequest request = bundle.LoadAssetAsync<Object>(name);

            if (downloading.ContainsKey(key))
            {
                downloading[key]?.Invoke(request.asset);
                downloading.Remove(key);
            }
            bundle.Unload(false);

            yield return null;
        }
    }
}