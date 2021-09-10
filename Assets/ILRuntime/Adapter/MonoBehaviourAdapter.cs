using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using System;
using System.Collections.Generic;

namespace ILRuntime
{
    public class MonoBehaviourAdapter : CrossBindingAdaptor
    {
        public override Type BaseCLRType
        {
            get
            {
                return typeof(UnityEngine.MonoBehaviour);
            }
        }

        public override Type AdaptorType
        {
            get
            {
                return typeof(Adapter);
            }
        }

        public override object CreateCLRInstance(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
        {
            return new Adapter(appdomain, instance);
        }

        public class Adapter : UnityEngine.MonoBehaviour, CrossBindingAdaptorType
        {
            ILTypeInstance instance;
            ILRuntime.Runtime.Enviorment.AppDomain appdomain;

            public Adapter()
            {

            }

            public Adapter(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
            {
                this.appdomain = appdomain;
                this.instance = instance;
                Load();
            }

            public ILTypeInstance ILInstance { get { return instance; } }

            public void Init(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
            {
                this.appdomain = appdomain;
                this.instance = instance;
                Load();
            }

            #region Function
            private readonly Dictionary<string, ILMethod> methods = new Dictionary<string, ILMethod>()
            {
                {"Awake", new ILMethod() },
                {"Start", new ILMethod() },
                {"Update", new ILMethod() },
            };

            private void Load()
            {
                foreach (var method in methods)
                {
                    method.Value.Init(appdomain, instance, method.Key);
                }
            }

            public void Awake()
            {
                methods["Awake"].Invoke();
            }

            private void Start()
            {
                methods["Start"].Invoke();
            }

            private void Update()
            {
                methods["Update"].Invoke();
            }
            #endregion

            public override string ToString()
            {
                IMethod m = appdomain.ObjectType.GetMethod("ToString", 0);
                m = instance.Type.GetVirtualMethod(m);
                if (m == null || m is ILMethod)
                {
                    return instance.ToString();
                }
                else
                    return instance.Type.FullName;
            }
        }
    }
}