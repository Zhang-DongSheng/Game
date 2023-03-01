using System;
using System.Collections.Generic;

namespace UnityEngine
{
    /// <summary>
    /// ‘§÷∆ÃÂ÷˙ ÷
    /// </summary>
    [ExecuteInEditMode]
    public class PrefabHelper : MonoBehaviour
    {
        public Transform target;

        public List<GameObject> templates;

        public Component component;

        public string path;

        private void Awake()
        {
            if (target == null)
                target = transform;
        }

        protected void Execute()
        {
            if (component == null)
            {
                var parent = target.Find(path);

                if (parent == null) return;

                CreateTemplate(parent);
            }
            else
            {
                Type type = component.GetType();

                var components = target.GetComponentsInChildren(type);

                int count = components.Length;

                for (int i = 0; i < count; i++)
                {
                    var parent = components[i].transform.Find(path);

                    if (parent == null) continue;

                    CreateTemplate(parent);
                }
            }
        }

        protected void CreateTemplate(Transform parent)
        {
            while (parent.childCount > 0)
            {
                GameObject.DestroyImmediate(parent.GetChild(0).gameObject, true);
            }
            int count = templates.Count;

            for (int i = 0; i < count; i++)
            {
                GameObject go = GameObject.Instantiate(templates[i], parent);

                go.transform.localScale = Vector3.one;

                go.transform.localPosition = Vector3.zero;
            }
        }
    }
}