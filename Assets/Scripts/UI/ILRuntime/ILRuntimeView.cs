using Data;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Intepreter;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class ILRuntimeView : ViewBase
    {
        private ILRuntime.Runtime.Enviorment.AppDomain appDomain;

        private ILTypeInstance script;

        private readonly Dictionary<string, IMethod> methods = new Dictionary<string, IMethod>()
        {
            { "Init", null},
            { "Refresh", null},
            { "Release", null},
        };

        public override void Init(UIInformation information)
        {
            base.Init(information);

            string name = string.Format("ILRuntime.Game.UI.{0}", information.name);

            this.appDomain = ILRuntimeLogic.Instance.AppDomain;

            script = appDomain.Instantiate(name);

            if (script == null)
            {
                Debuger.LogError( Author.ILRuntime, name + " is null!");
                return;
            }
            var list = new List<string>(methods.Keys);

            foreach (var key in list)
            {
                int count;

                switch (key)
                {
                    case "Refresh":
                        count = 1;
                        break;
                    default:
                        count = 0;
                        break;

                }
                methods[key] = script.Type.GetMethod(key, count);
            }

            if (methods.TryGetValue("Init", out IMethod method) && method != null)
            {
                appDomain.Invoke(method, script);
            }
        }

        public override void Refresh(UIParameter parameter)
        {
            string key = "Refresh";

            if (methods.TryGetValue(key, out IMethod method) && method != null)
            {
                appDomain.Invoke(method, script, parameter);
            }
        }

        public override void Release()
        {
            string key = "Release";

            if (methods.TryGetValue(key, out IMethod method))
            {
                appDomain.Invoke(method, script);
            }
            base.Release();
        }
    }
}