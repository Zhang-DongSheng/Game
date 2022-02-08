using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
    public class HierarchyBeautify : Beautify<HierarchyBeautify>
    {
        private readonly List<Type> ignore = new List<Type>()
        {
            typeof(Transform),
            typeof(ParticleSystemRenderer),
            typeof(CanvasRenderer),
        };
        private readonly Vector2 cell = new Vector2(16, 1);

        private readonly Dictionary<int, Information> items = new Dictionary<int, Information>();

        [InitializeOnLoadMethod]
        protected static void Initialized()
        {
            EditorApplication.hierarchyChanged += Instance.OnValidate;
            EditorApplication.hierarchyWindowItemOnGUI += Instance.Refresh;
        }
        [Callbacks.DidReloadScripts]
        protected static void OnScriptsReloaded()
        {
            Instance.OnValidate();
        }

        protected void OnValidate()
        {
            items.Clear();
        }

        private void Refresh(int instanceID, Rect rect)
        {
            if (!display) return;

            if (items.ContainsKey(instanceID) && items[instanceID].go != null)
            {
                RefreshActive(items[instanceID], rect);

                RefreshIcon(items[instanceID], rect);
            }
            else
            {
                GameObject target = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

                if (target == null) return;

                items.Add(instanceID, new Information()
                {
                    go = target,
                    components = new List<Texture2D>(),
                });
                Component[] components = items[instanceID].go.GetComponents<Component>();

                int count = components.Length;

                for (int i = 0; i < count; i++)
                {
                    if (components[i] == null)
                    {
                        continue;
                    }
                    else if (ignore.Exists(x => x.Equals(components[i].GetType()) || x.IsSubclassOf(components[i].GetType())))
                    {
                        continue;
                    }
                    else
                    {
                        if (AssetPreview.GetMiniThumbnail(components[i]) is Texture2D texture)
                        {
                            items[instanceID].components.Add(texture);
                        }
                    }
                }
            }
        }

        private void RefreshIcon(Information information, Rect rect)
        {
            rect.width += rect.x;

            rect.x = 0;

            int count = information.components.Count;

            for (int i = 0; i < count; i++)
            {
                GUI.DrawTexture(new Rect(rect.width - (cell.x + 1) * (i + 1), rect.y + cell.y, cell.x, cell.x), information.components[i]);
            }
        }

        private void RefreshActive(Information information, Rect rect)
        {
            rect.width = 16f;

            information.active = information.go.activeSelf;

            information.active = GUI.Toggle(rect, information.active, string.Empty);

            if (information.go.activeSelf != information.active)
            {
                information.go.SetActive(information.active);
            }
        }

        struct Information
        {
            public GameObject go;

            public List<Texture2D> components;

            public bool active;
        }
    }
}