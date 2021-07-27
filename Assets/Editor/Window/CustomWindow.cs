using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UnityEditor.Window
{
    public abstract class CustomWindow : EditorWindow
    {
        protected Vector2 scroll;

        protected List<GUIStyle> style = new List<GUIStyle>();

        private readonly List<ItemFile> items = new List<ItemFile>();

        protected static void Open<T>(string title) where T : CustomWindow, new()
        {
            CustomWindow window = GetWindow<T>();
            window.minSize = new Vector2(128, 128);
            window.maxSize = new Vector2(2048, 2048);
            window.titleContent = new GUIContent(title);
            window.Init(); window.Show();
        }

        private void OnGUI()
        {
            Refresh();
        }

        protected abstract void Init();

        protected abstract void Refresh();

        protected void ShowNotification(string message)
        {
            ShowNotification(new GUIContent(message));
        }

        protected List<ItemFile> LoadFiles(string path, string pattern = "*.*")
        {
            DirectoryInfo directory = Directory.CreateDirectory(path);

            FileInfo[] _files = directory.GetFiles(pattern, SearchOption.AllDirectories);

            items.Clear();

            items.Add(new ItemFile()
            {
                name = "All",
            });

            foreach (var file in _files)
            {
                items.Add(new ItemFile()
                {
                    name = Path.GetFileNameWithoutExtension(file.Name),
                    path = file.FullName,
                    length = file.Length,
                });
            }
            return items;
        }

        protected float Width
        {
            get
            {
                return base.position.width;
            }
        }

        protected float Height
        {
            get
            {
                return base.position.height;
            }
        }

        public static string ToAssetPath(string path)
        {
            int length = Application.dataPath.Length;

            path = string.Format("Assets{0}", path.Remove(0, length));

            return path.Replace('\\', '/');
        }

        public static string ToAbsolutePath(string path)
        {
            if (path.StartsWith("Assets/"))
            {
                path = path.Remove(0, 6);
            }
            return string.Format("{0}{1}", Application.dataPath, path);
        }
    }
}