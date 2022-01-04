using System;
using System.IO;
using UnityEngine;

namespace UnityEditor
{
    public class AssetBundleRename
    {
        public static void SetAssetBundles(string path, Func<string, string> rename)
        {
            DirectoryInfo dir = new DirectoryInfo(path);

            FileSystemInfo[] files = dir.GetFileSystemInfos();

            for (int i = 0; i < files.Length; i++)
            {
                if (files[i] is DirectoryInfo)
                {
                    SetAssetBundles(files[i].FullName, rename);
                }
                else if (!files[i].Name.EndsWith(".meta"))
                {
                    SetAssetBundle(files[i].FullName, rename);
                }
            }
        }

        public static void SetAssetBundle(string path, Func<string, string> rename)
        {
            string importerPath = "Assets" + path.Substring(Application.dataPath.Length);

            AssetImporter assetImporter = AssetImporter.GetAtPath(importerPath);

            string bundle;

            if (rename != null)
            {
                bundle = rename.Invoke(path);
            }
            else
            {
                bundle = Path.GetFileNameWithoutExtension(path);
            }
            assetImporter.assetBundleName = bundle;
        }
    }
}