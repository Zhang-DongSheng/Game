using Boo.Lang;
using System;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

namespace UnityEditor
{
    public class MD5Builder : EditorWindow
    {
        private readonly List<string> extension = new List<string>()
        {
            ".txt",
            ".jpg",
            ".png",
            ".mp3",
        };

        private readonly string OutputFolder = "MD5";

        private readonly string OutputFileName = "md5file";

        private readonly string IgnoreKey = "E:/Branch/branches/Assets/AssetRes/";

        private Rect rect_folder, rect_file;

        private string input_folder, input_file;

        private string key, value;

        [MenuItem("Tools/File/Md5")]
        private static void Open()
        {
            MD5Builder window = EditorWindow.GetWindow<MD5Builder>();
            window.titleContent = new GUIContent("Md5 Builder");
            window.minSize = new Vector2(500, 100);
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Folder:", GUILayout.Width(50));

                rect_folder = EditorGUILayout.GetControlRect(GUILayout.Width(Screen.width - 247));

                input_folder = EditorGUI.TextField(rect_folder, input_folder);

                if ((Event.current.type == EventType.DragUpdated || Event.current.type == EventType.DragExited) && rect_folder.Contains(Event.current.mousePosition))
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Generic;

                    if (DragAndDrop.paths != null && DragAndDrop.paths.Length > 0)
                    {
                        input_folder = Application.dataPath + DragAndDrop.paths[0].Remove(0, 6);
                    }
                }

                if (GUILayout.Button("Compute"))
                {
                    ComputeFolder(input_folder);
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("File:", GUILayout.Width(50));

                rect_file = EditorGUILayout.GetControlRect(GUILayout.Width(Screen.width - 247));

                input_file = EditorGUI.TextField(rect_file, input_file);

                if ((Event.current.type == EventType.DragUpdated || Event.current.type == EventType.DragExited) && rect_file.Contains(Event.current.mousePosition))
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Generic;

                    if (DragAndDrop.paths != null && DragAndDrop.paths.Length > 0)
                    {
                        input_file = Application.dataPath + DragAndDrop.paths[0].Remove(0, 6);
                    }
                }

                if (GUILayout.Button("Compute"))
                {
                    ComputeFile(input_file);
                }
            }
            GUILayout.EndHorizontal();
        }

        private void ComputeFolder(string path)
        {
            if (Directory.Exists(path))
            {
                try
                {
                    FileStream stream = new FileStream(NewFile, FileMode.OpenOrCreate);

                    StreamWriter writer = new StreamWriter(stream);

                    DirectoryInfo dir = new DirectoryInfo(path);

                    foreach (var folder in dir.GetDirectories())
                    {
                        ComputeFolder(ref writer, folder.FullName);
                    }

                    foreach (var file in dir.GetFiles())
                    {
                        if (extension.Contains(file.Extension))
                        {
                            key = GetKey(file.FullName); value = GetMd5(file.FullName);

                            writer.WriteLine(string.Format("{0}|{1}", key, value));
                        }
                    }

                    writer.Dispose();

                    stream.Dispose();
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
                finally
                {
                    AssetDatabase.Refresh();
                }
            }

            ShowNotification(new GUIContent("Compute File Completed!"));
        }

        private void ComputeFolder(ref StreamWriter writer, string path)
        {
            if (Directory.Exists(path))
            {
                DirectoryInfo dir = new DirectoryInfo(path);

                foreach (var folder in dir.GetDirectories())
                {
                    ComputeFolder(ref writer, folder.FullName);
                }

                foreach (var file in dir.GetFiles())
                {
                    if (extension.Contains(file.Extension))
                    {
                        key = GetKey(file.FullName); value = GetMd5(file.FullName);

                        writer.WriteLine(string.Format("{0}|{1}", key, value));
                    }
                }
            }
        }

        private void ComputeFile(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    FileStream stream = new FileStream(NewFile, FileMode.OpenOrCreate);

                    StreamWriter writer = new StreamWriter(stream);

                    key = GetKey(path); value = GetMd5(path);

                    writer.WriteLine(string.Format("{0}|{1}", key, value));

                    writer.Dispose();

                    stream.Dispose();
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
                finally
                {
                    AssetDatabase.Refresh();
                }
            }

            ShowNotification(new GUIContent("Compute File Completed!"));
        }

        private string GetKey(string path)
        {
            string key = path.Replace("\\", "/").Replace(@"\", "/");

            if (key.StartsWith(IgnoreKey))
            {
                key = key.Remove(0, IgnoreKey.Length);
            }

            return key;
        }

        private string GetMd5(string path)
        {
            string result = string.Empty;

            try
            {
                if (File.Exists(path))
                {
                    byte[] buffer = File.ReadAllBytes(path);

                    MD5 md5 = new MD5CryptoServiceProvider();

                    byte[] hash = md5.ComputeHash(buffer);

                    foreach (byte v in hash)
                    {
                        result += Convert.ToString(v, 16);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }

            return result;
        }

        private string NewFile
        {
            get
            {
                string folder = Path.Combine(Application.dataPath, OutputFolder);

                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                string file = string.Format("{0}/{1}.txt", folder, OutputFileName);

                int index = 0;

                while (File.Exists(file))
                {
                    file = string.Format("{0}/{1}_{2}.txt", folder, OutputFileName, index++);
                }

                return file;
            }
        }
    }
}
