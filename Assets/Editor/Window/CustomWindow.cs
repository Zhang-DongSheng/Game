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

        protected readonly Dictionary<string, bool> enables = new Dictionary<string, bool>();

        protected static void Open<T>(string title) where T : CustomWindow, new()
        {
            CustomWindow window = GetWindow<T>();
            window.minSize = new Vector2(128, 128);
            window.maxSize = new Vector2(2048, 2048);
            window.titleContent = new GUIContent(title);
            window.Initialise(); window.Show();
        }

        private void OnGUI()
        {
            Refresh();
        }

        protected abstract void Initialise();

        protected abstract void Refresh();

        protected void ShowNotification(string message)
        {
            ShowNotification(new GUIContent(message));
        }

        protected bool Enable(string key)
        {
            if (enables.ContainsKey(key))
            {
                return enables[key];
            }
            return true;
        }

        protected void Enable(string key, bool active)
        {
            if (enables.ContainsKey(key))
            {
                enables[key] = active;
            }
            else
            {
                enables.Add(key, active);
            }
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

        protected string ToLanguage(string key)
        {
            return EditorLanguage.GetWorld(key);
        }
    }
    [System.Serializable]
    public class Index
    {
        public UnityAction<int> action;

        private int _value = 0;
        public int value
        {
            get
            {
                return _value;
            }
            set
            {
                if (_value.Equals(value)) return;

                _value = value;

                Execute();
            }
        }

        public void Execute()
        {
            action?.Invoke(_value);
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
                if (_value.Equals(value)) return;

                _value = value;

                Execute();
            }
        }

        public void Execute()
        {
            action?.Invoke(_value);
        }
    }
}