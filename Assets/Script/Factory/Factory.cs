using Data;
using System;
using System.Collections.Generic;

namespace UnityEngine.Factory
{
    public class Factory : MonoSingleton<Factory>
    {
        private readonly Dictionary<string, Workshop> shops = new Dictionary<string, Workshop>();

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            DataPrefab data = DataManager.Instance.Load<DataPrefab>("Prefab", "Data/Prefab");

            if (data != null)
            {
                for (int i = 0; i < data.resources.Count; i++)
                {
                    PrefabInformation prefab = data.resources[i];

                    if (true) 
                    {
                        Instance.shops.Add(prefab.key, new Workshop(prefab));


                        Debug.LogError(prefab.key);
                    }
                }
            }

            Debug.LogError("sss");
        }

        public Object Pop(string key)
        {
            return null;
        }

        public void Push(string key, Object asset)
        {
            
        }
    }
}