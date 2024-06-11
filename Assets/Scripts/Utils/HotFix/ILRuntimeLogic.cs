using ILRuntime;
using System;
using System.Collections;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

namespace Game
{
    public class ILRuntimeLogic : Singleton<ILRuntimeLogic>
    {
        public const string path = "ILRuntime/Hotfix";

        private ILRuntime.Runtime.Enviorment.AppDomain appdomain;

        private readonly MemoryStream[] stream = new MemoryStream[2];

        public ILRuntime.Runtime.Enviorment.AppDomain AppDomain
        {
            get
            {
                return appdomain;
            }
        }

        public void Initialize()
        {
            RuntimeManager.Instance.StartCoroutine(LoadILRuntime());
        }

        private IEnumerator LoadILRuntime()
        {
            appdomain = new ILRuntime.Runtime.Enviorment.AppDomain();

            byte[] _dll = null, _pdb = null;

            #region DLL
            string url = string.Format("{0}/{1}.dll", Application.streamingAssetsPath, path);

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
            url = string.Format("{0}/{1}.pdb", Application.streamingAssetsPath, path);

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

            ScheduleLogic.Instance.Update(Schedule.Hotfix);
        }

        private void OnILRuntimeInitialized()
        {
#if DEBUG && !NO_PROFILER
            appdomain.UnityMainThreadID = Thread.CurrentThread.ManagedThreadId;
#endif
            appdomain.RegisterCrossBindingAdaptor(new MonoBehaviourAdapter());

            CLRRedirection();

            string KEY = "Program";

            switch (KEY)
            {
                case "Program":
                    {
                        appdomain.Invoke("ILRuntime.Game.Program", "Initialize", null);
                    }
                    break;
                case "Main.Test":
                    {
                        object script = appdomain.Instantiate("ILRuntime.Main");

                        appdomain.Invoke("ILRuntime.Game.Main", "Test", script);
                    }
                    break;
                case "Main.GetSet":
                    {
                        var script = appdomain.LoadedTypes["ILRuntime.Game.Main"];

                        var type = script.ReflectionType;

                        var ctor = type.GetConstructor(new System.Type[0]);

                        var obj = ctor.Invoke(null);

                        var field = type.GetField("index", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                        field.SetValue(obj, 123456);

                        var value = field.GetValue(obj);

                        Debug.Log("ID = " + value);
                    }
                    break;
                default:
                    break;
            }
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

        public void Release()
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