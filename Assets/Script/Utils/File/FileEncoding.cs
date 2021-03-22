using System;
using System.IO;
using System.Text;

namespace Utils
{
    public static class FileEncoding
    {
        public static readonly Encoding DEFAULT = Encoding.Default;

        public static readonly Encoding ASCII = Encoding.ASCII;

        public static readonly Encoding UTF7 = Encoding.UTF7;

        public static readonly Encoding UTF8 = Encoding.UTF8;

        public static readonly Encoding UTF32 = Encoding.UTF32;

        public static readonly Encoding UNICODE = Encoding.Unicode;

        public static readonly Encoding BIGENDIANUNICODE = Encoding.BigEndianUnicode;

        public static readonly Encoding CHINESE = Encoding.GetEncoding("GB2312");

        public static readonly Encoding TRADITIONALCHINESE = Encoding.GetEncoding("Big5");

        public static readonly Encoding JAPANESE = Encoding.GetEncoding("Shift-JIS");

        public static Encoding Get(string path)
        {
            var buffer = new byte[4];

            using (var file = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                file.Read(buffer, 0, 4);
            }

            if (buffer[0] == 0x2b && buffer[1] == 0x2f && buffer[2] == 0x76) return UTF7;
            if (buffer[0] == 0xef && buffer[1] == 0xbb && buffer[2] == 0xbf) return UTF8;
            if (buffer[0] == 0xff && buffer[1] == 0xfe) return UNICODE;
            if (buffer[0] == 0xfe && buffer[1] == 0xff) return BIGENDIANUNICODE;
            if (buffer[0] == 0 && buffer[1] == 0 && buffer[2] == 0xfe && buffer[3] == 0xff) return UTF32;

            return ASCII;
        }

        public static void Convert(string path, Encoding source, Encoding target)
        {
            if (!File.Exists(path)) return;

            try
            {
                File.WriteAllText(path, File.ReadAllText(path, source), target);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}