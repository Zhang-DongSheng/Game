using ILRuntime;
using ILRuntime.CLR.Method;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using System;
using System.Collections;
using System.Collections.Generic;
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
            //appdomain.RegisterValueTypeBinder(typeof(Vector3), new Vector3Binder());

            SetupCLRRedirection();

            SetupCLRRedirection2();

            appdomain.Invoke("Hotfix.Script.Program", "Initialize", null);
        }

        unsafe void SetupCLRRedirection()
        {
            //�������ͨ��Ӧ��д��InitializeILRuntime������Ϊ����ʾд����
            var arr = typeof(GameObject).GetMethods();
            foreach (var i in arr)
            {
                if (i.Name == "AddComponent" && i.GetGenericArguments().Length == 1)
                {
                    appdomain.RegisterCLRMethodRedirection(i, AddComponent);
                }
            }
        }

        unsafe void SetupCLRRedirection2()
        {
            //�������ͨ��Ӧ��д��InitializeILRuntime������Ϊ����ʾд����
            var arr = typeof(GameObject).GetMethods();
            foreach (var i in arr)
            {
                if (i.Name == "GetComponent" && i.GetGenericArguments().Length == 1)
                {
                    appdomain.RegisterCLRMethodRedirection(i, GetComponent);
                }
            }
        }

        MonoBehaviourAdapter.Adaptor GetComponent(ILType type)
        {
            var arr = GetComponents<MonoBehaviourAdapter.Adaptor>();
            for (int i = 0; i < arr.Length; i++)
            {
                var instance = arr[i];
                if (instance.ILInstance != null && instance.ILInstance.Type == type)
                {
                    return instance;
                }
            }
            return null;
        }

        unsafe static StackObject* AddComponent(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            //CLR�ض����˵���뿴����ĵ��ͽ̳̣����ﲻ��������
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;

            var ptr = __esp - 1;
            //��Ա�����ĵ�һ������Ϊthis
            GameObject instance = StackObject.ToObject(ptr, __domain, __mStack) as GameObject;
            if (instance == null)
                throw new System.NullReferenceException();
            __intp.Free(ptr);

            var genericArgument = __method.GenericArguments;
            //AddComponentӦ������ֻ��1�����Ͳ���
            if (genericArgument != null && genericArgument.Length == 1)
            {
                var type = genericArgument[0];
                object res;
                if (type is CLRType)
                {
                    //Unity�����̵��಻��Ҫ�κ����⴦��ֱ�ӵ���Unity�ӿ�
                    res = instance.AddComponent(type.TypeForCLR);
                }
                else
                {
                    //�ȸ�DLL�ڵ����ͱȽ��鷳���������ǵ��Լ��ֶ�����ʵ��
                    var ilInstance = new ILTypeInstance(type as ILType, false);//�ֶ�����ʵ������ΪĬ�Ϸ�ʽ��new MonoBehaviour������Unity�ﲻ����
                                                                               //����������Adapterʵ��
                    var clrInstance = instance.AddComponent<MonoBehaviourAdapter.Adaptor>();
                    //unity������ʵ����û���ȸ�DLL�����ʵ����������Ҫ�ֶ���ֵ
                    clrInstance.ILInstance = ilInstance;
                    clrInstance.AppDomain = __domain;
                    //���ʵ��Ĭ�ϴ�����CLRInstance����ͨ��AddComponent��������Чʵ�������Ե��ֶ��滻
                    ilInstance.CLRInstance = clrInstance;

                    res = clrInstance.ILInstance;//����ILRuntime��ʵ��Ӧ��ΪILInstance

                    clrInstance.Awake();//��ΪUnity�����������ʱ��û׼�����������ﲹ��һ��
                }

                return ILIntepreter.PushObject(ptr, __mStack, res);
            }

            return __esp;
        }

        unsafe static StackObject* GetComponent(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            //CLR�ض����˵���뿴����ĵ��ͽ̳̣����ﲻ��������
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;

            var ptr = __esp - 1;
            //��Ա�����ĵ�һ������Ϊthis
            GameObject instance = StackObject.ToObject(ptr, __domain, __mStack) as GameObject;
            if (instance == null)
                throw new System.NullReferenceException();
            __intp.Free(ptr);

            var genericArgument = __method.GenericArguments;
            //AddComponentӦ������ֻ��1�����Ͳ���
            if (genericArgument != null && genericArgument.Length == 1)
            {
                var type = genericArgument[0];
                object res = null;
                if (type is CLRType)
                {
                    //Unity�����̵��಻��Ҫ�κ����⴦��ֱ�ӵ���Unity�ӿ�
                    res = instance.GetComponent(type.TypeForCLR);
                }
                else
                {
                    //��Ϊ����DLL�����MonoBehaviourʵ�ʶ������Component����������ֻ��ȫȡ������������
                    var clrInstances = instance.GetComponents<MonoBehaviourAdapter.Adaptor>();
                    for (int i = 0; i < clrInstances.Length; i++)
                    {
                        var clrInstance = clrInstances[i];
                        if (clrInstance.ILInstance != null)//ILInstanceΪnull, ��ʾ����Ч��MonoBehaviour��Ҫ�Թ�
                        {
                            if (clrInstance.ILInstance.Type == type)
                            {
                                res = clrInstance.ILInstance;//����ILRuntime��ʵ��Ӧ��ΪILInstance
                                break;
                            }
                        }
                    }
                }

                return ILIntepreter.PushObject(ptr, __mStack, res);
            }

            return __esp;
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