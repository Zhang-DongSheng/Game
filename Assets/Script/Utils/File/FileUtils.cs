using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Encrypt;

namespace Game.Utils
{
    public static class FileUtils
    {
        public static void FolderCreate(string path, bool delete = false)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    if (delete)
                    {
                        Directory.Delete(path); Directory.CreateDirectory(path);
                    }
                }
                else
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception e)
            {
                Debuger.LogError(Author.File, e.Message);
            }
        }

        public static void FolderDelete(string path, bool recursive = true)
        {
            if (Directory.Exists(path))
            {
                try
                {
                    Directory.Delete(path, recursive);
                }
                catch (Exception e)
                {
                    Debuger.LogError(Author.File, e.Message);
                }
            }
        }

        public static bool FolerExists(string path, out string folder)
        {
            if (string.IsNullOrEmpty(Path.GetExtension(path)))
            {
                folder = path;
            }
            else
            {
                folder = Path.GetDirectoryName(path);
            }
            if (Directory.Exists(folder))
            {
                return true;
            }
            else if (File.Exists(folder))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static float FolderSize(string path)
        {
            float size = 0;

            if (Directory.Exists(path))
            {
                DirectoryInfo dir = new DirectoryInfo(path);

                foreach (FileInfo file in dir.GetFiles())
                {
                    size += file.Length;
                }
            }
            return size;
        }

        public static string Read(string path)
        {
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }
            return string.Empty;
        }

        public static string ReadEncrypt(string path, EncryptType encrypt = EncryptType.AES)
        {
            if (File.Exists(path))
            {
                byte[] buffer = File.ReadAllBytes(path);

                return FileEncrypt.Encrypt(Encoding.Default.GetString(buffer), encrypt);
            }
            return string.Empty;
        }

        public static void Write(string path, string content)
        {
            try
            {
                if (!FolerExists(path, out string folder))
                {
                    Directory.CreateDirectory(folder);
                }
                File.WriteAllText(path, content);
            }
            catch (Exception e)
            {
                Debuger.LogError(Author.File, e.Message);
            }
        }

        public static void WriteAppend(string path, params string[] content)
        {
            if (File.Exists(path))
            {
                File.AppendAllLines(path, content);
            }
            else
            {
                if (!FolerExists(path, out string folder))
                {
                    Directory.CreateDirectory(folder);
                }
                using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate))
                {
                    StreamWriter writer = new StreamWriter(stream);

                    for (int i = 0; i < content.Length; i++)
                    {
                        writer.WriteLine(content[i]);
                    }
                    writer.Flush(); writer.Dispose();
                }
            }
        }

        public static void WriteEncrypt(string path, string content, EncryptType encrypt = EncryptType.AES)
        {
            try
            {
                byte[] buffer = Encoding.Default.GetBytes(FileEncrypt.Decrypt(content, encrypt));

                if (!FolerExists(path, out string folder))
                {
                    Directory.CreateDirectory(folder);
                }
                File.WriteAllBytes(path, buffer);
            }
            catch (Exception e)
            {
                Debuger.LogError(Author.File, e.Message);
            }
        }

        public static void Copy(string src, string dst)
        {
            if (File.Exists(src))
            {
                if (!FolerExists(dst, out string folder))
                {
                    Directory.CreateDirectory(folder);
                }
                File.Copy(src, dst, true);
            }
        }

        public static void Move(string src, string dst)
        {
            if (File.Exists(src))
            {
                if (!FolerExists(dst, out string folder))
                {
                    Directory.CreateDirectory(folder);
                }
                File.Move(src, dst);
            }
        }

        public static void Delete(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public static string UnityToSystem(string path)
        {
            if (path.StartsWith("Assets/"))
            {
                return string.Format("{0}{1}", Application.dataPath, path.Remove(0, 6));
            }
            return path;
        }

        public static string SystemToUnity(string path)
        {
            path = string.Format("Assets{0}", path.Remove(0, Application.dataPath.Length));

            path = path.Replace('\\', '/');

            return path;
        }

        public static string New(string path)
        {
            string directory = Path.GetDirectoryName(path);

            string file = Path.GetFileNameWithoutExtension(path);

            string extension = Path.GetExtension(path);

            int index = 0;

            path = string.Format("{0}/{1}_{2}{3}", directory, file, index++, extension);

            while (File.Exists(path))
            {
                path = string.Format("{0}/{1}_{2}{3}", directory, file, index++, extension);
            }
            return path;
        }
    }
}