﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace UnityEditor
{
    public class MD5Tools : EditorWindow
    {
        private string input_string, input_file;

        private string result;

        [MenuItem("Tools/File/MD5")]
        protected static void Open()
        {
            MD5Tools window = EditorWindow.GetWindow<MD5Tools>();
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

        public static string ComputeContent(string value)
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

        public static string ComputeFile(string path)
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

        public static void Record(string path, string root, List<ItemFile> items)
        {
            string key, value;

            if (File.Exists(path))
            {
                StreamWriter writer = new StreamWriter(path, true);

                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i].select)
                    {
                        key = root + items[i].root + "/" + items[i].name;

                        value = ComputeFile(items[i].path);

                        writer.WriteLine(string.Format("{0}|{1}", key, value));
                    }
                }
                writer.Dispose();
            }
            else
            {
                using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate))
                {
                    StreamWriter writer = new StreamWriter(stream);

                    for (int i = 0; i < items.Count; i++)
                    {
                        if (items[i].select)
                        {
                            key = root + items[i].root + "/" + items[i].name;

                            value = ComputeFile(items[i].path);

                            writer.WriteLine(string.Format("{0}|{1}", key, value));
                        }
                    }
                    writer.Dispose();
                };
            }
        }

        public static void Recompilation(string path)
        {
            if (!File.Exists(path)) return;

            Dictionary<string, string> md5 = new Dictionary<string, string>();

            List<string> lines = new List<string>();

            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                StreamReader reader = new StreamReader(stream);

                string line = reader.ReadLine();

                string[] param = new string[2];

                while (!string.IsNullOrEmpty(line))
                {
                    param = line.Split('|');

                    if (param.Length == 2)
                    {
                        if (md5.ContainsKey(param[0]))
                        {
                            md5[param[0]] = param[1];
                        }
                        else
                        {
                            md5.Add(param[0], param[1]);
                        }
                    }
                    line = reader.ReadLine();
                }
            }
            foreach (var line in md5)
            {
                lines.Add(string.Join("|", line.Key, line.Value));
            }
            File.WriteAllLines(path, lines);
        }
    }
}