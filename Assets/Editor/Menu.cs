using Game;
using System.IO;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityEditor
{
    public class Menu
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
        [MenuItem("Tools/Folder/Project")]
        protected static void OpenProjectFolder()
        {
            OpenFolder(Application.dataPath.Substring(0, Application.dataPath.Length - 7));
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
        [MenuItem("Assets/Copy Path Pro", priority = 19)]
        internal static void CopyPath()
        {
            if (Selection.activeObject != null)
            {
                string path = AssetDatabase.GetAssetPath(Selection.activeObject);

                path = Utility.Path.UnityToSystem(path);

                path = path.Replace("\\", "/");

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
        [MenuItem("Assets/Open HotFix Project", priority = 900)]
        internal static void OpenHotFixProject()
        {
            string folder = "ILRuntime/Hotfix~";

            string file = "Hotfix";

            string path = string.Format("{0}/{1}/{2}.sln", Application.dataPath, folder, file);

            EditorUtility.OpenWithDefaultApp(path);
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