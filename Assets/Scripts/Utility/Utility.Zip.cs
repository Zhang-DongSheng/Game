using ICSharpCode.SharpZipLib.Checksum;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            public static void CompressFile(string src, string dst, int level = 5, string password = null)
            {
                byte[] buffer = File.ReadAllBytes(src);

                Document.CreateDirectory(Path.GetDirectoryName(dst));

                using (var stream = new FileStream(dst, FileMode.OpenOrCreate))
                {
                    using (var zip = new ZipOutputStream(stream))
                    {
                        if (!string.IsNullOrEmpty(password))
                        {
                            zip.Password = password;
                        } 
                        zip.PutNextEntry(new ZipEntry(Path.GetFileName(src)));
                        zip.SetLevel(level);
                        zip.Write(buffer, 0, buffer.Length);
                    }
                }
            }

            public static void CompressDirectory(string src, string dst, int level = 5, string password = null)
            {
                //Crc32 crc = new Crc32();

                //string[] filenames = Directory.GetFileSystemEntries(strDirectory);

                //foreach (string file in filenames)
                //{
                //    if (Directory.Exists(file))
                //    {
                //        string pPath = parentPath;
                //        pPath += file.Substring(file.LastIndexOf("/") + 1);
                //        pPath += "/";
                //        ZipSetp(file, s, pPath);
                //    }
                //    else
                //    {
                //        using (FileStream fs = File.OpenRead(file))
                //        {
                //            byte[] buffer = new byte[fs.Length];
                //            fs.Read(buffer, 0, buffer.Length);

                //            string fileName = parentPath + file.Substring(file.LastIndexOf("/") + 1);
                //            ZipEntry entry = new ZipEntry(fileName);

                //            entry.DateTime = DateTime.Now;
                //            entry.Size = fs.Length;

                //            fs.Close();

                //            crc.Reset();
                //            crc.Update(buffer);

                //            entry.Crc = crc.Value;
                //            s.PutNextEntry(entry);

                //            s.Write(buffer, 0, buffer.Length);
                //        }
                //    }
                //}
            }

            public static void Uncompress(string src, string dst, string password)
            {
                if (File.Exists(src))
                {

                }
                else
                {
                    Debuger.LogError(Author.File, $"File {src} not exist!");
                }
            }
        }
    }
}