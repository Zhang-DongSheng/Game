using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Intepreter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILRuntime
{
    public class ILRuntimeHelper
    {

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