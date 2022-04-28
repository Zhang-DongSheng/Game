using Game;
using System;
using System.IO;

namespace UnityEditor
{
    public static class AssetBundleRename
    {
        public static void SetAssetBundles(string folder, Func<string, string> rename)
        {
            DirectoryInfo dir = new DirectoryInfo(folder);

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

        public static void SetAssetBundle(string file, Func<string, string> rename)
        {
            string importerPath = Utility.Path.SystemToUnity(file);

            AssetImporter importer = AssetImporter.GetAtPath(importerPath);

            string bundle;

            if (rename != null)
            {
                bundle = rename.Invoke(file);
            }
            else
            {
                bundle = Path.GetFileNameWithoutExtension(file);
            }
            importer.assetBundleName = bundle;
        }

        public static void Clear()
        {
            string[] names = AssetDatabase.GetAllAssetBundleNames();

            int count = names.Length;

            for (int i = 0; i < count; i++)
            {
                AssetDatabase.RemoveAssetBundleName(names[i], true);
            }
            EditorUtility.ClearProgressBar();
        }
    }
}