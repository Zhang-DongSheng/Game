using Data;
using System;
using System.Collections.Generic;

namespace UnityEngine
{
    public class Factory : MonoSingleton<Factory>
    {
        private readonly Dictionary<string, Operator> operators = new Dictionary<string, Operator>();

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
#if UNITY_EDITOR
            DataResource data = Resources.Load<DataResource>("Resource");

            if (data != null)
            {
                for (int i = 0; i < data.resources.Count; i++)
                {
                    ResourceInformation res = data.resources[i];

                    switch (res.type)
                    {
                        case ResourceType.Prefab:
                            Instance.operators.Add(res.key, new GameObjectOperator(res));
                            break;
                        case ResourceType.Asset:
                            Instance.operators.Add(res.key, new AssetOperator(res));
                            break;
                        case ResourceType.Atlas:
                            Instance.operators.Add(res.key, new AtlasOperator(res));
                            break;
                        case ResourceType.Audio:
                            Instance.operators.Add(res.key, new AudioOperator(res));
                            break;
                        case ResourceType.Texture:
                            Instance.operators.Add(res.key, new TextureOperator(res));
                            break;
                    }
                }
            }
#endif
        }

        public void Pop(string key, Action<Object> action)
        {
            if (operators.ContainsKey(key))
            {
                operators[key].Pop(action);
            }
            else
            {
                Debug.LogWarningFormat("can't have the asset {0}", key);
            }
        }

        public void Push(string key, Object asset)
        {
            if (operators.ContainsKey(key))
            {
                operators[key].Push(asset);
            }
            else
            {
                GameObject.Destroy(asset);
            }
        }

        public void Remove(string key)
        {
            if (operators.ContainsKey(key))
            {
                operators.Remove(key);
            }
            Debug.LogErrorFormat("the asset is error", key);
        }
    }
}