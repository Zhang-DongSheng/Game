using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Game
{
    public class ILRuntimeLogic : MonoSingleton<ILRuntimeLogic>, ILogic
    {
        private readonly string key = "HotFix/Hotfix";

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
            string url = string.Format("{0}/{1}.dll", Application.streamingAssetsPath, key);

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
            url = string.Format("{0}/{1}.pdb", Application.streamingAssetsPath, key);

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
                MemoryStream dll = new MemoryStream(_dll);

                MemoryStream pdb = new MemoryStream(_pdb);

                appdomain.LoadAssembly(dll, pdb, new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            OnILRuntimeInitialized();
        }

        private void OnILRuntimeInitialized()
        {
            object result = appdomain.Invoke("Hotfix.Script.Utils.Helper", "Add", null, 1f, 2f);

            Debug.LogError(result);
        }
    }
}