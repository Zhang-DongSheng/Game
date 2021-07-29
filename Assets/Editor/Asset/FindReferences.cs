using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace UnityEditor
{
    public class FindReferences
    {
        private static readonly Dictionary<string, int> references = new Dictionary<string, int>();

        private static readonly List<string> assets = new List<string>();

        private static readonly List<string> extensions = new List<string>()
        {
            ".prefab",
            ".unity",
            ".mat",
            ".asset"
        };

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
                        Debug.Log("<color=blue>Completed!</color>");
                    }
                    else
                    {
                        Debug.LogFormat("<color=red>[{0}]</color> Not Found References!", key);
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
                        Debug.LogFormat("<color=red>[{0}]</color> Not Found References!", item.Key);
                        empty = true;
                    }
                }

                if (!empty)
                {
                    Debug.Log("<color=blue>Completed!</color>");
                }
            });
        }

        public static bool Missing(GameObject go)
        {
            bool missing = false;

            Component[] components = go.GetComponentsInChildren<Component>(true);

            foreach (var component in components)
            {
                if (component != null)
                {
                    SerializedObject so = new SerializedObject(component);

                    SerializedProperty sp = so.GetIterator();

                    while (sp.NextVisible(true))
                    {
                        if (sp.propertyType == SerializedPropertyType.ObjectReference &&
                            sp.objectReferenceInstanceIDValue != 0 &&
                            sp.objectReferenceValue == null)
                        {
                            Debug.LogErrorFormat("{0} Missing : {1}", go.name, sp.propertyPath);
                        }
                    }
                }
                else
                {
                    missing = true;
                }
            }
            return missing;
        }

        public static void Overflow(string filter, params string[] folders)
        {
            if (folders == null || folders.Length == 0) return;

            string[] guids = AssetDatabase.FindAssets(filter, folders);

            if (guids.Length == 0) return;

            string path; Object asset;

            foreach (var guid in guids)
            {
                path = AssetDatabase.GUIDToAssetPath(guid);

                asset = AssetDatabase.LoadAssetAtPath<Object>(path);

                if (asset == null)
                {
                    continue;
                }
                else if (asset is Texture2D texture)
                {
                    OverflowTexture(path, texture);
                }
                else if (asset is Material material)
                {
                    OverflowMaterial(path, material);
                }
                else if (asset is TextAsset text)
                {
                    OverflowTextAsset(path, text);
                }
            }
        }

        protected static void OverflowTexture(string name, Texture2D texture)
        {
            if (texture.width > 2048 || texture.height > 2048)
            {
                Debug.LogError(string.Format("{0} ³ß´ç³¬³ö2048£¡", name), texture);
            }
            else if (texture.width > 1024 || texture.height > 1024)
            {
                Debug.LogWarning(string.Format("{0} ³ß´ç´óÓÚ1024£¡", name), texture);
            }
            else if (texture.format != TextureFormat.ASTC_4x4)
            {
#if UNITY_ANDROID
                if (texture.format == TextureFormat.ETC2_RGB ||
                   texture.format == TextureFormat.ETC2_RGBA8Crunched)
#elif UNITY_IOS
                if(texture.format == TextureFormat.PVRTC_RGB4 ||
                   texture.format == TextureFormat.PVRTC_RGBA4)
#endif
                { }
                else
                {
                    Debug.LogError(string.Format("{0} Î´Ñ¹Ëõ£¡", name), texture);
                }
            }
        }

        protected static void OverflowMaterial(string name, Material material)
        {
            if (material.passCount > 2)
            {
                Debug.LogError(string.Format("{0} äÖÈ¾´ÎÊý¹ý¶à£¡", name), material);
            }
        }

        protected static void OverflowTextAsset(string name, TextAsset textAsset)
        {
            if (textAsset.bytes.Length > 1024 * 1024 * 1)
            {
                Debug.LogError(string.Format("{0} ´úÂë³¬³¤£¡", name), textAsset);
            }
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
                        Debug.Log(string.Format("<color=green>{0}</color>", file), AssetDatabase.LoadAssetAtPath<Object>(ToAssetPath(file)));
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

        protected static string ToAssetPath(string path)
        {
            int length = Application.dataPath.Length;

            path = string.Format("Assets/{0}", path.Remove(0, length));

            return path.Replace('\\', '/');
        }
    }
}