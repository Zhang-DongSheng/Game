using Game;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace UnityEditor
{
    public class AssetBundleHelper
    {
        const BuildAssetBundleOptions Options = BuildAssetBundleOptions.ChunkBasedCompression;

        const BuildTarget TARGET = BuildTarget.Android;

        [MenuItem("Assets/Build AssetBundle")]
        protected static void BuildAssetToAssetBundle()
        {
            string path = string.Format("{0}/{1}/Select", Utility.Path.Project, GameConfig.AssetBundle);

            Utility.Document.CreateDirectory(path);

            if (Selection.objects != null && Selection.objects.Length > 0)
            {
                List<AssetBundleBuild> builds = new List<AssetBundleBuild>();

                for (int i = 0; i < Selection.objects.Length; i++)
                {
                    AssetBundleBuild build = new AssetBundleBuild()
                    {
                        assetBundleName = Selection.objects[i].name,
                        assetNames = new string[] { AssetDatabase.GetAssetPath(Selection.objects[i]) }
                    };
                    builds.Add(build);
                }
                BuildPipeline.BuildAssetBundles(path, builds.ToArray(), Options, TARGET);
            }
        }

        [MenuItem("AssetBundle/Raname")]
        protected static void Rename()
        {
            AssetBundleRename.SetAssetBundles(string.Format("{0}/{1}", Application.dataPath, GameConfig.Resource), (path) =>
            {
                string name = path.Remove(0, Application.dataPath.Length + 1);

                if (name.Contains("."))
                {
                    name = name.Substring(0, name.IndexOf("."));
                }
                return name.ToLower();
            });
            AssetDatabase.Refresh();
        }

        [MenuItem("AssetBundle/Build")]
        protected static void Build()
        {
            string path = string.Format("{0}/{1}/{2}", Utility.Path.Project, GameConfig.AssetBundle, TARGET);

            Utility.Document.CreateDirectory(path, true);

            BuildPipeline.BuildAssetBundles(path, Options, TARGET);

            string src = string.Format("{0}/{1}", path, TARGET.ToString());

            string dst = string.Format("{0}/{1}", path, GameConfig.Manifest);

            Utility.Document.Rename(src, dst);
        }

        [MenuItem("AssetBundle/Record")]
        protected static void Record()
        {
            string path = string.Format("{0}/{1}/{2}", Utility.Path.Project, GameConfig.AssetBundle, TARGET);

            string content = string.Empty;

            DirectoryInfo directory = new DirectoryInfo(path);

            FileInfo[] files = directory.GetFiles("*", SearchOption.AllDirectories);

            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Extension != ".manifest")
                {
                    string key = files[i].FullName.Remove(0, path.Length + 1);

                    if (key == GameConfig.Record) continue;

                    string value = Utility.MD5.ComputeFile(files[i].FullName);

                    content += string.Format("{0}|{1}\n\r", key, value);
                }
            }
            Utility.Document.Write(string.Format("{0}/{1}", path, GameConfig.Record), content);
        }
    }
}