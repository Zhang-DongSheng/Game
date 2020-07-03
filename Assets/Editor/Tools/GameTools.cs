using System.IO;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityEditor
{
    public class GameTools
    {
        private static readonly string sceneName = "Main";

        private static int pictureIndex;

        [MenuItem("Tools/OpenMainScene &Q")]
        private static void OpenMainScene()
        {
            string path_scene = sceneName;
            Scene scene_cur = EditorSceneManager.GetActiveScene();
            if (scene_cur.name != path_scene)
            {
                path_scene = string.Format("Assets/Scene/{0}.unity", path_scene);
                EditorSceneManager.OpenScene(path_scene);
            }
            EditorApplication.isPlaying = true;
        }

        [MenuItem("Tools/Screenshot &X")]
        private static void Screenshot()
        {
            string pic_name = Application.dataPath + string.Format("/Pic_{0}.png", ++pictureIndex);
            ScreenCapture.CaptureScreenshot(pic_name);
            AssetDatabase.Refresh();
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

        [MenuItem("Tools/File/Console")]
        private static void OpenConsoleFile()
        {
            OpenFile(Application.consoleLogPath);
        }

        private static void OpenFolder(string path)
        {
            if (string.IsNullOrEmpty(path)) return;

            if (Directory.Exists(path))
            {
                path = path.Replace("/", "\\");

                System.Diagnostics.Process.Start("explorer.exe", path);
            }
            else
            {
                Debug.LogError("No Directory: " + path);
            }
        }

        private static void OpenFile(string path)
        {
            if (string.IsNullOrEmpty(path)) return;

            if (File.Exists(path))
            {
                string[] fileInfo = path.Split('.');

                string suffix = fileInfo[fileInfo.Length - 1];

                string process;

                switch (suffix)
                {
                    case "txt":
                    case "log":
                        process = "notepad.exe";
                        break;
                    default:
                        process = "explorer.exe";
                        break;
                }

                path = path.Replace("/", "\\");

                if (!string.IsNullOrEmpty(process))
                {
                    System.Diagnostics.Process.Start(process, path);
                }
            }
            else
            {
                Debug.LogError("No Find: " + path);
            }
        }
    }
}