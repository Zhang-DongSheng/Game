using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace Game
{
    public static partial class Utility
    {
        /// <summary>
        /// ÎÄµµ
        /// </summary>
        public static class Document
        {
            public static void CreateDirectory(string path, bool delete = false)
            {
                try
                {
                    if (Directory.Exists(path))
                    {
                        if (delete)
                        {
                            Directory.Delete(path, true);
                        }
                        else
                        {
                            return;
                        }
                    }
                    else if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                    Directory.CreateDirectory(path);
                }
                catch (Exception e)
                {
                    Debuger.LogException(Author.File, e);
                }
            }

            public static void CreateDirectoryByFile(string path, bool delete = false)
            {
                try
                {
                    FileInfo file = new FileInfo(path);

                    DirectoryInfo directory = file.Directory;

                    if (directory.Exists)
                    {
                        if (delete)
                        {
                            directory.Delete();
                        }
                        else
                        {
                            return;
                        }
                    }
                    Directory.CreateDirectory(directory.FullName);
                }
                catch (Exception e)
                {
                    Debuger.LogException(Author.File, e);
                }
            }

            public static bool Exist(string path)
            {
                if (Directory.Exists(path))
                {
                    return true;
                }
                else if (File.Exists(path))
                {
                    return true;
                }
                return false;
            }

            public static float Size(string path)
            {
                float size = 0;

                if (File.Exists(path))
                {
                    FileInfo info = new FileInfo(path);

                    size = info.Length;
                }
                else if (Directory.Exists(path))
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

                    return Cryptogram.Encrypt(Encoding.Default.GetString(buffer), encrypt);
                }
                return string.Empty;
            }

            public static void Write(string path, string content)
            {
                try
                {
                    CreateDirectory(Path.GetDirectoryName(path));

                    File.WriteAllText(path, content);
                }
                catch (Exception e)
                {
                    Debuger.LogException(Author.File, e);
                }
            }

            public static void Write(string path, byte[] buffer)
            {
                try
                {
                    CreateDirectory(Path.GetDirectoryName(path));

                    File.WriteAllBytes(path, buffer);
                }
                catch (Exception e)
                {
                    Debuger.LogException(Author.File, e);
                }
            }

            public static void WriteEncrypt(string path, string content, EncryptType encrypt = EncryptType.AES)
            {
                try
                {
                    byte[] buffer = Encoding.Default.GetBytes(Cryptogram.Decrypt(content, encrypt));

                    CreateDirectory(Path.GetDirectoryName(path));

                    File.WriteAllBytes(path, buffer);
                }
                catch (Exception e)
                {
                    Debuger.LogException(Author.File, e);
                }
            }

            public static void Append(string path, params string[] content)
            {
                if (File.Exists(path))
                {
                    File.AppendAllLines(path, content);
                }
                else
                {
                    CreateDirectory(Path.GetDirectoryName(path));

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

            public static void Copy(string src, string dst)
            {
                if (Directory.Exists(src))
                {
                    DirectoryInfo directory = new DirectoryInfo(src);

                    CreateDirectory(dst);

                    DirectoryInfo[] directories = directory.GetDirectories("*", SearchOption.AllDirectories);

                    for (int i = 0; i < directories.Length; i++)
                    {
                        string path = Path.Replace(directories[i].FullName, src, dst);

                        CreateDirectory(path);
                    }
                    FileInfo[] files = directory.GetFiles("*", SearchOption.AllDirectories);

                    for (int i = 0; i < files.Length; i++)
                    {
                        string path = Path.Replace(files[i].FullName, src, dst);

                        File.Copy(files[i].FullName, path, true);
                    }
                }
                else if (File.Exists(src))
                {
                    CreateDirectoryByFile(dst);

                    File.Copy(src, dst, true);
                }
            }

            public static void Move(string src, string dst)
            {
                if (File.Exists(src))
                {
                    CreateDirectoryByFile(dst);

                    File.Move(src, dst);
                }
                else if (Directory.Exists(src))
                {
                    Directory.Move(src, dst);
                }
            }

            public static void Delete(string path)
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                else if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
            }

            public static void Rename(string src, string dst)
            {
                Move(src, dst);
            }
        }
    }
}