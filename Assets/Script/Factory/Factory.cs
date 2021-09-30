using Data;
using System.Collections.Generic;

namespace UnityEngine
{
    public class Factory : MonoSingleton<Factory>
    {
        private readonly Dictionary<string, Workshop> shops = new Dictionary<string, Workshop>();

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
#if UNITY_EDITOR
            DataResource data = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Package/Data/Resource.asset", typeof(Object)) as DataResource;

            if (data != null)
            {
                for (int i = 0; i < data.resources.Count; i++)
                {
                    ResourceInformation res = data.resources[i];

                    if (true)
                    {
                        Instance.shops.Add(res.key, new Workshop(res));
                    }
                }
            }
#endif
        }

        public Object Pop(string key)
        {
            if (shops.ContainsKey(key))
            {
                return shops[key].Pop();
            }
            return null;
        }

        public void Push(string key, Object asset)
        {
            if (shops.ContainsKey(key))
            {
                shops[key].Push(asset);
            }
            else
            {
                GameObject.Destroy(asset);
            }
        }
    }
}