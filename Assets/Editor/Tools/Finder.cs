using System.Collections.Generic;
using System.IO;

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
                            path = dir,
                            asset = CustomWindow.ToAssetPath(dir),
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
                            order = order,
                            path = file.FullName,
                            asset = CustomWindow.ToAssetPath(file.FullName),
                        });
                    }
                }
            }
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

        public long length;
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