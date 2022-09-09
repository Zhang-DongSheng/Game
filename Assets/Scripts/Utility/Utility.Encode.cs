using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace Game
{
    public static partial class Utility
    {
        /// <summary>
        /// ±àÂë
        /// </summary>
        public static class Encode
        {
            public static readonly Encoding UTF8 = new UTF8Encoding(false);

            public static readonly Encoding BIGENDIANUNICODE = Encoding.BigEndianUnicode;

            public static readonly Encoding CHINESE = Encoding.GetEncoding("GB2312");

            public static readonly Encoding TRADITIONALCHINESE = Encoding.GetEncoding("Big5");

            public static readonly Encoding JAPANESE = Encoding.GetEncoding("Shift-JIS");

            public static Encoding FileEncoding(string path)
            {
                var buffer = new byte[4];

                using (var file = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    file.Read(buffer, 0, 4);
                }
                if (buffer[0] == 0x2b && buffer[1] == 0x2f && buffer[2] == 0x76) return Encoding.UTF7;
                if (buffer[0] == 0xef && buffer[1] == 0xbb && buffer[2] == 0xbf) return Encoding.UTF8;
                if (buffer[0] == 0xff && buffer[1] == 0xfe) return Encoding.Unicode;
                if (buffer[0] == 0xfe && buffer[1] == 0xff) return Encoding.BigEndianUnicode;
                if (buffer[0] == 0 && buffer[1] == 0 && buffer[2] == 0xfe && buffer[3] == 0xff) return Encoding.UTF32;
                return Encoding.ASCII;
            }

            public static void Convert(string path, Encoding src, Encoding dst)
            {
                if (!File.Exists(path))
                {
                    return;
                }
                try
                {
                    File.WriteAllText(path, File.ReadAllText(path, src), dst);
                }
                catch (Exception e)
                {
                    Debuger.LogException(Author.Utility, e);
                }
            }
        }
    }
}