using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace UnityEditor.Window
{
    public class Md5Tools
    {
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
                        key = root + items[i].folder + "/" + items[i].name;

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
                            key = root + items[i].folder + "/" + items[i].name;

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

            Dictionary<string, string> history = new Dictionary<string, string>();

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
                        if (history.ContainsKey(param[0]))
                        {
                            history[param[0]] = param[1];
                        }
                        else
                        {
                            history.Add(param[0], param[1]);
                        }
                    }
                    line = reader.ReadLine();
                }
            }
            foreach (var line in history)
            {
                lines.Add(string.Join("|", line.Key, line.Value));
            }
            lines.Sort((a, b) =>
            {
                return a.CompareTo(b);
            });
            File.WriteAllLines(path, lines);
        }
    }
}