using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace UnityEngine
{
    /// <summary>
    /// ‘§÷∆ÃÂ÷˙ ÷
    /// </summary>
    [ExecuteInEditMode]
    public class PrefabEditor : MonoBehaviour
    {
#if UNITY_EDITOR
        public Transform target;

        public MonoScript component;

        public string path;

        public int order = -1;

        public bool destroy;

        public List<GameObject> templates;

        private void Awake()
        {
            if (target == null)
                target = transform;
        }
        [ContextMenu("Execute")]
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
                Type type = component.GetClass();

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
            if (destroy)
            {
                while (parent.childCount > 0)
                {
                    GameObject.DestroyImmediate(parent.GetChild(0).gameObject, true);
                }
            }
            int count = templates.Count;

            for (int i = 0; i < count; i++)
            {
                GameObject go = GameObject.Instantiate(templates[i], parent);

                go.transform.localScale = Vector3.one;

                go.transform.localPosition = Vector3.zero;

                go.name = templates[i].name;

                if (order > -1)
                {
                    go.transform.SetSiblingIndex(order);
                }
                else
                {
                    go.transform.SetAsLastSibling();
                }
            }
        }
#endif
    }
}