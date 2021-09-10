using ILRuntime;
using System;
using System.Collections;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

namespace Game
{
    public class ILRuntimeLogic : MonoSingleton<ILRuntimeLogic>, ILogic
    {
        public const string KEY = "HotFix/Hotfix";

        private readonly MemoryStream[] stream = new MemoryStream[2];

        private ILRuntime.Runtime.Enviorment.AppDomain appdomain;

        public void Init()
        {
            if (Application.isPlaying)
            {
                StartCoroutine(LoadILRuntime());
            }
        }

        private IEnumerator LoadILRuntime()
        {
            appdomain = new ILRuntime.Runtime.Enviorment.AppDomain();

            byte[] _dll = null, _pdb = null;

            #region DLL
            string url = string.Format("{0}/{1}.dll", Application.streamingAssetsPath, KEY);

            UnityWebRequest request = UnityWebRequest.Get(url);

            yield return request.SendWebRequest();

            if (string.IsNullOrEmpty(request.error))
            {
                switch (request.result)
                {
                    case UnityWebRequest.Result.Success:
                        {
                            _dll = request.downloadHandler.data;
                        }
                        break;
                }
            }
            else
            {
                Debug.LogError(request.error);
            }
            #endregion

            #region PDB
            url = string.Format("{0}/{1}.pdb", Application.streamingAssetsPath, KEY);

            request = UnityWebRequest.Get(url);

            yield return request.SendWebRequest();

            if (string.IsNullOrEmpty(request.error))
            {
                switch (request.result)
                {
                    case UnityWebRequest.Result.Success:
                        {
                            _pdb = request.downloadHandler.data;
                        }
                        break;
                }
            }
            else
            {
                Debug.LogError(request.error);
            }
            #endregion

            try
            {
                stream[0] = new MemoryStream(_dll);

                stream[1] = new MemoryStream(_pdb);

                appdomain.LoadAssembly(stream[0], stream[1], new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            OnILRuntimeInitialized();
        }

        private void OnILRuntimeInitialized()
        {
#if UNITY_EDITOR || UNITY_ANDROID || UNITY_IPHONE
            appdomain.UnityMainThreadID = Thread.CurrentThread.ManagedThreadId;
#endif
            appdomain.RegisterCrossBindingAdaptor(new MonoBehaviourAdapter());

            CLRRedirection();

            appdomain.Invoke("Hotfix.Script.Program", "Initialize", null);
        }

        unsafe void CLRRedirection()
        {
            var arr = typeof(GameObject).GetMethods();

            foreach (var method in arr)
            {
                if (method.GetGenericArguments().Length == 1)
                {
                    switch (method.Name)
                    {
                        case "AddComponent":
                            {
                                appdomain.RegisterCLRMethodRedirection(method, ILRuntimeCLR.AddComponent);
                            }
                            break;
                        case "GetComponent":
                            {
                                appdomain.RegisterCLRMethodRedirection(method, ILRuntimeCLR.GetComponent);
                            }
                            break;
                    }
                }
            }
        }

        private void OnDestroy()
        {
            for (int i = 0; i < stream.Length; i++)
            {
                if (stream[i] != null)
                {
                    stream[i].Dispose();
                }
                stream[i] = null;
            }
        }
    }
}