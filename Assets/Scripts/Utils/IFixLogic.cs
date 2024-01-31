using IFix.Core;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Game
{
    public class IFixLogic : Singleton<IFixLogic>, ILogic
    {
        private readonly List<string> _assemblies = new List<string>()
        {
            "Assembly-CSharp.patch.bytes",
        };

        public void Initialize()
        {
            RuntimeManager.Instance.StartCoroutine(Load());
        }

        private IEnumerator Load()
        {
            VirtualMachine.Info = (s) => UnityEngine.Debug.Log(s);
            var sw = Stopwatch.StartNew();
            foreach (var assembly in _assemblies)
            {
                string url = string.Format("{0}/IFix/{1}", Application.streamingAssetsPath, assembly);

                var request = UnityWebRequest.Get(url);

                yield return request.SendWebRequest();

                if (string.IsNullOrEmpty(request.error))
                {
                    switch (request.result)
                    {
                        case UnityWebRequest.Result.Success:
                            {
                                PatchManager.Load(new MemoryStream(request.downloadHandler.data));
                            }
                            break;
                    }
                    Debuger.Log(Author.Resource, $"loading {assembly} ...");
                }
                else
                {
                    Debuger.LogError(Author.Data, request.error);
                }
            }
            UnityEngine.Debug.Log("loaded patch, using " + sw.ElapsedMilliseconds + " ms");
        }

        public void Release() { }
    }
}