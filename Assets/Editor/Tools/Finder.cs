using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UnityEditor.Window
{
    public static class Finder
    {
        public static List<ItemBase> Find(string path)
        {
            List<ItemBase> items = new List<ItemBase>();

            Find(path, 0, ref items);

            return items;
        }

        private static void Find(string path, int order, ref List<ItemBase> nodes)
        {
            if (Directory.Exists(path))
            {
                string[] dirs = Directory.GetDirectories(path);

                if (dirs.Length > 0)
                {
                    foreach (string dir in dirs)
                    {
                        ItemFolder folder = new ItemFolder()
                        {
                            name = Path.GetFileName(dir),
                            asset = ToAssetPath(dir),
                            path = dir,
                            order = order,
                        };
                        Find(dir, folder.order + 1, ref folder.items); nodes.Add(folder);
                    }
                }

                DirectoryInfo root = new DirectoryInfo(path);

                foreach (FileInfo file in root.GetFiles())
                {
                    if (file.Extension != ".meta")
                    {
                        nodes.Add(new ItemFile()
                        {
                            name = Path.GetFileNameWithoutExtension(file.Name),
                            asset = ToAssetPath(file.FullName),
                            path = file.FullName,
                            size = file.Length,
                            order = order,
                        });
                    }
                }
            }
        }

        public static List<ItemFile> LoadFiles(string path, string pattern = "*.*")
        {
            List<ItemFile> items = new List<ItemFile>();

            DirectoryInfo directory = Directory.CreateDirectory(path);

            FileInfo[] _files = directory.GetFiles(pattern, SearchOption.AllDirectories);

            items.Add(new ItemFile()
            {
                name = "All",
            });

            foreach (var file in _files)
            {
                items.Add(new ItemFile()
                {
                    name = Path.GetFileNameWithoutExtension(file.Name),
                    asset = ToAssetPath(file.FullName),
                    path = file.FullName,
                    size = file.Length,
                    order = 0,
                });
            }
            return items;
        }

        public static string ToAssetPath(string path)
        {
            int length = Application.dataPath.Length;

            path = string.Format("Assets{0}", path.Remove(0, length));

            return path.Replace('\\', '/');
        }
    }

    public abstract class ItemBase
    {
        public abstract ItemType type { get; }

        public string name;

        public string asset;

        public string path;

        public int order;

        public bool select;
    }

    public class ItemFile : ItemBase
    {
        public override ItemType type { get { return ItemType.File; } }

        public string folder;

        public long size;
    }

    public class ItemFolder : ItemBase
    {
        public override ItemType type { get { return ItemType.Folder; } }

        public List<ItemBase> items = new List<ItemBase>();
    }

    public enum ItemType
    {
        Folder,
        File,
    }
}