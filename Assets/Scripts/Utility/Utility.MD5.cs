using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Game
{
    public static partial class Utility
    {
        public static class _Md5
        {
            private static readonly StringBuilder builder = new StringBuilder();

            public static string ComputeContent(string value)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    return ComputeBytes(Encoding.Default.GetBytes(value));
                }
                return string.Empty;
            }

            public static string ComputeFile(string path)
            {
                if (File.Exists(path))
                {
                    return ComputeBytes(File.ReadAllBytes(path));
                }
                return string.Empty;
            }

            public static string ComputeObject(object obj)
            {
                byte[] buffer = null;

                using (MemoryStream stream = new MemoryStream())
                {
                    BinaryFormatter binary = new BinaryFormatter();
                    binary.Serialize(stream, obj);
                    buffer = stream.ToArray();
                }
                if (buffer != null && buffer.Length > 0)
                {
                    return ComputeBytes(buffer);
                }
                return string.Empty;
            }

            public static string ComputeBytes(byte[] buffer)
            {
                try
                {
                    System.Security.Cryptography.MD5 md5 = new MD5CryptoServiceProvider();

                    byte[] hash = md5.ComputeHash(buffer);

                    builder.Clear();

                    foreach (byte v in hash)
                    {
                        builder.Append(Convert.ToString(v, 16));
                    }
                }
                catch (Exception e)
                {
                    Debuger.LogError(Author.Utility, e.Message);
                }
                return builder.ToString();
            }
        }
    }
}