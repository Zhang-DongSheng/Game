using UnityEngine;

namespace UnityEditor
{
    internal class HierarchyTools
    {
        [MenuItem("GameObject/Copy Select Path", true)]
        internal static bool CopySelectPathDerection()
        {
            return Selection.activeGameObject != null;
        }
        [MenuItem("GameObject/Copy Select Path", priority = 49)]
        internal static void CopySelectPath()
        {
            GameObject select = Selection.activeGameObject;

            string path = select.name;

            Transform parent = select.transform.parent;

            while (parent != null)
            {
                path = string.Format("{0}/{1}", parent.name, path);

                parent = parent.parent;
            }
            GUIUtility.systemCopyBuffer = path;
        }
    }
}