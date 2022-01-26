using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Encoding = System.Text.Encoding;

namespace Game
{
    public static partial class Utility
    {
        public static class Encoding1
        {
            public static readonly System.Text.Encoding BIGENDIANUNICODE = System.Text.Encoding.BigEndianUnicode;

            public static readonly System.Text.Encoding CHINESE = System.Text.Encoding.GetEncoding("GB2312");

            public static readonly System.Text.Encoding TRADITIONALCHINESE = System.Text.Encoding.GetEncoding("Big5");

            public static readonly System.Text.Encoding JAPANESE = System.Text.Encoding.GetEncoding("Shift-JIS");

            

            public static void Convert(string path, Encoding src, Encoding dst)
            {
                if (!File.Exists(path)) return;

                try
                {
                    File.WriteAllText(path, File.ReadAllText(path, src), dst);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
    }
}
