using System;
using System.Collections.Generic;

namespace UnityEngine.Factory
{
    public class Factory : MonoSingleton<Factory>
    {
        private readonly Dictionary<string, PrefabInformation> config = new Dictionary<string, PrefabInformation>();

        private readonly Dictionary<string, Transform> parents = new Dictionary<string, Transform>();

        private readonly Dictionary<string, Workshop> shops = new Dictionary<string, Workshop>();

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            FactoryConfig.Init(Instance.config);
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
                Add(config[key]);

                asset = shops[key].Pop();
            }
            return asset;
        }

        public void Push(string key, Object asset)
        {
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
                shops.Add(prefab.key, new Workshop(prefab.path, Parent(prefab.key, prefab.extension), prefab.capacity));
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

        private Transform Parent(string name, string extension)
        {
            string key;

            switch (extension)
            {
                case ".prefab":
                    if (name.StartsWith("UI"))
                    {
                        key = "UI";
                    }
                    else
                    {
                        key = "Prefab";
                    }
                    break;
                default:
                    key = "Root";
                    break;
            }

            if (string.IsNullOrEmpty(key)) return null;

            if (parents.ContainsKey(key))
            {
                return parents[key];
            }
            else
            {
                Transform parent = new GameObject(key).transform;

                parent.SetParent(transform);

                parents.Add(key, parent);

                return parent;
            }
        }
    }

    public enum Parent
    {
        None,
        Cube,
        UI,
        Special,
    }
    [System.Serializable]
    public class PrefabInformation
    {
        public string key;

        public string path;

        public int capacity;

        public string extension;
    }
    [System.Serializable]
    public class FactoryConfig
    {
        public const string XML = "config";

        public int version;

        public List<PrefabInformation> prefabs = new List<PrefabInformation>();

        public static void Init(Dictionary<string, PrefabInformation> list)
        {
            TextAsset asset = Resources.Load<TextAsset>(XML);

            FactoryConfig config = JsonUtility.FromJson<FactoryConfig>(asset.text);

            list.Clear();

            for (int i = 0; i < config.prefabs.Count; i++)
            {
                list.Add(config.prefabs[i].key, config.prefabs[i]);
            }
        }
    }
}