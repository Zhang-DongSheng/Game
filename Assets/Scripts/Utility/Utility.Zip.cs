using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public static partial class Utility
    {
        public static class Zip
        {
            public static void CreateFromDirectory(string src, string dst)
            {
                ZipFile.CreateFromDirectory(src, dst);
            }

            public static void ExtractToDirectory(string src, string dst)
            {
                ZipFile.ExtractToDirectory(src, dst);
            }
        }
    }
}