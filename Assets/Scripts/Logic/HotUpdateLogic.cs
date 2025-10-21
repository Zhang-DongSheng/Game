using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Game.Logic
{
    public class HotUpdateLogic : Singleton<HotUpdateLogic>, ILogic
    {
        private const string NAME = "Assembly-HotUpdate";

        private Assembly hotUpdateAssembly;

        public Assembly HotUpdateAssembly
        {
            get
            {
                return hotUpdateAssembly;
            }
        }

        public void Initialize()
        {
            
        }

        public void LoadILRuntime()
        {
#if UNITY_EDITOR
            hotUpdateAssembly = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == NAME);
#else
            var path = $"{Application.streamingAssetsPath}/{NAME}.dll.bytes";

            var buffer = File.ReadAllBytes(path);

            hotUpdateAssembly = Assembly.Load(buffer);
#endif
            Type type = hotUpdateAssembly.GetType("Game.HotUpdate.Hello");

            type.GetMethod("Run").Invoke(null, null);

            ScheduleLogic.Instance.Update(Schedule.Hotfix);
        }

        public void Release()
        {
            hotUpdateAssembly = null;
        }
    }
}