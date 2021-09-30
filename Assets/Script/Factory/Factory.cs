using Data;
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
                        case ResourceType.GameObject:
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

        public Object Pop(string key, string value = null)
        {
            if (operators.ContainsKey(key))
            {
                return operators[key].Pop(value);
            }
            return null;
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
    }
}