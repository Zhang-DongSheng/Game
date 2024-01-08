using ICSharpCode.SharpZipLib.Checksum;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;
using UnityEngine;

namespace Game
{
    public static partial class Utility
    {
        public static class Zip
        {
            public static void Compress(string src, string dst, int level = 5, string password = null)
            {
                if (File.Exists(src))
                {
                    CompressFile(src, dst, level, password);
                }
                else if (Directory.Exists(src))
                {
                    CompressDirectory(src, dst, level, password);
                }
                else
                {
                    Debuger.LogError(Author.File, $"File {src} not exist!");
                }
            }

            private static void CompressFile(string src, string dst, int level = 5, string password = null)
            {
                Document.CreateDirectory(Path.GetDirectoryName(dst));

                byte[] buffer = File.ReadAllBytes(src);

                using (var stream = new FileStream(dst, FileMode.OpenOrCreate))
                {
                    using (var zip = new ZipOutputStream(stream))
                    {
                        if (!string.IsNullOrEmpty(password))
                        {
                            zip.Password = password;
                        }
                        zip.SetLevel(level);

                        string entry = Path.GetFileName(src);

                        zip.PutNextEntry(new ZipEntry(entry));

                        zip.Write(buffer, 0, buffer.Length);
                    }
                }
            }

            private static void CompressDirectory(string src, string dst, int level = 5, string password = null)
            {
                Document.CreateDirectory(Path.GetDirectoryName(dst));

                using (var stream = new FileStream(dst, FileMode.OpenOrCreate))
                {
                    using (var zip = new ZipOutputStream(stream))
                    {
                        if (!string.IsNullOrEmpty(password))
                        {
                            zip.Password = password;
                        }
                        zip.SetLevel(level);

                        CompressDirectory(src, zip, string.Empty);
                    }
                }
            }

            private static void CompressDirectory(string src, ZipOutputStream zip, string parent)
            {
                src = Path.DirectoryEndWithSeparatorChar(src);

                Crc32 crc = new Crc32();

                string[] files = Directory.GetFiles(src);

                foreach (string file in files)
                {
                    byte[] buffer = File.ReadAllBytes(file);

                    int index = file.LastIndexOf("\\");

                    string entry = parent + file.Substring(index + 1);

                    ZipEntry ze = new ZipEntry(entry)
                    {
                        DateTime = DateTime.Now,
                        Size = buffer.Length,
                    };
                    crc.Reset();

                    crc.Update(buffer);

                    ze.Crc = crc.Value;

                    zip.PutNextEntry(ze);

                    zip.Write(buffer, 0, buffer.Length);
                }
                // вснд╪Ч╪п
                string[] directories = Directory.GetDirectories(src);

                foreach (string directory in directories)
                {
                    string root = parent;

                    int index = directory.LastIndexOf("\\");

                    root += directory.Substring(index + 1);

                    root += "\\";

                    CompressDirectory(directory, zip, root);
                }
            }

            public static void Uncompress(string src, string dst, string password)
            {
                if (File.Exists(src))
                {
                    Document.CreateDirectory(dst);

                    dst = Path.DirectoryEndWithSeparatorChar(dst);

                    ZipEntry ent = null;

                    using (var zipStream = new ZipInputStream(File.OpenRead(src)))
                    {
                        if (!string.IsNullOrEmpty(password))
                        {
                            zipStream.Password = password;
                        }
                        while ((ent = zipStream.GetNextEntry()) != null)
                        {
                            if (ent.IsDirectory)
                            {
                                continue;
                            }
                            if (string.IsNullOrEmpty(ent.Name))
                            {
                                continue;
                            }
                            string fileName = dst + ent.Name.Replace('/', '\\');
                            var index = ent.Name.LastIndexOf('/');
                            if (index != -1)
                            {
                                string path = dst + ent.Name.Substring(0, index).Replace('/', '\\');
                                Directory.CreateDirectory(path);
                            }
                            var bytes = new byte[ent.Size];
                            zipStream.Read(bytes, 0, bytes.Length);
                            File.WriteAllBytes(fileName, bytes);
                        }
                    }
                }
                else
                {
                    Debuger.LogError(Author.File, $"File {src} not exist!");
                }
            }
        }
    }
}