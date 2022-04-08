using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;

namespace Game.Resource
{
    public class ResourceManager : MonoSingleton<ResourceManager>
    {
        private readonly List<ResRequest> requests = new List<ResRequest>();

        public void Load(ResRequest request)
        {
            if (requests.Exists(x => x.key == request.key))
            {
                return;
            }
            requests.Add(request); Next();
        }

        private void Next()
        {
            if (requests.Count > 0)
            {
                ResRequest request = requests[0];

                requests.RemoveAt(0);

                StartCoroutine(Loading(request));
            }
        }

        private IEnumerator Loading(ResRequest res)
        {
            Object asset = AssetDatabase.LoadAssetAtPath<Object>(res.url);

            res.success?.Invoke(asset);

            yield return null;
        }

        private IEnumerator Download(ResRequest res)
        {
            UnityWebRequest request = UnityWebRequest.Get(res.url);

            yield return request.SendWebRequest();

            switch (request.result)
            {
                case UnityWebRequest.Result.InProgress:
                    {
                        
                    }
                    break;
                case UnityWebRequest.Result.Success:
                    {
                        //res.success?.Invoke(request.downloadHandler.data);
                    }
                    break;
                default:
                    {
                        Debuger.LogError(Author.Resource, request.error);
                    }
                    break;
            }
        }
    }
}