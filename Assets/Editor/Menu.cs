using Game;
using System.IO;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityEditor
{
    public static class Menu
    {
        [MenuItem("Game/OpenMainScene", priority = 101)]
        private static void OpenMainScene()
        {
            string[] scenes = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes);

            string main = scenes.Length > 0 ? scenes[0] : string.Empty;

            Scene current = EditorSceneManager.GetActiveScene();

            if (!string.IsNullOrEmpty(main) && current.path != main)
            {
                EditorSceneManager.OpenScene(main);
            }
            EditorApplication.isPlaying = true;
        }
        [MenuItem("Tools/Screenshot &X")]
        private static void Screenshot()
        {
            string path = Application.dataPath + "/Screenshot.png";
            path = Utility.Path.NewFile(path);
            ScreenCapture.CaptureScreenshot(path);
            AssetDatabase.Refresh();
        }
        [MenuItem("Tools/Folder/Project")]
        private static void OpenProjectFolder()
        {
            OpenFolder(Application.dataPath.Substring(0, Application.dataPath.Length - 7));
        }
        [MenuItem("Tools/Folder/Data")]
        private static void OpenDataFolder()
        {
            OpenFolder(Application.dataPath);
        }
        [MenuItem("Tools/Folder/StreamingAssets")]
        private static void OpenStreamingAssetsFolder()
        {
            OpenFolder(Application.streamingAssetsPath);
        }
        [MenuItem("Tools/Folder/PersistentData")]
        private static void OpenPersistentDataFolder()
        {
            OpenFolder(Application.persistentDataPath);
        }
        [MenuItem("Tools/Folder/TemporaryCache")]
        private static void OpenTemporaryCacheFolder()
        {
            OpenFolder(Application.temporaryCachePath);
        }
        [MenuItem("Tools/File/Log")]
        private static void OpenConsoleFile()
        {
            OpenFile(Application.consoleLogPath);
        }
        [MenuItem("Assets/Open HotFix Project", priority = 999)]
        private static void OpenHotFixProject()
        {
            string folder = "ILRuntime/Hotfix~";

            string file = "Hotfix";

            string path = string.Format("{0}/{1}/{2}.sln", Application.dataPath, folder, file);

            EditorUtility.OpenWithDefaultApp(path);
        }
        [MenuItem("Assets/Copy Path Pro/System Path")]
        private static void CopyFullPath()
        {
            if (Selection.activeObject != null)
            {
                string path = AssetDatabase.GetAssetPath(Selection.activeObject);

                path = Utility.Path.UnityToSystem(path);

                GUIUtility.systemCopyBuffer = path;

                if (EditorWindow.focusedWindow != null)
                {
                    EditorWindow.focusedWindow.ShowNotification(new GUIContent("路径复制成功！"));
                }
                else
                {
                    Debuger.Log(Author.File, "路径复制成功！");
                }
            }
        }
        [MenuItem("Assets/Copy Path Pro/Asset Path")]
        private static void CopyAssetPath()
        {
            if (Selection.activeObject != null)
            {
                string path = AssetDatabase.GetAssetPath(Selection.activeObject);

                GUIUtility.systemCopyBuffer = path;

                if (EditorWindow.focusedWindow != null)
                {
                    EditorWindow.focusedWindow.ShowNotification(new GUIContent("路径复制成功！"));
                }
                else
                {
                    Debuger.Log(Author.File, "路径复制成功！");
                }
            }
        }
        [MenuItem("Assets/Copy Path Pro/Package Path")]
        private static void CopyPackagePath()
        {
            if (Selection.activeObject != null)
            {
                string path = AssetDatabase.GetAssetPath(Selection.activeObject);

                path = path[7..];

                GUIUtility.systemCopyBuffer = path;

                if (EditorWindow.focusedWindow != null)
                {
                    EditorWindow.focusedWindow.ShowNotification(new GUIContent("路径复制成功！"));
                }
                else
                {
                    Debuger.Log(Author.File, "路径复制成功！");
                }
            }
        }

        private static void OpenFolder(string path)
        {
            if (string.IsNullOrEmpty(path)) return;

            if (Directory.Exists(path))
            {
                EditorUtility.RevealInFinder(path);
            }
        }

        private static void OpenFile(string path)
        {
            if (string.IsNullOrEmpty(path)) return;

            if (File.Exists(path))
            {
                EditorUtility.OpenWithDefaultApp(path);
            }
        }
    }
}