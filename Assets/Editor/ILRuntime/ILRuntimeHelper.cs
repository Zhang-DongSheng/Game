using Game;
using ILRuntime;
using System.IO;
using UnityEngine;

namespace UnityEditor
{
    public class ILRuntimeHelper
    {
        private static readonly string PATH = string.Format("{0}/{1}", Application.dataPath, "ILRuntime");

        private static string path;

        [MenuItem("ILRuntime/����Ŀ")]
        static void OpenProject()
        {
            path = string.Format("{0}/Hotfix~/Hotfix.sln", PATH);

            EditorUtility.OpenWithDefaultApp(path);
        }

        [MenuItem("ILRuntime/���ɿ���̳�������")]
        static void GenerateCrossbindAdapter()
        {
            GenerateCrossbindAdapter<MonoBehaviour>();

            GenerateCrossbindAdapter<Transform>();

            AssetDatabase.Refresh();
        }

        static void GenerateCrossbindAdapter<T>()
        {
            path = string.Format("{0}/Adapter/{1}Adapter.cs", PATH, typeof(T).Name);

            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine(ILRuntime.Runtime.Enviorment.CrossBindingCodeGenerator.GenerateCrossBindingAdapterCode(typeof(T), "ILRuntime"));
            }
        }

        [MenuItem("ILRuntime/Generate CLR Binding Code by Analysis")]
        static void GenerateCLRBindingByAnalysis()
        {
            //���µķ����ȸ�dll�������������ɰ󶨴���
            ILRuntime.Runtime.Enviorment.AppDomain domain = new ILRuntime.Runtime.Enviorment.AppDomain();

            string path = string.Format("{0}/{1}.dll", Application.streamingAssetsPath, ILRuntimeLogic.KEY);

            using (System.IO.FileStream stream = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                domain.LoadAssembly(stream);

                //Crossbind Adapter is needed to generate the correct binding code
                InitILRuntime(domain);
                ILRuntime.Runtime.CLRBinding.BindingCodeGenerator.GenerateBindingCode(domain, "Assets/ILRuntime/Generated");
            }

            AssetDatabase.Refresh();
        }

        static void InitILRuntime(ILRuntime.Runtime.Enviorment.AppDomain domain)
        {
            //������Ҫע�������ȸ�DLL���õ��Ŀ���̳�Adapter�������޷���ȷץȡ����
            domain.RegisterCrossBindingAdaptor(new MonoBehaviourAdapter());
        }
    }
}