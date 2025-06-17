 using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace UnityEditor
{
    public class FindReferences
    {
        private static readonly Dictionary<string, int> references = new Dictionary<string, int>();

        private static readonly List<string> extensions = new List<string>()
        {
            ".prefab",
            ".unity",
            ".mat",
            ".asset"
        };

        private static readonly List<string> assets = new List<string>();

        [MenuItem("Assets/Find References", false, 24)]
        protected static void Find()
        {
            EditorSettings.serializationMode = SerializationMode.ForceText;

            string path = AssetDatabase.GetAssetPath(Selection.activeObject);

            if (!string.IsNullOrEmpty(path))
            {
                string guid = AssetDatabase.AssetPathToGUID(path);

                if (!Load()) return;

                Match(guid, assets, true, (key) =>
                {
                    if (references[key] > 0)
                    {
                        Debuger.Log(Author.Editor, "Completed!");
                    }
                    else
                    {
                        Debuger.Log(Author.Editor, $"[{key}] Not Found References!");
                    }
                });
            }
        }

        public static void Empty(string filter, params string[] folders)
        {
            if (folders == null || folders.Length == 0) return;

            EditorSettings.serializationMode = SerializationMode.ForceText;

            string[] guids = AssetDatabase.FindAssets(filter, folders);

            if (guids.Length == 0) return;

            if (!Load()) return;

            references.Clear();

            Match(guids, 0, assets, () =>
            {
                bool empty = false;

                foreach (var item in references)
                {
                    if (item.Value == 0)
                    {
                        Debuger.Log(Author.Editor, $"[{item.Key}] Not Found References!");
                        empty = true;
                    }
                }

                if (!empty)
                {
                    Debuger.Log(Author.Editor, "Completed!");
                }
            });
        }

        protected static bool Load()
        {
            string[] files = Directory.GetFiles(Application.dataPath, "*.*", SearchOption.AllDirectories);

            assets.Clear();

            foreach (var file in files)
            {
                if (extensions.Contains(Path.GetExtension(file).ToLower()))
                {
                    assets.Add(file);
                }
            }
            return assets.Count > 0;
        }

        protected static void Match(string[] guids, int index, List<string> assets, System.Action complete = null)
        {
            if (guids.Length > index)
            {
                Match(guids[index], assets, false, (_) =>
                {
                    Match(guids, ++index, assets, complete);
                });
            }
            else
            {
                complete?.Invoke();
            }
        }

        protected static void Match(string guid, List<string> assets, bool debug = false, System.Action<string> complete = null)
        {
            string key = AssetDatabase.GUIDToAssetPath(guid);

            if (!references.ContainsKey(key)) references.Add(key, 0);

            int index = 0;

            EditorApplication.update = delegate ()
            {
                string file = assets[index];

                bool cancel = EditorUtility.DisplayCancelableProgressBar("Finding ...", file, index / (float)assets.Count);

                if (Regex.IsMatch(File.ReadAllText(file), guid))
                {
                    if (debug)
                    {
                        Debuger.Log(Author.Editor, file, AssetDatabase.LoadAssetAtPath<Object>(ToAssetPath(file)));
                    }
                    references[key]++;
                }

                if (cancel || ++index >= assets.Count)
                {
                    EditorUtility.ClearProgressBar();

                    EditorApplication.update = null;

                    complete?.Invoke(key);
                }
            };
        }

        protected static string ToAssetPath(string path)
        {
            int length = Application.dataPath.Length;

            path = string.Format("Assets/{0}", path.Remove(0, length));

            return path.Replace('\\', '/');
        }
    }
}