using System.Collections;
using System.Collections.Generic;
using UnityEditor.Window;
using UnityEngine;

namespace UnityEditor
{
    public class Beautify<T> where T : class, new()
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new T();
                }
                return _instance;
            }
        }
        public bool display = true;
    }
    public class BeautifyWindow : CustomWindow
    {
        [MenuItem("Window/Beautify")]
        protected static void Open()
        {
            Open<BeautifyWindow>("√¿ªØ");
        }

        protected override void Init()
        {
            
        }

        protected override void Refresh()
        {
            bool active = HierarchyBeautify.Instance.display;

            if (GUILayout.Button(string.Format("Hierarchy : {0}", active ? "On" : "Off")))
            {
                HierarchyBeautify.Instance.display = !active;
            }

            active = ProjectBeautify.Instance.display;

            if (GUILayout.Button(string.Format("Project : {0}", active ? "On" : "Off")))
            {
                ProjectBeautify.Instance.display = !active;
            }
        }
    }
}