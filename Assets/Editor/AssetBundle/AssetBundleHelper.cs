using Game;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
    public class AssetBundleHelper
    {
        const BuildAssetBundleOptions Options = BuildAssetBundleOptions.ChunkBasedCompression;

        const BuildTarget TARGET = BuildTarget.Android;

        [MenuItem("Assets/Build AssetBundle")]
        protected static void BuildAssetToAssetBundle()
        {
            string path = Application.dataPath.Remove(Application.dataPath.Length - 6) + "AssetBundle/Select/";

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
                string name = path.Remove(0, string.Format("{0}/", Application.dataPath).Length);

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
            string path = string.Format("{0}/AssetBundle/{1}/{2}", Application.dataPath.Remove(Application.dataPath.Length - 7), TARGET, GameConfig.Manifest);

            Utility.Document.CreateDirectory(path, true);

            BuildPipeline.BuildAssetBundles(path, Options, TARGET);
        }
    }
}