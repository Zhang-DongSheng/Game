using Game;
using ILRuntime;
using System.IO;
using UnityEngine;

namespace UnityEditor
{
    public class ILRuntimeTools
    {
        private static readonly string PATH = string.Format("{0}/{1}", Application.dataPath, "ILRuntime");

        private static string path;

        [MenuItem("ILRuntime/打开项目")]
        protected static void OpenProject()
        {
            path = string.Format("{0}/Hotfix~/Hotfix.sln", PATH);

            EditorUtility.OpenWithDefaultApp(path);
        }

        [MenuItem("ILRuntime/生成跨域继承适配器")]
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

        [MenuItem("ILRuntime/通过自动分析热更DLL生成CLR绑定")]
        protected static void GenerateCLRBindingByAnalysis()
        {
            ILRuntime.Runtime.Enviorment.AppDomain domain = new ILRuntime.Runtime.Enviorment.AppDomain();

            string path = string.Format("{0}/{1}.dll", Application.streamingAssetsPath, ILRuntimeLogic.KEY);

            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                domain.LoadAssembly(stream);

                InitILRuntime(domain);

                ILRuntime.Runtime.CLRBinding.BindingCodeGenerator.GenerateBindingCode(domain, "Assets/ILRuntime/Generated");
            }
            AssetDatabase.Refresh();
        }

        protected static void InitILRuntime(ILRuntime.Runtime.Enviorment.AppDomain domain)
        {
            //这里需要注册所有热更DLL中用到的跨域继承Adapter，否则无法正确抓取引用
            domain.RegisterCrossBindingAdaptor(new MonoBehaviourAdapter());
        }

        [MenuItem("ILRuntime/打开ILRuntime中文文档")]
        protected static void OpenDocumentation()
        {
            Application.OpenURL("https://ourpalm.github.io/ILRuntime/");
        }
    }
}