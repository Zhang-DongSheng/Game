using Game.UI;
using System;
using System.Linq;
using System.Reflection;

namespace Game
{
    public class HotfixLogic : Singleton<HotfixLogic>, ILogic
    {
        private const string HotfixProject = "HotUpdate";

        private Assembly assembly;

        public void Initialize()
        {
            
        }

        public void Detection()
        {
            // Editor环境下，HotUpdate.dll.bytes已经被自动加载，不需要加载，重复加载反而会出问题。
#if !UNITY_EDITOR
            assembly = Assembly.Load(File.ReadAllBytes($"{Application.streamingAssetsPath}/{HotfixProject}.dll.bytes"));
#else
            // Editor下无需加载，直接查找获得HotUpdate程序集
            assembly = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == HotfixProject);
#endif
            Type type = assembly.GetType("Hello");

            type.GetMethod("Run").Invoke(null, null);

            ScheduleLogic.Instance.Update(Schedule.Hotfix);
        }

        public Type GetType(string name)
        {
            return assembly.GetType(name);
        }

        public void Release()
        {
            
        }
    }
}