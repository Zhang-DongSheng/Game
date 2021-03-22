using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace UnityEditor
{
    public class Md5Tool : EditorWindow
    {
        private string input_string, input_file;

        private string result;

        [MenuItem("Tools/File/MD5")]
        private static void Open()
        {
            Md5Tool window = EditorWindow.GetWindow<Md5Tool>();
            window.titleContent = new GUIContent("MD5");
            window.minSize = new Vector2(500, 100);
            window.Show();
        }

        private void OnGUI()
        {
            Refresh();
        }

        private void Refresh()
        {
            GUILayout.BeginArea(new Rect(10, 10, Screen.width - 20, Screen.height - 20));
            {
                GUILayout.BeginHorizontal(GUILayout.Height(25));
                {
                    GUILayout.Label("字符串", GUILayout.Width(50));

                    input_string = GUILayout.TextField(input_string);

                    if (GUILayout.Button("确定", GUILayout.Width(60)))
                    {
                        if (string.IsNullOrEmpty(input_string))
                        {
                            ShowNotification(new GUIContent("Error: Empty!"));
                        }
                        else
                        {
                            result = ComputeContent(input_string);
                        }
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal(GUILayout.Height(25));
                {
                    GUILayout.Label("文件", GUILayout.Width(50));

                    if (GUILayout.Button(input_file))
                    {
                        input_file = EditorUtility.OpenFilePanel("Md5", "", "");
                    }
                    if (GUILayout.Button("确定", GUILayout.Width(60)))
                    {
                        result = ComputeFile(input_file);
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal(GUILayout.Height(30));
                {
                    GUILayout.Label("MD5", GUILayout.Width(50));

                    GUILayout.Label(result, new GUIStyle() { alignment = TextAnchor.MiddleCenter, fontSize = 20 });

                    if (GUILayout.Button("复制", GUILayout.Width(60)))
                    {
                        GUIUtility.systemCopyBuffer = result;
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndArea();
        }

        private string ComputeContent(string value)
        {
            string result = string.Empty;

            try
            {
                byte[] buffer = Encoding.Default.GetBytes(value);

                MD5 md5 = new MD5CryptoServiceProvider();

                byte[] hash = md5.ComputeHash(buffer);

                foreach (byte v in hash)
                {
                    result += Convert.ToString(v, 16);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
            return result;
        }

        private string ComputeFile(string path)
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
    }
}