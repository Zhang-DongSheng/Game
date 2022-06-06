using Game;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UnityEditor
{
    public class BeautifyProject : Beautify<BeautifyProject>
    {
        private readonly Dictionary<string, Information> items = new Dictionary<string, Information>();

        private int count;

        [InitializeOnLoadMethod]
        protected static void Initialized()
        {
            EditorApplication.projectChanged += Instance.OnValidate;
            EditorApplication.projectWindowItemOnGUI += Instance.Refresh;
        }
        [Callbacks.DidReloadScripts]
        protected static void OnScriptsReloaded()
        {
            Instance.OnValidate();
        }

        protected void OnValidate()
        {
            items.Clear();

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
                items.Add(directories[i].FullName.Replace("\\", "/"), information);
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
                    items.Add(files[i].FullName.Replace("\\", "/"), information);
                }
            }
        }

        private void Refresh(string guid, Rect rect)
        {
            if (!display) return;

            string key = string.Format("{0}/{1}", Utility._Path.Project, AssetDatabase.GUIDToAssetPath(guid));

            RefreshSize(key, rect);
        }

        private void RefreshSize(string key, Rect rect)
        {
            if (rect.height > 16) return;   //>16为防止文件图标缩放时引起排版错乱

            if (items.ContainsKey(key))
            {
                string text = items[key].ToString();

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
                return string.Format("{0}", size.ToStringSize());
            }
        }
    }
}