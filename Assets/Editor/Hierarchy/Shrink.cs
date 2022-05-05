using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityEditor.Hierarchy
{
    class Shrink
    {
        [MenuItem("GameObject/Shrink/Retract", priority = 49)]
        protected static void Retract()
        {
            SetExpandedRecursive(false);
        }
        [MenuItem("GameObject/Shrink/Spread", priority = 49)]
        protected static void Spread()
        {
            SetExpandedRecursive(true);
        }

        protected static void SetExpandedRecursive(bool expand)
        {
            EditorApplication.ExecuteMenuItem("Window/General/Hierarchy");

            EditorWindow window = EditorWindow.focusedWindow;

            var method = window.GetType().GetMethod("SetExpandedRecursive");

            if (Selection.activeGameObject != null)
            {
                method.Invoke(window, new object[] { Selection.activeGameObject.GetInstanceID(), expand });
            }
            else
            {
                foreach (GameObject root in SceneManager.GetActiveScene().GetRootGameObjects())
                {
                    method.Invoke(window, new object[] { root.GetInstanceID(), expand });
                }
            }
        }
    }
}