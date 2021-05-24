using UnityEngine;

namespace UnityEditor
{
    public abstract class CustomWindow : EditorWindow
    {
        protected abstract string Title { get; }

        protected static void Open<T>() where T : CustomWindow, new()
        {
            CustomWindow window = GetWindow<T>();
            window.minSize = new Vector2(128, 128);
            window.maxSize = new Vector2(2048, 2048);
            window.titleContent = new GUIContent(window.Title);
            window.Init(); window.Show();
        }

        private void Awake()
        {

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
    }

    public class ItemBase
    {
        public string name;

        public string path;

        public bool select;
    }

    public class ItemFile : ItemBase
    {
        public string root;

        public long length;
    }

    public class ItemFolder : ItemBase
    {

    }
}