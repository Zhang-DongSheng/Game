using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Callbacks;

namespace Game
{
    public sealed class FixScriptLogic : Singleton<FixScriptLogic>, ILogic
    {
        //[DidReloadScripts] // 最好脚本加载完毕就 hook
        //static void InstallHook()
        //{
        //    if (_hooker == null)
        //    {
        //        Type type = Type.GetType("UnityEditor.LogEntries,UnityEditor.dll");
        //        // 找到需要 Hook 的方法
        //        MethodInfo miTarget = type.GetMethod("Clear", BindingFlags.Static | BindingFlags.Public);

        //        type = typeof(PinnedLog);

        //        // 找到被替换成的新方法
        //        MethodInfo miReplacement = type.GetMethod("NewClearLog", BindingFlags.Static | BindingFlags.NonPublic);

        //        // 这个方法是用来调用原始方法的
        //        MethodInfo miProxy = type.GetMethod("ProxyClearLog", BindingFlags.Static | BindingFlags.NonPublic);

        //        // 创建一个 Hooker 并 Install 就OK啦, 之后无论哪个代码再调用原始方法都会重定向到
        //        //  我们写的方法ヾ(ﾟ∀ﾟゞ)
        //        _hooker = new MethodHooker(miTarget, miReplacement, miProxy);
        //        _hooker.Install();
        //    }
        //}

        public void Initialize()
        {

        }

        public void Release()
        {

        }
    }
}