using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Game
{
    public class ILRuntimeLogic : MonoSingleton<ILRuntimeLogic>, ILogic
    {
        private ILRuntime.Runtime.Enviorment.AppDomain appdomain;

        public void Init()
        {
            StartCoroutine(LoadILRuntime());
        }

        private IEnumerator LoadILRuntime()
        {
            appdomain = new ILRuntime.Runtime.Enviorment.AppDomain();

            byte[] _dll = null, _pdb = null;

            #region DLL
            string url = Application.streamingAssetsPath + "/HotFix/HotFix_Project.dll";

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
            url = Application.streamingAssetsPath + "/HotFix/HotFix_Project.pdb";

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
            appdomain.Invoke("HotFix_Project.InstanceClass", "StaticFunTest", null, null);
        }
    }
}