using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UnityEditor
{
    public class ProjectBeautify
    {
        private static Dictionary<string, Information> assets = new Dictionary<string, Information>();

        private static string path = Application.dataPath.Substring(0, Application.dataPath.Length - 7);

        private static bool display = true;

        private static int count;

        [InitializeOnLoadMethod]
        protected static void Initialized()
        {
            EditorApplication.projectChanged += OnValidate;
            EditorApplication.projectWindowItemOnGUI += Refresh;
        }
        [Callbacks.DidReloadScripts]
        protected static void OnScriptsReloaded()
        {
            OnValidate();
        }

        protected static void OnValidate()
        {
            if (!display) return;

            assets.Clear();

            DirectoryInfo root = new DirectoryInfo(Application.dataPath);

            DirectoryInfo[] directories = root.GetDirectories("*", SearchOption.AllDirectories);

            count = directories.Length;

            for (int i = 0; i < count; i++)
            {
                Information information = new Information()
                {
                    size = 0,
                };
                var children = directories[i].GetFiles("*", SearchOption.AllDirectories);

                foreach (FileInfo info in children)
                {
                    information.size += info.Length;
                }
                assets.Add(directories[i].FullName.Replace("\\", "/"), information);
            }

            FileInfo[] files = root.GetFiles("*", SearchOption.AllDirectories);

            count = files.Length;

            for (int i = 0; i < count; i++)
            {
                if (files[i].Extension == ".meta")
                {
                    continue;
                }
                else
                {
                    Information information = new Information()
                    {
                        size = files[i].Length,
                    };
                    assets.Add(files[i].FullName.Replace("\\", "/"), information);
                }
            }
        }

        private static void Refresh(string guid, Rect rect)
        {
            if (!display) return;

            string key = string.Format("{0}/{1}", path, AssetDatabase.GUIDToAssetPath(guid));

            RefreshSize(key, rect);
        }

        private static void RefreshSize(string key, Rect rect)
        {
            if (rect.height > 16) return;   //>16为防止文件图标缩放时引起排版错乱

            if (assets.ContainsKey(key))
            {
                string text = assets[key].ToString();

                var label = EditorStyles.label;

                var content = new GUIContent(text);

                var width = label.CalcSize(content).x + 10;

                var pos = rect;

                pos.x = pos.xMax - width;

                pos.width = width;

                GUI.Label(pos, text);
            }
        }

        struct Information
        {
            public string name;

            public long size;

            public override string ToString()
            {
                return string.Format("{0}", size);
            }
        }
    }
}