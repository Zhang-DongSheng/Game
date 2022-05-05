using System.Collections;
using System.Collections.Generic;
using UnityEditor.Window;
using UnityEngine;

namespace UnityEditor
{
    public abstract class Beautify<T> where T : class, new()
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new T();
                    //{
                    //    display = UnityEngine.PlayerPrefs.GetInt("Beautify_" + typeof(T).ToString());
                    //}
                }
                return _instance;
            }
        }
        public bool display;
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
            bool active = BeautifyHierarchy.Instance.display;

            if (GUILayout.Button(string.Format("Hierarchy : {0}", active ? "On" : "Off")))
            {
                BeautifyHierarchy.Instance.display = !active;
            }

            active = BeautifyProject.Instance.display;

            if (GUILayout.Button(string.Format("Project : {0}", active ? "On" : "Off")))
            {
                BeautifyProject.Instance.display = !active;
            }
        }
    }
}