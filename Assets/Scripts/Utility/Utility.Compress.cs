using ICSharpCode.SharpZipLib.GZip;
using System;
using System.IO;
using UnityEngine;

namespace Game
{
    public static partial class Utility
    {
        public static class _Compress
        {
            const int Level = 6;

            const int Size = 1024 * 4;

            public static bool Compress(byte[] buffer, string output)
            {
                if (File.Exists(output))
                {
                    File.Delete(output);
                }
                FileStream stream = new FileStream(output, FileMode.OpenOrCreate);

                try
                {
                    using (GZipOutputStream zip = new GZipOutputStream(stream))
                    {
                        zip.SetLevel(Level);
                        zip.Write(buffer, 0, buffer.Length);
                        zip.Flush();
                    }
                    return true;
                }
                catch (Exception e)
                {
                    Debuger.LogException(Author.File, e);
                }
                return false;
            }

            public static bool Compress(string input, string output)
            {
                if (File.Exists(input))
                {
                    return Compress(File.ReadAllBytes(input), output);
                }
                return false;
            }

            public static bool Decompress(byte[] bytes, string output)
            {
                if (File.Exists(output))
                {
                    File.Delete(output);
                }
                FileStream stream = new FileStream(output, FileMode.OpenOrCreate);

                MemoryStream memory = new MemoryStream(bytes);

                try
                {
                    using (Stream zip = new GZipInputStream(memory))
                    {
                        byte[] buffer = new byte[Size];

                        int size = 0;

                        while (true)
                        {
                            size = zip.Read(buffer, 0, buffer.Length);

                            if (size > 0)
                            {
                                stream.Write(buffer, 0, size);
                                stream.Flush();
                            }
                            else
                            {
                                break;
                            }
                        }
                        stream.Close();
                    }
                    return true;
                }
                catch (Exception e)
                {
                    Debuger.LogException(Author.File, e);
                }
                return false;
            }

            public static bool Decompress(string input, string output)
            {
                if (File.Exists(input))
                {
                    return Decompress(File.ReadAllBytes(input), output);
                }
                return false;
            }
        }
    }
}