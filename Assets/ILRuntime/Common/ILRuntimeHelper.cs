using ILRuntime.CLR.Method;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILRuntime
{
    public class ILRuntimeHelper
    {

    }

    public static class ILRuntimeCLR
    {
        public static unsafe StackObject* AddComponent(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
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
                    var ilInstance = new ILTypeInstance(type as ILType, false);
                    //�ֶ�����ʵ������ΪĬ�Ϸ�ʽ��new MonoBehaviour������Unity�ﲻ����
                    //����������Adapterʵ��
                    var clrInstance = instance.AddComponent<MonoBehaviourAdapter.Adapter>();
                    //unity������ʵ����û���ȸ�DLL�����ʵ����������Ҫ�ֶ���ֵ
                    clrInstance.Init(__domain, ilInstance);
                    //���ʵ��Ĭ�ϴ�����CLRInstance����ͨ��AddComponent��������Чʵ�������Ե��ֶ��滻
                    ilInstance.CLRInstance = clrInstance;

                    res = clrInstance.ILInstance;//����ILRuntime��ʵ��Ӧ��ΪILInstance

                    clrInstance.Awake();//��ΪUnity�����������ʱ��û׼�����������ﲹ��һ��
                }

                return ILIntepreter.PushObject(ptr, __mStack, res);
            }

            return __esp;
        }

        public static unsafe StackObject* GetComponent(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
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
                    var clrInstances = instance.GetComponents<MonoBehaviourAdapter.Adapter>();
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
    }

    public class ILMethod
    {
        private IMethod method;

        private string methodName;

        private ILTypeInstance instance;

        Runtime.Enviorment.AppDomain appDomain;

        public void Init(Runtime.Enviorment.AppDomain appDomain, ILTypeInstance instance, string methodName)
        {
            this.appDomain = appDomain;

            this.instance = instance;

            this.methodName = methodName;
        }

        public void Invoke()
        {
            if (appDomain != null)
            {
                if (method == null)
                {
                    method = instance.Type.GetMethod(methodName, 0);
                }
                appDomain.Invoke(method, instance, null);
            }
        }
    }
}