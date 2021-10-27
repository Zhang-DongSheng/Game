using System.IO;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityEditor
{
    public class Helper
    {
        private static int pictureIndex;

        [MenuItem("Tools/OpenMainScene &Q")]
        protected static void OpenMainScene()
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
        protected static void Screenshot()
        {
            string pic_name = Application.dataPath + string.Format("/Pic_{0}.png", ++pictureIndex);
            ScreenCapture.CaptureScreenshot(pic_name);
            AssetDatabase.Refresh();
        }
        [MenuItem("Tools/Folder/Data")]
        protected static void OpenDataFolder()
        {
            OpenFolder(Application.dataPath);
        }
        [MenuItem("Tools/Folder/StreamingAssets")]
        protected static void OpenStreamingAssetsFolder()
        {
            OpenFolder(Application.streamingAssetsPath);
        }
        [MenuItem("Tools/Folder/PersistentData")]
        protected static void OpenPersistentDataFolder()
        {
            OpenFolder(Application.persistentDataPath);
        }
        [MenuItem("Tools/Folder/TemporaryCache")]
        protected static void OpenTemporaryCacheFolder()
        {
            OpenFolder(Application.temporaryCachePath);
        }
        [MenuItem("Tools/File/Log")]
        protected static void OpenConsoleFile()
        {
            OpenFile(Application.consoleLogPath);
        }
        [MenuItem("GameObject/Shrink/Retract", priority = 49)]
        protected static void Retract()
        {
            Shrink(false);
        }
        [MenuItem("GameObject/Shrink/Spread", priority = 49)]
        protected static void Spread()
        {
            Shrink(true);
        }

        protected static void Shrink(bool expand)
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

        protected static void OpenFolder(string path)
        {
            if (string.IsNullOrEmpty(path)) return;

            if (Directory.Exists(path))
            {
                EditorUtility.RevealInFinder(path);
            }
            else
            {
                Debug.LogError("No Directory: " + path);
            }
        }

        protected static void OpenFile(string path)
        {
            if (string.IsNullOrEmpty(path)) return;

            if (File.Exists(path))
            {
                EditorUtility.OpenWithDefaultApp(path);
            }
            else
            {
                Debug.LogError("No Find: " + path);
            }
        }
    }
}