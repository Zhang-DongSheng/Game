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
                var encoding = Encoding.Default;

                if (!File.Exists(path)) return encoding;

                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    StreamReader reader = new StreamReader(stream);

                    encoding = reader.CurrentEncoding;

                    reader.Dispose();
                }
                return encoding;
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