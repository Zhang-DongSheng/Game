using Game;
using System.IO;
using System.Text;

namespace UnityEditor.Listener
{
    public class AssetModificationListener : AssetModificationProcessor
    {
        public static void OnWillCreateAsset(string path)
        {
            if (path.EndsWith(".meta"))
            {
                path = path.Replace(".meta", null);
            }

            string extension = Path.GetExtension(path);

            switch (extension)
            {
                case ".cs":
                    ScriptEncoding(path);
                    break;
                default:
                    break;
            }
        }

        private static void ScriptEncoding(string path)
        {
            Encoding encoding = Utility.Encode.FileEncoding(path);

            Encoding UTF8 = new UTF8Encoding(false);

            if (encoding != UTF8)
            {
                File.WriteAllText(path, File.ReadAllText(path), UTF8);
            }
        }
    }
}