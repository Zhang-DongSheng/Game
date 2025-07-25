using Game.Const;
using ILRuntime;
using System.IO;
using UnityEngine;

namespace UnityEditor
{
    public class ILRuntimeTools
    {
        private readonly static string PATH = string.Format("{0}/{1}", Application.dataPath, "ILRuntime");

        private readonly static string PATH_CLR = "ILRuntime/Generated";

        private static string path;
        [MenuItem("HotFix/ILRuntime/打开项目")]
        protected static void OpenProject()
        {
            path = string.Format("{0}/Hotfix~/Hotfix.sln", PATH);

            EditorUtility.OpenWithDefaultApp(path);
        }
        [MenuItem("HotFix/ILRuntime/生成跨域继承适配器")]
        protected static void GenerateCrossbindAdapter()
        {
            //添加需要生成适配器的类
            GenerateCrossbindAdapter<MonoBehaviour>();

            GenerateCrossbindAdapter<Transform>();

            AssetDatabase.Refresh();
        }

        protected static void GenerateCrossbindAdapter<T>()
        {
            path = string.Format("{0}/Adapter/{1}Adapter.cs", PATH, typeof(T).Name);

            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine(ILRuntime.Runtime.Enviorment.CrossBindingCodeGenerator.GenerateCrossBindingAdapterCode(typeof(T), "ILRuntime"));
            }
        }
        [MenuItem("HotFix/ILRuntime/通过自动分析热更DLL生成CLR绑定")]
        protected static void GenerateCLRBindingByAnalysis()
        {
            ILRuntime.Runtime.Enviorment.AppDomain domain = new ILRuntime.Runtime.Enviorment.AppDomain();

            string path = string.Format("{0}/{1}.dll", Application.streamingAssetsPath, AssetPath.HotfixDll);

            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                domain.LoadAssembly(stream);

                InitILRuntime(domain);

                ILRuntime.Runtime.CLRBinding.BindingCodeGenerator.GenerateBindingCode(domain, string.Format("Assets/{0}", PATH_CLR));
            }
            AssetDatabase.Refresh();
        }

        protected static void InitILRuntime(ILRuntime.Runtime.Enviorment.AppDomain domain)
        {
            //这里需要注册所有热更DLL中用到的跨域继承Adapter，否则无法正确抓取引用
            domain.RegisterCrossBindingAdaptor(new MonoBehaviourAdapter());
        }
        [MenuItem("HotFix/ILRuntime/清除CLR绑定")]
        static public void ClearCLRBinding()
        {
            string path = string.Format("{0}/{1}", Application.dataPath, PATH_CLR);

            if (System.IO.Directory.Exists(path))
            {
                System.IO.Directory.Delete(path, true);
            }
            AssetDatabase.Refresh();
        }
        [MenuItem("HotFix/ILRuntime/打开ILRuntime中文文档")]
        protected static void OpenDocumentation()
        {
            Application.OpenURL("https://ourpalm.github.io/ILRuntime/");
        }
    }
}