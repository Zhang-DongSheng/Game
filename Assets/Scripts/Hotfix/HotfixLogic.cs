using Game.UI;
using System;
using System.Linq;
using System.Reflection;

namespace Game
{
    public class HotfixLogic : Singleton<HotfixLogic>, ILogic
    {
        private const string HotfixProject = "HotUpdate";

        public void Initialize()
        {
            
        }

        public void Detection()
        {
            // Editor�����£�HotUpdate.dll.bytes�Ѿ����Զ����أ�����Ҫ���أ��ظ����ط���������⡣
#if !UNITY_EDITOR
            Assembly assembly = Assembly.Load(File.ReadAllBytes($"{Application.streamingAssetsPath}/{HotfixProject}.dll.bytes"));
#else
            // Editor��������أ�ֱ�Ӳ��һ��HotUpdate����
            Assembly assembly = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == HotfixProject);
#endif
            Type type = assembly.GetType("Hello");

            type.GetMethod("Run").Invoke(null, null);

            ScheduleLogic.Instance.Update(Schedule.Hotfix);
        }

        public void Release()
        {
            
        }
    }
}