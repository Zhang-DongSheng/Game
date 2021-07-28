using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEditor.Window
{
    public abstract class CustomWindow : EditorWindow
    {
        protected readonly Index index = new Index();

        protected readonly Input input = new Input();

        protected Vector2 scroll;

        protected List<GUIStyle> style = new List<GUIStyle>();

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

        protected virtual void Horizontal(UnityAction action)
        {
            GUILayout.BeginHorizontal();
            {
                action?.Invoke();
            }
            GUILayout.EndHorizontal();
        }

        protected virtual void Vertical(UnityAction action)
        {
            GUILayout.BeginVertical();
            {
                action?.Invoke();
            }
            GUILayout.EndVertical();
        }

        protected virtual void Scroll(UnityAction action)
        {
            scroll = GUILayout.BeginScrollView(scroll);
            {
                action?.Invoke();
            }
            GUILayout.EndScrollView();
        }

        protected string ToAssetPath(string path)
        {
            int length = Application.dataPath.Length;

            path = string.Format("Assets{0}", path.Remove(0, length));

            return path.Replace('\\', '/');
        }

        protected string ToAbsolutePath(string path)
        {
            if (path.StartsWith("Assets/"))
            {
                path = path.Remove(0, 6);
            }
            return string.Format("{0}{1}", Application.dataPath, path);
        }

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
    [System.Serializable]
    public class Index
    {
        public UnityAction<int> action;

        private int _index = 0;
        public int index
        {
            get
            {
                return _index;
            }
            set
            {
                if (_index != value)
                {
                    _index = value;

                    action?.Invoke(_index);
                }
            }
        }
    }
    [System.Serializable]
    public class Input
    {
        public UnityAction<string> action;

        private string _value = string.Empty;
        public string value
        {
            get
            {
                return _value;
            }
            set
            {
                if (_value != value)
                {
                    _value = value;

                    action?.Invoke(_value);
                }
            }
        }
    }
}