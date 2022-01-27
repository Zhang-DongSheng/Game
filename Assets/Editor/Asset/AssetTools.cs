using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
    public class AssetTools
    {
        [MenuItem("Assets/CopyPath", priority = 1)]
        protected static void CopyPath()
        {
            if (Selection.activeObject != null)
            {
                string path = AssetDatabase.GetAssetPath(Selection.activeObject);

                path = string.Format("{0}{1}", Application.dataPath, path.Remove(0, 6));

                path = path.Replace("\\", "/");

                GUIUtility.systemCopyBuffer = path;

                EditorWindow.focusedWindow.ShowNotification(new GUIContent("路径复制成功！"));
            }
        }
    }
}