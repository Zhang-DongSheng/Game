using System.Collections.Generic;

namespace UnityEditor.Window
{
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