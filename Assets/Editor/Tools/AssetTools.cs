using Game;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
    internal class AssetTools
    {
        [MenuItem("Assets/Copy Path Pro", priority = 19)]
        internal static void CopyPath()
        {
            if (Selection.activeObject != null)
            {
                string path = AssetDatabase.GetAssetPath(Selection.activeObject);

                path = Utility.Path.UnityToSystem(path);

                path = path.Replace("\\", "/");

                GUIUtility.systemCopyBuffer = path;

                if (EditorWindow.focusedWindow != null)
                {
                    EditorWindow.focusedWindow.ShowNotification(new GUIContent("路径复制成功！"));
                }
                else
                {
                    Debuger.Log(Author.File, "路径复制成功！");
                }
            }
        }
        [MenuItem("Assets/Open HotFix Project", priority = 101)]
        internal static void OpenHotFixProject()
        {
            string folder = "ILRuntime/Hotfix~";

            string file = "Hotfix";

            string path = string.Format("{0}/{1}/{2}.sln", Application.dataPath, folder, file);

            EditorUtility.OpenWithDefaultApp(path);
        }

        public static List<T> FindAssetsByType<T>() where T : Object
        {
            List<T> assets = new List<T>();

            var guids = AssetDatabase.FindAssets($"t:{typeof(T)}");

            for (int i = 0, count = guids.Length; i < count; ++i)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);

                T asset = AssetDatabase.LoadAssetAtPath<T>(path);

                if (asset != null)
                {
                    assets.Add(asset);
                }
            }
            return assets;
        }
    }
}