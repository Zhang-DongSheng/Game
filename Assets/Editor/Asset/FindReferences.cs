using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace UnityEditor
{
    public class FindReferences
    {
        [MenuItem("Assets/Find References", false, 24)]
        protected static void Find()
        {
            EditorSettings.serializationMode = SerializationMode.ForceText;

            string path = AssetDatabase.GetAssetPath(Selection.activeObject);

            if (!string.IsNullOrEmpty(path))
            {
                string[] files = Directory.GetFiles(Application.dataPath, "*.*", SearchOption.AllDirectories);

                string guid = AssetDatabase.AssetPathToGUID(path);

                List<string> extensions = new List<string>()
                {
                    ".prefab",
                    ".unity",
                    ".mat",
                    ".asset"
                };

                List<string> assets = new List<string>();

                foreach (var file in files)
                {
                    if (extensions.Contains(Path.GetExtension(file).ToLower()))
                    {
                        assets.Add(file);
                    }
                }

                if (assets.Count == 0) return;

                int index = 0; bool contain = false;

                EditorApplication.update = delegate ()
                {
                    string file = assets[index];

                    bool cancel = EditorUtility.DisplayCancelableProgressBar("Finding ...", file, index / (float)assets.Count);

                    if (Regex.IsMatch(File.ReadAllText(file), guid))
                    {
                        contain = true;

                        Debug.Log(string.Format("<color=green>{0}</color>", file), AssetDatabase.LoadAssetAtPath<Object>(LocalToAssetPath(file)));
                    }

                    if (cancel || ++index >= assets.Count)
                    {
                        EditorUtility.ClearProgressBar();
                        EditorApplication.update = null;

                        if (contain)
                        {
                            Debug.Log("搜索完成！");
                        }
                        else
                        {
                            Debug.Log("未发现任何引用！");
                        }
                    }
                };
            }
        }

        protected static string LocalToAssetPath(string path)
        {
            return "Assets/" + path.Replace(Application.dataPath, string.Empty).Replace('\\', '/');
        }
    }
}