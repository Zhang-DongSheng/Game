using System;
using System.Collections.Generic;

namespace UnityEngine.Factory
{
    public class Factory : MonoSingleton<Factory>
    {
        private readonly Dictionary<string, PrefabInformation> config = new Dictionary<string, PrefabInformation>();

        private readonly Dictionary<string, Workshop> shops = new Dictionary<string, Workshop>();

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            FactoryConfig.Load(Instance.config);
        }

        public Object Pop(string key)
        {
            Object asset = null;

            if (shops.ContainsKey(key))
            {
                asset = shops[key].Pop();
            }
            else if (config.ContainsKey(key))
            {
                Add(config[key]); asset = shops[key].Pop();
            }
            return asset;
        }

        public void Push(string key, Object asset)
        {
            if (asset is GameObject go)
            {
                go.transform.SetParent(transform);
                go.SetActive(false);
            }

            if (shops.ContainsKey(key))
            {
                shops[key].Push(asset);
            }
            else
            {
                GameObject.DestroyImmediate(asset);
            }
        }

        public void Remove(string key)
        {
            if (shops.ContainsKey(key))
            {
                shops[key].Clear();
            }
            shops.Remove(key);
        }

        public void Add(PrefabInformation prefab)
        {
            try
            {
                shops.Add(prefab.key, new Workshop(prefab.path, prefab.capacity));
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        public void Clear()
        {
            foreach (var shop in shops.Values)
            {
                shop.Clear();
            }
            shops.Clear();
        }
    }
    /// <summary>
    /// 预制体信息
    /// </summary>
    [System.Serializable]
    public class PrefabInformation
    {
        public string key;

        public string path;

        public int capacity;

        public string extension;
    }
    /// <summary>
    /// 工厂配置信息
    /// </summary>
    [System.Serializable]
    public class FactoryConfig
    {
        public const string XML = "config";

        public int version;

        public List<PrefabInformation> prefabs = new List<PrefabInformation>();

        public static void Load(Dictionary<string, PrefabInformation> list)
        {
            TextAsset asset = Resources.Load<TextAsset>(XML);

            FactoryConfig config = JsonUtility.FromJson<FactoryConfig>(asset.text);

            list.Clear();

            for (int i = 0; i < config.prefabs.Count; i++)
            {
                if (list.ContainsKey(config.prefabs[i].key))
                {
                    Debug.LogErrorFormat("存在重复Key: <color=blue>{0}</color>, 请重命名该资源", config.prefabs[i].key);
                }
                else
                {
                    list.Add(config.prefabs[i].key, config.prefabs[i]);
                }
            }
        }
    }
}