using Game;
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

                path = Utility._Path.UnityToSystem(path);

                path = path.Replace("\\", "/");

                GUIUtility.systemCopyBuffer = path;

                EditorWindow.focusedWindow.ShowNotification(new GUIContent("路径复制成功！"));
            }
        }
    }
}