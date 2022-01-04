using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UnityEditor
{
    public class AssetBundleHelper
    {
        [MenuItem("Assets/Build AssetBundle")]
        protected static void BuildAssetToAssetBundle()
        {
            string folder = Application.dataPath.Remove(Application.dataPath.Length - 6) + "AssetBundle/Select/";

            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

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
                BuildPipeline.BuildAssetBundles(folder, builds.ToArray(), BuildAssetBundleOptions.None, BuildTarget.Android);
            }
        }
    }
}