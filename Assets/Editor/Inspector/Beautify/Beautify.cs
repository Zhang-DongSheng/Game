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
                }
                return _instance;
            }
        }
        public bool display;
    }
    /// <summary>
    /// ����
    /// </summary>
    public class BeautifyWindow : CustomWindow
    {
        [MenuItem("Window/Beautify")]
        protected static void Open()
        {
            Open<BeautifyWindow>("����");
        }

        protected override void Initialise()
        {
            
        }

        protected override void Refresh()
        {
            bool active = BeautifyHierarchy.Instance.display;

            BeautifyHierarchy.Instance.display = GUILayout.Toggle(active, "Hierarchy", "Button");

            active = BeautifyProject.Instance.display;

            BeautifyProject.Instance.display = GUILayout.Toggle(active, "Project", "Button");
        }
    }
}