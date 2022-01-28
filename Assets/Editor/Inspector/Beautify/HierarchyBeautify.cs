using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityEditor
{
    public class HierarchyBeautify
    {
        private static readonly List<Type> ignore = new List<Type>()
        {
            typeof(Transform),
            typeof(ParticleSystemRenderer),
            typeof(CanvasRenderer),
        };

        private static readonly Vector2 cell = new Vector2(16, 1);

        private static bool display = true;

        [InitializeOnLoadMethod]
        protected static void Initialized()
        {
            EditorApplication.hierarchyChanged += OnValidate;
            EditorApplication.hierarchyWindowItemOnGUI += Refresh;
        }
        [Callbacks.DidReloadScripts]
        protected static void OnScriptsReloaded()
        {
            OnValidate();
        }

        protected static void OnValidate()
        {

        }

        private static void Refresh(int instanceID, Rect rect)
        {
            if (!display) return;

            Object current = EditorUtility.InstanceIDToObject(instanceID);

            if (current == null) return;

            RefreshActive(current as GameObject, rect);

            RefreshIcon(current as GameObject, rect);
        }

        private static void RefreshIcon(GameObject go, Rect rect)
        {
            rect.width += rect.x;

            rect.x = 0;

            Component[] components = go.GetComponents<Component>();

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
                        GUI.DrawTexture(new Rect(rect.width - (cell.x + 1) * (i + 1), rect.y + cell.y, cell.x, cell.x), texture);
                    }
                }
            }
        }

        private static void RefreshActive(GameObject go, Rect rect)
        {
            bool active = go.activeSelf;

            rect.width = 16f;

            active = GUI.Toggle(rect, active, string.Empty);

            if (go.activeSelf != active)
            {
                go.SetActive(active);
            }
        }
    }
}